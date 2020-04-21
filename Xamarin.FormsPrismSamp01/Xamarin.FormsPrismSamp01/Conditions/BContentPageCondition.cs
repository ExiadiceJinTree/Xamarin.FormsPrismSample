using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.FormsPrismSamp01.Conditions
{
    internal sealed class BContentPageCondition
    {
        public string Title { get; set; }

        public BContentPageCondition(string title)
        {
            this.Title = title;
        }
    }
}
