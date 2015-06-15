using ClientTest.Models;
using ClientTest.Utility;
using Livet;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using UserManageUtility.Models;

namespace UserManageUtility.ViewModels
{
    public class UserManageViewModel : ViewModel, IDataErrorInfo
    {
        #region フィールド

        private ApiServer _apiServer = new ApiServer();

        private static readonly Regex AlphaNumUnderscoreRegex = new Regex(@"^[a-zA-Z0-9_]*$", RegexOptions.Compiled);

        #endregion フィールド

        #region プロパティ

        #region ユーザー関係

        #region Users変更通知プロパティ
        private ObservableCollection<SelectableUserInfo> _Users;
        /// <summary>
        /// 全ユーザー
        /// </summary>
        public ObservableCollection<SelectableUserInfo> Users
        {
            get { return _Users; }
            private set 
            {
                if (ReferenceEquals(null,value)) return;

                //最初だった場合
                if (_Users == null)
                {
                    _Users = value;
                }
                else
                {
                    var newUsers = value.Where(x => !_Users.Any(y => x.Id == y.Id));
                    foreach (var user in newUsers)
                    {
                        _Users.Add(user);
                    }
                }
                RaisePropertyChanged("Users");
            }
        }
        #endregion

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

        #region PhoneNumber変更通知プロパティ
        private string _PhoneNumber;

