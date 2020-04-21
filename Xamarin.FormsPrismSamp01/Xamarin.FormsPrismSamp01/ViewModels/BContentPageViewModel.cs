using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.FormsPrismSamp01.Conditions;

namespace Xamarin.FormsPrismSamp01.ViewModels
{
    // Page新規作成時は継承元クラスがBindableBaseになっているが、継承元クラスをViewModelBaseに変更した方がいい。
    // MainPageViewModelもそうなっているし、ViewModelBase自体がBindableBaseを継承しているので機能的に上位互換のため問題なし。
    //public class BContentPageViewModel : BindableBase
    public class BContentPageViewModel : ViewModelBase
    {
        // コンストラクタを継承元クラスのViewModelBaseのコンストラクタに合わせて引数付きに変更する。
        //public BContentPageViewModel()
        //{

        //}
        public BContentPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        // 画面遷移時の遷移先画面の初期化処理実装部。一度だけ呼ばれる。画面遷移時のパラメータを受け取って処理できる。
        // * Prism7.2より前ではOnNavigatingToメソッドだったが7.2以降はInitializeになった。
        public override void Initialize(INavigationParameters parameters)
        {
            // パラメータkeyが文字列リテラルなのがよくない
            //this.Title = parameters["title"].ToString();

            // パラメータkeyが文字列でない、かつ条件用クラスオブジェクトを受け取る方法。
            BContentPageCondition bContentPageCond = parameters[nameof(BContentPageCondition)] as BContentPageCondition;
            if (bContentPageCond == null)
            {
                throw new ArgumentException(nameof(BContentPageCondition));
            }
            this.Title = bContentPageCond.Title;
        }
    }
}
