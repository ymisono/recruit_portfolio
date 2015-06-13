using ClientTest.Models;
using ClientTest.Utility;
using Livet;
using Livet.Commands;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace ClientTest.ViewModels
{
    public class LoginViewModel : ViewModel, IDataErrorInfo
    {
        #region フィールド
        private Session _session;

        private int _loginLoadDotCount = 0;

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


        #region HelloMessage変更通知プロパティ
        private string _HelloMessage;

        public string HelloMessage
        {
            get
            { return _HelloMessage; }
            set
            { 
                if (_HelloMessage == value)
                    return;
                _HelloMessage = value;
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
            //エラーがなければ、実行可能
            return _errors.Count > 0;
        }

        public async void Login()
        {
            //ロード画面のタイマー
            var loginLoadTimer = new DispatcherTimer();

            //ログイン処理
            try
            {
                //非同期でログイン
                var loginTask = _session.LoginAsync(UserName, Password);
                //メッセージを表示して待つ
                IsMenuHidden = true;
                LoadMessage = "ログインしています";

                //アニメーションする
                loginLoadTimer.Interval = TimeSpan.FromSeconds(0.3);
                loginLoadTimer.Tick += new EventHandler((s, e) =>
                {
                    //文字列を更新
                    if (_loginLoadDotCount > 2) _loginLoadDotCount = 0;
                    else _loginLoadDotCount++;

                    if (!_session.IsLoggedIn)
                    {
                        LoadMessage = "ログインしています" + new String('.', _loginLoadDotCount);
                    }
                    else LoadMessage = "";
                });
                loginLoadTimer.Start();

                //ここで待つ
                await loginTask;

                //成功したら閉じる
                if (_session.IsLoggedIn)
                {
                    //ようこそのアニメーションの準備完了
                    IsHelloAnimationReady = true;

                    //ロードのタイマーを止める
                    loginLoadTimer.Stop();
                    //ロードの文字列を消す
                    LoadMessage = "";
                    
                    //ようこそメッセージを表示
                    HelloMessage = "ようこそ " + _username + " さん！";

                    //ユーザー名を憶えておく
                    LocalSettings.AddUpdateAppSettings("RememberUserName", UserName);

                    // タイマーを作成する
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1.4);
                    timer.Tick += new EventHandler( (sender, e) => 
                    {
                        loginLoadTimer.Stop();
                        timer.Stop();
                        //タイマー内でウィンドウを閉じる
                        Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
                    });
                    // タイマーの実行開始
                    timer.Start();
                }
            }
            catch(ApplicationException ex)
            {
                //隠してたメニューを出す
                IsMenuHidden = false;

                //ロードのタイマーを止める
                loginLoadTimer.Stop();
                //ロードの文字列を消す
                LoadMessage = "";

                //詳細が存在してれば、それを使う
                if(ex.Data.Contains("details"))
                {
                    NotifyMessage = ex.Data["details"] as String;
                }
                else
                {
                NotifyMessage = ex.Message;
            }
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

            //前に使用したユーザー名があるなら、それを使う
            try
            {
                UserName = LocalSettings.ReadSetting("RememberUserName");
            }
            catch (ApplicationException)
            {
                UserName = "";
            }
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