        public string PhoneNumber
        {
            get
            { return _PhoneNumber; }
            set
            { 
                if (_PhoneNumber == value)
                    return;
                _PhoneNumber = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsDeleted変更通知プロパティ
        private bool _IsDeleted;

        public bool IsDeleted
        {
            get
            { return _IsDeleted; }
            set
            { 
                if (_IsDeleted == value)
                    return;
                _IsDeleted = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #endregion ユーザー関係

        #region ロール関係

        #region Roles変更通知プロパティ
        private ObservableCollection<Role> _Roles;

        public ObservableCollection<Role> Roles
        {
            get { return _Roles; }
            private set
            {
                if (ReferenceEquals(null, value)) return;

                //最初だった場合
                if (_Roles == null)
                {
                    _Roles = value;
                }
                else
                {
                    var newRoles = value.Where(x => !_Roles.Any(y => x.Id == y.Id));
                    foreach (var role in newRoles)
                    {
                        _Roles.Add(role);
                    }
                }
                RaisePropertyChanged("Roles");
            }
        }
        #endregion


        #region RoleName変更通知プロパティ
        private string _RoleName;

        public string RoleName
        {
            get
            { return _RoleName; }
            set
            { 
                if (_RoleName == value)
                    return;
                _RoleName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region RoleDescription変更通知プロパティ
        private string _RoleDescription;

        public string RoleDescription
        {
            get
            { return _RoleDescription; }
            set
            { 
                if (_RoleDescription == value)
                    return;
                _RoleDescription = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion ロール関係

        #region Notification変更通知プロパティ
        private string _notification;

        public string Notification
        {
            get
            { return _notification; }
            set
            { 
                if (_notification == value)
                    return;
                _notification = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #endregion プロパティ

        #region コマンド

        #region ClickUserListCommand
        private Livet.Commands.ViewModelCommand _ClickUserListCommand;

        public Livet.Commands.ViewModelCommand ClickUserListCommand
        {
            get
            {
                if (_ClickUserListCommand == null)
                {
                    _ClickUserListCommand = new Livet.Commands.ViewModelCommand(ClickUserList);
                }
                return _ClickUserListCommand;
            }
        }

        public void ClickUserList()
        {

        }
        #endregion


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
            Notification = "";

            if (!_apiServer.CurrentSession.IsLoggedIn)
            {
                Notification = "ログインしてません";
                return;
            }

            //必須項目に一つでも空欄があれば、帰る
            if (String.IsNullOrEmpty(UserName) ||
                String.IsNullOrEmpty(Password) ||
                String.IsNullOrEmpty(PasswordConfirm)
                )
            {
                Notification = "必須入力に空欄があります";
                return;
            }

            //エラーが一つでもあれば帰る
            foreach (var err in _errors)
            {
                if (err.Value != null)
                {
                    Notification = "入力項目にエラーがあります";
                    return;
                }
            }

            try
            {
                var registerTask = _apiServer.RegisterAsync(UserName, Password, EmailAddress,PhoneNumber);

                Notification = "書き込み中……";

                await registerTask;

                //ついでに更新
                await Update();

                Notification = "登録しました";
            }
            catch(ApplicationException ex)
            {
                Notification = ex.Message;
            }
        }
        #endregion

        #region DeleteCommand
        private Livet.Commands.ListenerCommand<ConfirmationMessage> _DeleteCommand;

        public Livet.Commands.ListenerCommand<ConfirmationMessage> DeleteCommand
        {
            get
            {
                if (_DeleteCommand == null)
                {
                    _DeleteCommand = new Livet.Commands.ListenerCommand<ConfirmationMessage>(Delete);
                }
                return _DeleteCommand;
            }
        }

        public async void Delete(ConfirmationMessage message)
        {
            var selectedUser = Users.SingleOrDefault(u => u.IsSelected == true);

            if (selectedUser == null)
            {
                Notification = "対象が選択されていません";
                return;
            }

            if (message.Response.HasValue && message.Response.Value)
            {
                try
                {
                    Notification = "削除中です";

                    await _apiServer.DeleteByIdAsync(selectedUser.Id, "Account");

                    //空にする
                    Users.Clear();

                    await Update();

                    Notification = "削除しました";
                }
                catch (ApplicationException ex)
                {
                    Notification = ex.Message;
                }
            }
            
        }
        #endregion

        #region AddNewRoleCommand
        private Livet.Commands.ViewModelCommand _AddNewRoleCommand;

        public Livet.Commands.ViewModelCommand AddNewRoleCommand
        {
            get
            {
                if (_AddNewRoleCommand == null)
                {
                    _AddNewRoleCommand = new Livet.Commands.ViewModelCommand(AddNewRole);
                }
                return _AddNewRoleCommand;
            }
        }

        public async void AddNewRole()
        {
            //対象
            if ( String.IsNullOrEmpty(RoleName) || String.IsNullOrEmpty(RoleDescription ) )
            {
                Notification = "対象が入力されていません";
                return;
            }

            try
            {
                Notification = "ロール追加中です";

                await _apiServer.CreateAsync<Role>(
                    new Role { Name = RoleName, Description=RoleDescription }, "Roles");

                await Update();

                Notification = "追加しました";
            }
            catch (ApplicationException ex)
            {
                Notification = ex.Message;
            }
        }
        #endregion

        #endregion コマンド

        public void Initialize()
        {
            //投げっぱなしだから、いつ終わるか分からないよーん
            LoginStartUpAsync();
        }


        //ユーザーの入力項目をクリア
        public void ClearUserInput()
        {
            _UserName = null;
            RaisePropertyChanged("UserName");
            Password = null;
            PasswordConfirm = null;
            EmailAddress = null;
            PhoneNumber = null;
            IsDeleted = false;
            RaisePropertyChanged();
        }

        public void ClearRoleInput()
        {
            RoleName = null;
            RoleDescription = null;
        }

#region ヘルパー（プライベートメソッド）

        //voidで投げっぱ
        private async void LoginStartUpAsync()
        {
            try
            {
                Notification = "初期化中……";

                var pass = LocalSettings.ReadSetting("AdminPass");

                await _apiServer.CurrentSession.LoginAsync("misono", pass);

                //ここで更新する
                await Update();

                Notification = "";
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
            }
        }

        private async Task Update()
        {
            Users = await _apiServer.ReadAsync<ObservableCollection<SelectableUserInfo>>("Account/UserInfo");

            Roles = await _apiServer.ReadAsync<ObservableCollection<Role>>("Roles");
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
