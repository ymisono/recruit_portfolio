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
    public class LoginViewModel : ViewModel
    {
        #region フィールド
        private Session _session;

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

        public async void Login()
        {
            //ログイン処理
            try
            {
                await _session.LoginAsync(Username, Password);
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

    }
}
