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
                await _session.LoginAsync(UserName, Password);
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }

            Messenger.Raise(new WindowActionMessage(WindowAction.Close,"Close"));
        }
        #endregion

        public LoginViewModel(Session session)
        {
            _session = session;
        }

        public void Initialize()
        {
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
