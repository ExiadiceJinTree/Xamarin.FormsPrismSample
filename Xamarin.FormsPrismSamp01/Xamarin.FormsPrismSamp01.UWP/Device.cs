using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.FormsPrismSamp01.Objects;

// Device毎に処理切り替え実装
[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.FormsPrismSamp01.UWP.Device))]
namespace Xamarin.FormsPrismSamp01.UWP
{
    public sealed class Device : IDevice
    {
        public string GetDeviceName()
        {
            return "UWP";
        }
    }
}
