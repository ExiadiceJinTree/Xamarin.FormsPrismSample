using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.FormsPrismSamp01.Objects;

// Device毎に処理切り替え実装
[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.FormsPrismSamp01.iOS.Device))]
namespace Xamarin.FormsPrismSamp01.iOS
{
    public sealed class Device : IDevice
    {
        public string GetDeviceName()
        {
            return "iOS";
        }
    }
}