using System;
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
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace ClientTest.ViewModels
{
    public class LoginViewModel : ViewModel, IDataErrorInfo
    {
        #region フィールド
        private Session _session;

        #endregion

        #region プロパティ
        private String _username;
        public String UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged("UserName");
            }
        }

        private String _password;
        public String Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        private String _notifyMessage;
        public String NotifyMessage
        {
            get { return _notifyMessage; }
            set
            {
                _notifyMessage = value;
                RaisePropertyChanged();
            }
        }


        #region IsMenuHidden変更通知プロパティ
        private bool _IsMenuHidden;

        public bool IsMenuHidden
        {
            get
            { return _IsMenuHidden; }
            set
            { 
                if (_IsMenuHidden == value)
                    return;
                _IsMenuHidden = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region LoadMessage変更通知プロパティ
        private string _LoadMessage;

        public string LoadMessage
        {
            get
            { return _LoadMessage; }
            set
            { 
                if (_LoadMessage == value)
                    return;
                _LoadMessage = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsHelloAnimationReady変更通知プロパティ
        private bool _IsHelloAnimationReady = false;

        public bool IsHelloAnimationReady
        {
            get
            { return _IsHelloAnimationReady; }
            set
            {
                _IsHelloAnimationReady = value;
                RaisePropertyChanged();
            }
        }
        #endregion


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
            //ログイン処理
            try
            {
                //非同期でログイン
                var loginTask = _session.LoginAsync(UserName, Password);
                //メッセージを表示して待つ
                IsMenuHidden = true;

                //ここで待つ
                await loginTask;

                //成功したら閉じる
                if (_session.IsLoggedIn)
                {
                    IsHelloAnimationReady = true;

                    //ようこそメッセージを表示
                    LoadMessage = "ようこそ " + _username + " さん！";

                    // タイマーを作成する
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1.4);
                    timer.Tick += new EventHandler( (sender, e) => 
                    {
                        //タイマー内でウィンドウを閉じる
                        Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
                    });
                    // タイマーの実行開始
                    timer.Start();
                }
            }
            catch(ApplicationException ex)
            {
                IsMenuHidden = false;

                NotifyMessage = ex.Message;
                //MessageBox.Show(ex.Message);
            }
            
        }
        #endregion

        public LoginViewModel(Session session)
        {
            _session = session;
        }

        public void Initialize()
        {
            IsMenuHidden = false;
            LoadMessage = "ログインしています……";

        }

#region IDataErrorInfo
        private Dictionary<String, String> _errors = new Dictionary<string, string>
        {
            {"UserName", null}, {"Password", null}
        };

        public string Error
        {
            get
            {
                var list = new List<String>();

                if (!String.IsNullOrEmpty(this["UserName"]))
                {
                    list.Add("ユーザー名");
                }
                if (!String.IsNullOrEmpty(this["Password"]))
                {
                    list.Add("パスワード");
                }

                return String.Join("・", list) + "が未入力です。";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                {
                    return _errors[columnName];
                }
                else
                {
                    return null;
                }
            }
        }
#endregion
    }
}
