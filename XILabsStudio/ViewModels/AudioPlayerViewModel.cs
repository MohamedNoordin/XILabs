using Plugin.Maui.Audio;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API.DataModels;
using System.Diagnostics;
using XILabsStudio.Resources.Strings;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using System.Timers;

namespace XILabsStudio.ViewModels
{
    [ObservableObject]
    public partial class AudioPlayerViewModel: IDisposable
    {
        private System.Timers.Timer updateTimer;
        private bool disposed = false;
        private Stream speechStream;
        private IAudioPlayer audioPlayer;

        [ObservableProperty]
        private Voice voice;

        [ObservableProperty]
        private string currentDateTime;

        [ObservableProperty]
        private string currentPositionFormatted;

        [ObservableProperty]
        private double duration;

        [ObservableProperty]
        private double currentPosition;

        [ObservableProperty]
        private string playPauseIconSource;

        [ObservableProperty]
        private bool isPlaying;

        public AudioPlayerViewModel(Stream speechStream, Voice voice)
        {
            updateTimer = new System.Timers.Timer(TimeSpan.FromMilliseconds(16));
            updateTimer.Elapsed += UpdatePlaybackView;
            updateTimer.AutoReset = true;

            this.speechStream = speechStream;
            this.Voice = voice;
            this.CurrentDateTime = DateTime.Now.ToString("MM.dd.yy, hh:mm");
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            speechStream.Seek(0, SeekOrigin.Begin);
            audioPlayer = AudioManager.Current.CreatePlayer(speechStream);
            audioPlayer.Play();
            PlayPauseIconSource = "pause_audio.png";
            IsPlaying = true;
            audioPlayer.PlaybackEnded += ((s, e) =>
            {
                IsPlaying = false;
                PlayPauseIconSource = "play_audio.png";
                audioPlayer.Seek(0);
            });
            updateTimer.Start();
        }

        [RelayCommand]
        private async Task PlayPauseAsync()
        {
            if (audioPlayer is not null)
            {
                // Stopped/paused
                if (!audioPlayer.IsPlaying)
                {
                    audioPlayer.Play();
                    PlayPauseIconSource = "pause_audio.png";
                    IsPlaying = true;
                    updateTimer.Start();
                }
                else // Playing
                {
                    audioPlayer.Pause();
                    PlayPauseIconSource = "play_audio.png";
                    IsPlaying = false;
                    updateTimer.Stop();
                }
            }
            else
            {
                speechStream.Seek(0, SeekOrigin.Begin);
                audioPlayer = AudioManager.Current.CreatePlayer(speechStream);
                audioPlayer.Play();
                PlayPauseIconSource = "pause_audio.png";
                IsPlaying = true;
                audioPlayer.PlaybackEnded += ((s, e) =>
                {
                    IsPlaying = false;
                    PlayPauseIconSource = "play_audio.png";
                    audioPlayer.Seek(0);
                    updateTimer.Stop();
                });
                updateTimer.Start();
            }
        }

        [RelayCommand]
        private void Rewind()
        {
            if (audioPlayer is not null && audioPlayer.CanSeek)
                audioPlayer.Seek(audioPlayer.CurrentPosition - 10);
        }

        [RelayCommand]
        private void FastForward()
        {
            if (audioPlayer is not null && audioPlayer.CanSeek)    
                audioPlayer.Seek(audioPlayer.CurrentPosition + 10);
        }

        [RelayCommand]
        private async Task DownloadAudioAsync()
        {
if (DeviceInfo.Platform == DevicePlatform.Android
                && (await Permissions.CheckStatusAsync<Permissions.Media>() != PermissionStatus.Granted || await Permissions.CheckStatusAsync<Permissions.StorageWrite>() != PermissionStatus.Granted))
            await Shell.Current.DisplayAlert(XIResources.OnAndroid_AskForPermissionTitle, XIResources.OnAndroid_AskForPermissionMessage, "OK");

            speechStream.Seek(0, SeekOrigin.Begin);
            var fileSaverResult = await FileSaver.Default.SaveAsync($"XILabs {Voice.Title} {CurrentDateTime}.mp3", speechStream, new CancellationToken());
            if (fileSaverResult.IsSuccessful)
                await Toast.Make(XIResources.AudioPlayerPopup_DownloadAudio_FileSaved).Show();
        }

        [RelayCommand]
        private async Task ShareAudioAsync()
        {
            using (var fileStream = System.IO.File.Create(Path.Combine(FileSystem.CacheDirectory, $"XILabs {Voice.Title.Replace('/', '_')} {CurrentDateTime.Replace(':', '_')}.mp3")))
            {
                speechStream.Seek(0, SeekOrigin.Begin);
                await speechStream.CopyToAsync(fileStream);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = $"XILabs {Voice.Title.Replace('/', '_')} {CurrentDateTime.Replace(':', '_')}.mp3",
                    File = new ShareFile(fileStream.Name)
                });
                    }
        }

        [RelayCommand]
        private void UpdatePlaybackPosition() =>
            audioPlayer.Seek(CurrentPosition);

        void UpdatePlaybackView(object sender, ElapsedEventArgs e)
        {
            if (!IsPlaying) return;

                    CurrentPosition = audioPlayer.CurrentPosition;
                    Duration = audioPlayer.Duration;
                    CurrentPositionFormatted = $@"{TimeSpan.FromSeconds(CurrentPosition).ToString("mm\\:ss")}/{TimeSpan.FromSeconds(Duration).ToString("mm\\:ss")}";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    updateTimer.Dispose();
                    audioPlayer.Dispose();
                    speechStream.Dispose();
                }

                disposed = true;
            }
        }

        // Public method for consumers to call when they're done with the object
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Suppress finalization to avoid unnecessary overhead
        }

        // Finalizer (destructor) to release resources if Dispose wasn't called explicitly
        ~AudioPlayerViewModel()
        {
            Dispose(false);
        }
    }
}
