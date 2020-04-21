using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.FormsPrismSamp01.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //====================================================================================================
        // [非MVVM手法] ViewのCodeBehind側でロジックを実装する:
        // この方法だとテストプロジェクトからテスト実施が難しい。そのため、画面(View)とロジック実装部を分ける考えがMVVM。
        //====================================================================================================
        private void AButton_Clicked(object sender, EventArgs e)
        {
            this.ALabel.Text = "AButton was clicked ! - 非MVVM手法";
        }
    }
}