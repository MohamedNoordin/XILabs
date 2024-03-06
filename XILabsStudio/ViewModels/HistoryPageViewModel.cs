using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;
using XILabsStudio.API.DataModels;
using XILabsStudio.Messages;
using XILabsStudio.Popups;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio.ViewModels
{
    [ObservableObject]
    public partial class HistoryPageViewModel
    {
        private XIOpenAPI xi;
        private HistoryAPI hAPI;
        private HistoryResponse history;

        [ObservableProperty]
        private ObservableCollection<HistoryPage> historyPages;

        [ObservableProperty]
        private HistoryPage currentPage;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isPlaying;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isAllSelected;

        [ObservableProperty]
        private bool isHistorySelected;

        [ObservableProperty]
        private int currentPageNumber;

        [ObservableProperty]
        private int selectedItemsCount;

        public HistoryPageViewModel()
        {
            HistoryPages = new ObservableCollection<HistoryPage>();
            CurrentPageNumber = 1;
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            IsLoading = true;

                xi = await XIOpenAPI.InitializeAsync();
                hAPI = (HistoryAPI)xi.GetInstanceOf<HistoryAPI>();

                history = await xi.GetHistoryAsync(100);
                var historiesList = history.History.Chunk(10);
                var tempPageNumber = 1;

                foreach (var historiesItem in historiesList)
                {
                    HistoryPages.Add(
                        new HistoryPage
                        {
                            Number = tempPageNumber,
                            Histories = historiesItem.ToObservableCollection()
                        });
                    tempPageNumber++;
                }

                CurrentPageNumber = 1;
                CurrentPage = HistoryPages[CurrentPageNumber - 1]; // Zero-indexed
                PreviousPageCommand.NotifyCanExecuteChanged();
                NextPageCommand.NotifyCanExecuteChanged();

            IsLoading = false;
                }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            history = await xi.GetHistoryAsync(100);
            HistoryPages.Clear();
            var historiesList = history.History.Chunk(10);
            var tempPageNumber = 1;

            foreach (var historiesItem in historiesList)
            {
                HistoryPages.Add(
                    new HistoryPage
                    {
                        Number = tempPageNumber,
                        Histories = historiesItem.ToObservableCollection()
                    });
                tempPageNumber++;
            }

            CurrentPage = HistoryPages[CurrentPageNumber - 1]; // Zero-indexed
            IsAllSelected = false;
            IsRefreshing = false;
            WeakReferenceMessenger.Default.Send(new HistoryChangedMessage(CurrentPage.Histories.First()));
        }

        [RelayCommand(CanExecute = nameof(CanLoadMore))]
        private async Task LoadMoreAsync()
        {
            history = await xi.GetHistoryAsync(100, history.LastHistoryItemId);
            var historiesList = history.History.Chunk(10);

            foreach (var historiesItem in historiesList)
            {
                HistoryPages.Add(
                    new HistoryPage
                    {
                        Number = HistoryPages.Count + 1,
                        Histories = historiesItem.ToObservableCollection()
                    });
            }
            WeakReferenceMessenger.Default.Send(new HistoryChangedMessage(CurrentPage.Histories.First()));
        }

        private bool CanLoadMore() => history.HasMore;

        [RelayCommand(CanExecute = nameof(CanPlay))]
        private async Task PlayAsync(string? itemID)
        {
            var item = CurrentPage.Histories.Where(h => h.HistoryItemId == itemID).FirstOrDefault();
            IsPlaying = true;
            await Shell.Current.ShowPopupAsync(
                new AudioPlayerPopup(
                    await hAPI.GetHistoryAudioAsync(itemID),
                    new Voice {  Name = item.VoiceName, Category = item.VoiceCategory }));

            var maximizeAudioPlayerToolbarItem =
            new ToolbarItem
            {
                Text = XIResources.AudioPlayerPopup_MaximizeAudioPlayer,
                Command = MaximizeAudioPlayerCommand
            };

            if (!Shell.Current.ToolbarItems.Any(i => i.Text == XIResources.AudioPlayerPopup_MaximizeAudioPlayer))
                Shell.Current.ToolbarItems.Add(maximizeAudioPlayerToolbarItem);
            IsPlaying = false;
        }

        [RelayCommand]
        private async Task MaximizeAudioPlayerAsync() =>
            await Shell.Current.ShowPopupAsync(new AudioPlayerPopup(true));

        private bool CanPlay(string? historyItemID) => !IsPlaying;

        [RelayCommand]
        private async void Selection()
        {
            if (IsAllSelected)
            {
                foreach (var page in HistoryPages)
                {
                foreach (var item in page.Histories)
                    item.IsSelected = true;
                }
            }
            else
            {
                foreach (var page in HistoryPages)
                {
                    foreach (var item in page.Histories)
                        item.IsSelected = false;
                }
            }

            DownloadSelectedCommand.NotifyCanExecuteChanged();
            RemoveSelectedCommand.NotifyCanExecuteChanged();
            await UpdateSelectedItemsCountAsync();
        }

        [RelayCommand(CanExecute = nameof(IsAnItemSelected))]
        private async Task DownloadSelectedAsync()
        {
            List<string> selectedItemIDs = new List<string>();
            foreach (var hp in HistoryPages)
            {
                selectedItemIDs.AddRange(hp.Histories.Where(
                    h => h.IsSelected)
                    .Select(i => i.HistoryItemId).ToList());
            }

            await FileSaver.Default.SaveAsync("History.zip",
                await hAPI.DownloadAsync(selectedItemIDs), 
                new CancellationToken());
        }

        [RelayCommand]
        private async Task DownloadThisAsync(object sender)
        {
            var selectedItem = sender as History;
            if (selectedItem is null)
            {
                await Toast.Make(XIResources.SelectAnItem).Show();
                return;
            }

            var selectedItemID = selectedItem.HistoryItemId;
            await FileSaver.Default.SaveAsync("HistoryItem.mp3",
    await hAPI.DownloadAsync(new List<string> { selectedItemID  } ),
    new CancellationToken());
        }

        [RelayCommand(CanExecute = nameof(IsAnItemSelected))]
        private async Task RemoveSelectedAsync()
        {
            if (await Shell.Current.DisplayAlert(XIResources.HistoryPage_DeleteSelectedItemsAlertTitle, XIResources.HistoryPage_DeleteSelectedItemsAlertDescription, XIResources.HistoryPage_DeleteSelectedItemsAlert_Delete, XIResources.Cancel))
            {
                List<string> selectedItemIDs = new List<string>();
                foreach (var hp in HistoryPages)
                {
                    selectedItemIDs.AddRange(hp.Histories.Where(
                        h => h.IsSelected)
                        .Select(i => i.HistoryItemId).ToList());
                }

                await hAPI.RemoveAsync(selectedItemIDs);
                await RefreshAsync();
            }
        }

        [RelayCommand]
        private async Task RemoveThisAsync(object sender)
        {
            var selectedItem = sender as History;
            if (selectedItem is null)
            {
                await Toast.Make(XIResources.SelectAnItem).Show();
                return;
            }

            var selectedItemID = selectedItem.HistoryItemId;
            await hAPI.RemoveAsync(new List<string> { selectedItemID });
            CurrentPage.Histories.Remove(selectedItem);
        }

        [RelayCommand]
        private async Task CopyHistoryTextAsync(object sender)
        {
            var selectedItem = sender as History;
            MainThread.BeginInvokeOnMainThread(async () => await Clipboard.SetTextAsync(selectedItem.Text));
            await Toast.Make(XIResources.CopiedToYourClipboard).Show();
        }

        [RelayCommand]
        private async Task ItemSelectionAsync()
        {
            DownloadSelectedCommand.NotifyCanExecuteChanged();
            RemoveSelectedCommand.NotifyCanExecuteChanged();
            await UpdateSelectedItemsCountAsync();
        }

        private bool IsAnItemSelected()
        {
            if (HistoryPages is null || HistoryPages.Count < 0) return false;

            return
                HistoryPages.Any(
                    hp => hp.Histories.Any(
                        h => h.IsSelected));
        }

        [RelayCommand(CanExecute = nameof(CanGoPrevious))]
        private async Task PreviousPageAsync()
        {
            CurrentPageNumber--;
            CurrentPage = HistoryPages[CurrentPageNumber - 1];
            PreviousPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
            SemanticScreenReader.Announce($"{XIResources.Page} {CurrentPageNumber} / {HistoryPages.Count}");
            await UpdateSelectedItemsCountAsync(true);
        }

        private bool CanGoPrevious() => CurrentPageNumber > 1;

        [RelayCommand(CanExecute = nameof(CanGoNext))]
        private async Task NextPageAsync()
        {
            CurrentPageNumber++;
            CurrentPage = HistoryPages[CurrentPageNumber - 1];
            PreviousPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
            SemanticScreenReader.Announce($"{XIResources.Page} {CurrentPageNumber} / {HistoryPages.Count}");
            await UpdateSelectedItemsCountAsync(true);
        }

        private bool CanGoNext() => CurrentPageNumber < HistoryPages.Count;

        private async Task UpdateSelectedItemsCountAsync(bool transition = false)
        {
            SelectedItemsCount = CurrentPage.Histories.Count(h => h.IsSelected);
            WeakReferenceMessenger.Default.Send(new HistoryChangedMessage(CurrentPage.Histories.First()));
            if (!transition)
                SemanticScreenReader.Announce($"{SelectedItemsCount} / {CurrentPage.Histories.Count} {XIResources.Selected}");
            else SemanticScreenReader.Announce($"{XIResources.Page} {CurrentPageNumber} / {HistoryPages.Count}");
        }
    }
}
