using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.FormsPrismSamp01.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.FormsPrismSamp01.ViewModels.Tests
{
    //====================================================================================================
    // 非MVVM手法だと、テストプロジェクトからView,ViewのCodeBehindに対するテスト実施が難しい。
    // MVVM手法だと、テストプロジェクトからViewModelに対してテスト実施でき、Viewと分離したテストができる。
    //====================================================================================================
    [TestClass()]
    public class MainPageViewModelTests
    {
        [TestMethod()]
        public void MainPageViewModelTest_初期表示設定()
        {
            var vm = new MainPageViewModel(null, null, null);
            Assert.AreEqual("Main Page", vm.Title);
            Assert.AreEqual("BBB", vm.BLabel);
        }

        [TestMethod()]
        public void BButtonCommandTest_表示値変更()
        {
            var vm = new MainPageViewModel(null, null, null);
            vm.BButtonClickCmd.Execute();
            Assert.AreEqual("BButton was clicked ! - MVVM手法", vm.BLabel);
        }
    }
}