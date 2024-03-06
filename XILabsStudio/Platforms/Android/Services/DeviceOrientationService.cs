using Android.Content;
using Android.Runtime;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XILabsStudio.Services
{
    public class DeviceOrientationService
    {
        private static MauiAppCompatActivity _activity;
        public static void Initialize(object activity)
        {
            _activity = (global::Microsoft.Maui.MauiAppCompatActivity)activity;
        }

        public static DeviceOrientation GetOrientation()
        {
            IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            SurfaceOrientation orientation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = orientation == SurfaceOrientation.Rotation90 || orientation == SurfaceOrientation.Rotation270;
            return isLandscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
        }

        public static void SetOrientation(DeviceOrientation orientation)
        {
            switch (orientation)
            {
                case DeviceOrientation.Landscape:
                    _activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
                    break;
                case DeviceOrientation.Undefined:
                case DeviceOrientation.Portrait:
                default:
                    _activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
                    break;
            }
        }
    }

    public enum DeviceOrientation
    {
        Undefined,
        Portrait,
        Landscape,
    }
}
