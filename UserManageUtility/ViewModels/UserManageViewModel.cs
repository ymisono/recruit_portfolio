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

using UserManageUtility.Models;
using System.Text.RegularExpressions;

namespace UserManageUtility.ViewModels
{
    public class UserManageViewModel : ViewModel, IDataErrorInfo
    {
        #region フィールド


        private static readonly Regex AlphaNumUnderscoreRegex = new Regex(@"^[a-zA-Z0-9_]*$", RegexOptions.Compiled);

        #endregion フィールド

        #region プロパティ

        #region UserName変更通知プロパティ
        private string _UserName;

        public string UserName
        {
            get
            { return _UserName; }
            set
            { 
                if (_UserName == value)
                    return;
                _UserName = value;

                if (value == null || String.IsNullOrEmpty(value.Trim()))
                {
                    _errors["UserName"] = "ユーザー名は必須です";
                    RaisePropertyChanged("Error");
                }
                else if (!AlphaNumUnderscoreRegex.IsMatch(value.ToString()))
                {
                    _errors["UserName"] = "半角英数字とアンダースコア(_)のみ使用可能です";
                    RaisePropertyChanged("Error");
                }
                else
                {
                    _errors["UserName"] = null;
                    RaisePropertyChanged();
                }

            }
        }
        #endregion UserName


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

                if (value == null || String.IsNullOrEmpty(value.Trim()))
                {
                    _errors["Password"] = "パスワードは必須です";
                    RaisePropertyChanged("Error");
                }
                else if(value.Contains(" "))
                {
                    _errors["Password"] = "スペースを含んではいけません";
                    RaisePropertyChanged("Error");
                }
                else
                {
                    _errors["Password"] = null;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion Password


        #region PasswordConfirm変更通知プロパティ
        private string _PasswordConfirm;

        public string PasswordConfirm
        {
            get
            { return _PasswordConfirm; }
            set
            { 
                if (_PasswordConfirm == value)
                    return;
                _PasswordConfirm = value;

                if (value == null || String.IsNullOrEmpty(value.Trim()))
                {
                    _errors["PasswordConfirm"] = "確認用パスワードを入力してください";
                    RaisePropertyChanged("Error");
                }
                else if (value.Contains(" "))
                {
                    _errors["PasswordConfirm"] = "スペースを含んではいけません";
                    RaisePropertyChanged("Error");
                }
                else
                {
                    _errors["PasswordConfirm"] = null;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion


        #region EmailAddress変更通知プロパティ
        private string _EmailAddress;

        public string EmailAddress
        {
            get
            { return _EmailAddress; }
            set
            { 
                if (_EmailAddress == value)
                    return;
                _EmailAddress = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #endregion プロパティ

        public void Initialize()
        {
        }

        #region IDataErrorInfo
        private Dictionary<String, String> _errors = new Dictionary<string, string>
        {
            {"UserName", null}, {"Password", null}, {"PasswordConfirm", null}
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
