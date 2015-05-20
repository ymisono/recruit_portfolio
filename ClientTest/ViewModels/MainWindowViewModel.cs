﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ClientTest.Models;
using System.Security;
using System.Windows;
using System.Net.Http;

namespace ClientTest.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        private Authorizer _authorizer;
        private TokenReceiver _token;


        #region DisplayUserName変更通知プロパティ
        private string _DisplayUserName;

        public string DisplayUserName
        {
            get
            { return _DisplayUserName; }
            set
            { 
                if (_DisplayUserName == value)
                    return;
                _DisplayUserName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region DisplayPassword変更通知プロパティ
        private String _DisplayPassword;

        public String DisplayPassword
        {
            get
            { return _DisplayPassword; }
            set
            { 
                if (_DisplayPassword == value)
                    return;
                _DisplayPassword = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MyName変更通知プロパティ
        private string _MyName;

        public string MyName
        {
            get
            { return _MyName; }
            set
            { 
                if (_MyName == value)
                    return;
                _MyName = value;
                RaisePropertyChanged();
            }
        }
        #endregion



        #region RegisterCommand
        private ViewModelCommand _RegisterCommand;

        public ViewModelCommand RegisterCommand
        {
            get
            {
                if (_RegisterCommand == null)
                {
                    _RegisterCommand = new ViewModelCommand(Register, CanRegister);
                }
                return _RegisterCommand;
            }
        }

        public bool CanRegister()
        {
            return true;
        }

        public void Register()
        {
            if ( !String.IsNullOrWhiteSpace(DisplayUserName)
                && !String.IsNullOrWhiteSpace(DisplayPassword) )
            {
                _authorizer.UserName = DisplayUserName;
                _authorizer.Password = DisplayPassword.ToString();
                _authorizer.ConfirmPassword = DisplayPassword.ToString();
                _authorizer.Register();
            }
        }
        #endregion


        #region LoginCommand
        private ViewModelCommand _LoginCommand;

        public ViewModelCommand LoginCommand
        {
            get
            {
                if (_LoginCommand == null)
                {
                    _LoginCommand = new ViewModelCommand(Login, CanLogin);
                }
                return _LoginCommand;
            }
        }

        public bool CanLogin()
        {
            return true;
        }

        public async void Login()
        {
            var userinfo = new UserInfo();
            try
            {
                await _authorizer.Login(DisplayUserName, DisplayPassword);
                await userinfo.Fetch();
            }
            catch(HttpRequestException e)
            {
                MessageBox.Show("ログインできません。","ログイン",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
            if( !String.IsNullOrEmpty(userinfo.UserName ))
                MyName = String.Format("ログイン中：{0}",userinfo.UserName);
            DisplayUserName = "";
            DisplayPassword = "";
        }
        #endregion



        public void Initialize()
        {
            _authorizer = new Authorizer();
        }
    }
}
