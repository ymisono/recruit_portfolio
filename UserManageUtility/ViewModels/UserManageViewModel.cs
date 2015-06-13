﻿using Livet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using ClientTest.Models;
using ClientTest.Utility;
using System.Threading.Tasks;

namespace UserManageUtility.ViewModels
{
    public class UserManageViewModel : ViewModel, IDataErrorInfo
    {
        #region フィールド

        private ApiServer _apiServer = new ApiServer();

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

                //確認用パスワードとダブってるか
                if(value == PasswordConfirm)
                {
                    _errors["PasswordConfirm"] = null;
                    RaisePropertyChanged("PasswordConfirm");
                }
                else if (!String.IsNullOrEmpty(PasswordConfirm))
                {
                    _errors["PasswordConfirm"] = "確認用パスワードが一致しません"; ;
                    RaisePropertyChanged("PasswordConfirm");
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
                else if (value != Password)
                {
                    _errors["PasswordConfirm"] = "確認用パスワードが一致しません";
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


        #region NotifyMessage変更通知プロパティ
        private string _NotifyMessage;

        public string NotifyMessage
        {
            get
            { return _NotifyMessage; }
            set
            { 
                if (_NotifyMessage == value)
                    return;
                _NotifyMessage = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #endregion プロパティ

        #region コマンド


        #region WriteToDBCommand
        private Livet.Commands.ViewModelCommand _WriteToDBCommand;

        public Livet.Commands.ViewModelCommand WriteToDBCommand
        {
            get
            {
                if (_WriteToDBCommand == null)
                {
                    _WriteToDBCommand = new Livet.Commands.ViewModelCommand(WriteToDB);
                }
                return _WriteToDBCommand;
            }
        }

        public async void WriteToDB()
        {
            NotifyMessage = "";

            if (!_apiServer.CurrentSession.IsLoggedIn)
            {
                NotifyMessage = "ログインしてません";
                return;
            }

            //一つでも空欄があれば、帰る
            if (String.IsNullOrEmpty(UserName) ||
                String.IsNullOrEmpty(Password) ||
                String.IsNullOrEmpty(PasswordConfirm)
                )
            {
                NotifyMessage = "必須入力に空欄があります";
                return;
            }

            //エラーが一つでもあれば帰る
            foreach (var err in _errors)
            {
                if (err.Value != null)
                {
                    NotifyMessage = "入力項目にエラーがあります";
                    return;
                }
            }

            try
            {
                await _apiServer.RegisterAsync(UserName, Password, EmailAddress);
            }
            catch(ApplicationException ex)
            {
                NotifyMessage = ex.Message;
            }
        }
        #endregion


          #endregion コマンド

        public void Initialize()
        {
            //投げっぱなしだから、いつ終わるか分からないよーん
            LoginStartUpAsync();
        }

#region ヘルパー（プライベートメソッド）

        //voidで投げっぱ
        private async void LoginStartUpAsync()
        {
            var pass = LocalSettings.ReadSetting("AdminPass");

            await _apiServer.CurrentSession.LoginAsync("misono", pass);
        }

#endregion

        #region IDataErrorInfo
        private Dictionary<String, String> _errors = new Dictionary<string, string>
        {
            {"UserName", null}, {"Password", null}, {"PasswordConfirm", null}
        };

        public string Error
        {
            get { throw new NotImplementedException(); }
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