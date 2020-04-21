using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.FormsPrismSamp01.Objects
{
    // Device毎に処理切り替え実装
    public interface IDevice
    {
        string GetDeviceName();
    }
}
