using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace ClientTest.Models
{
    public class LoginModel : NotificationObject
    {
        #region Username変更通知プロパティ
        private string _Username;

        public string Username
        {
            get
            { return _Username; }
            set
            { 
                if (_Username == value)
                    return;
                _Username = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Password変更通知プロパティ
        private string _Password;

        public string Password
        {
            get
            { return _Password; }
            set
            { 
                if (_Password == value)
                    return;
                _Password = value;
                RaisePropertyChanged();
            }
        }
        #endregion


    }
}
