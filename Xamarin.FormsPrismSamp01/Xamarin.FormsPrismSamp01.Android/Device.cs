using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.FormsPrismSamp01.Objects;

// Device毎に処理切り替え実装
[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.FormsPrismSamp01.Droid.Device))]
namespace Xamarin.FormsPrismSamp01.Droid
{
    internal sealed class Device : IDevice
    {
        public string GetDeviceName()
        {
            return "Android";
        }
    }
}