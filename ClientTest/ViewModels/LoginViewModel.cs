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

namespace ClientTest.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        #region フィールド
        private LoginModel _loginModel;


        #endregion

        #region プロパティ
        public String Username { get; set; }

        public String Password { get; set; }

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

        public void Login()
        {
            Messenger.Raise(new WindowActionMessage(WindowAction.Close,"Close"));
        }
        #endregion

        public LoginViewModel() { }

        public LoginViewModel(LoginModel loginModel)
        {
            _loginModel = loginModel;
        }

        public void Initialize()
        {
        }

    }
}
