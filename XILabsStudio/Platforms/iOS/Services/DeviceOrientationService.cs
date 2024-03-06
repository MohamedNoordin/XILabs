using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XILabsStudio.Services
{
    public class DeviceOrientationService
    {
        public static DeviceOrientation GetOrientation()
        {
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
            bool isPortrait = orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown;
            return isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
        }

        public static void SetOrientation(DeviceOrientation orientation)
        {
            switch (orientation)
            {
                case DeviceOrientation.Landscape:
                    UIApplication.SharedApplication.SetStatusBarOrientation(UIInterfaceOrientation.LandscapeLeft, true);
                    break;
                case DeviceOrientation.Undefined:
                case DeviceOrientation.Portrait:
                default:
                    UIApplication.SharedApplication.SetStatusBarOrientation(UIInterfaceOrientation.Portrait, true); break;
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
