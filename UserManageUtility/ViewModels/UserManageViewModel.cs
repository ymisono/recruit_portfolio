using ClientTest.Models;
using ClientTest.Utility;
using Livet;
using Livet.Commands;
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

        #region UserId変更通知プロパティ
        private string _UserId;

        public string UserId
        {
            get
            { return _UserId; }
            set
            { 
                if (_UserId == value)
                    return;
                _UserId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

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
                if (ReferenceEquals(null, value))
                {
                    _Users = null;
                    return;
                }

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

        #region LastName変更通知プロパティ
        private string _LastName;

        public string LastName
        {
            get
            { return _LastName; }
            set
            { 
                if (_LastName == value)
                    return;
                _LastName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region FirstName変更通知プロパティ
        private string _FirstName;

        public string FirstName
        {
            get
            { return _FirstName; }
            set
            { 
                if (_FirstName == value)
                    return;
                _FirstName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LastNameKana変更通知プロパティ
        private string _LastNameKana;

        public string LastNameKana
        {
            get
            { return _LastNameKana; }
            set
            { 
                if (_LastNameKana == value)
                    return;
                _LastNameKana = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region FirstNameKana変更通知プロパティ
        private string _FirstNameKana;

        public string FirstNameKana
        {
            get
            { return _FirstNameKana; }
            set
            { 
                if (_FirstNameKana == value)
                    return;
                _FirstNameKana = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged("IsDeleted");
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
                if (ReferenceEquals(null, value))
                {
                    _Roles = null;
                    return;
                }

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
            if ( 
                //新規の場合
                (
                    UserId == null &&
                    (
                        String.IsNullOrEmpty(UserName)|| 
                        String.IsNullOrEmpty(Password) ||
                        String.IsNullOrEmpty(PasswordConfirm)
                    )
                ) ||
                //変更の場合
                (UserId != null && String.IsNullOrEmpty(UserName))
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
                Task addOrUpdateTask;
                //新規（POST）
                if (UserId == null)
                {
                    //選択されているロール
                    var selectedRoles = Roles.Where(x => x.IsSelected == true);
                    addOrUpdateTask = _apiServer.RegisterAsync(
                        UserName, Password,
                        lastName:LastName,firstName:FirstName,
                        lastNameKana:LastNameKana,firstNameKana:FirstNameKana,
                        email: EmailAddress, phoneNumber: PhoneNumber,
                        roles: selectedRoles);
                }
                else
                {
                    //ここでは必ず選択されているはず
                    var selectedUser = Users.Single(u=>u.IsSelected==true);
                    addOrUpdateTask = _apiServer.UpdateAsync( 
                        new UserInfo
                        {
                            Id =UserId, UserName=UserName,
                            LastName = LastName, FirstName = FirstName,
                            LastNameKana = LastNameKana, FirstNameKana = FirstNameKana,
                            Email =EmailAddress,
                            PhoneNumber = PhoneNumber,
                            IsDeleted = IsDeleted,
                            Roles = selectedUser.Roles,
                        }, "Account/UserInfo");
                }

                Notification = "書き込み中……";

                await addOrUpdateTask;

                //ついでに更新
                Users.Clear();
                ClearUserInput();
                await Update();

                Notification = "完了しました";
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
                    ClearUserInput();
                    ClearRoleInput();
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
            if ( String.IsNullOrEmpty(RoleName) )
            {
                Notification = "ロール名が入力されていません";
                return;
            }

            var selectedRoles = Roles.Where(x=>x.IsSelected==true);
            if ( (selectedRoles.Count() > 1) == true)
            {
                Notification = "ロールの追加・削除は一つずつ行ってください";
                return;
            }

            try
            {
                //新規追加
                //入力ロールが既存ロールリストにないとき
                //ここでは必ず選択が1つのはず
                var selectedRole = selectedRoles.SingleOrDefault();
                //ロールがIdを持っていなければ新規
                if(selectedRole == null)
                {
                    Notification = "ロール追加中です";

                    await _apiServer.CreateAsync<Role>(
                        new Role { Name = RoleName, Description = RoleDescription }, "Roles");
                }
                else //あるなら、変更
                {
                    Notification = "ロール変更中です";

                    await _apiServer.UpdateByIdAsync<Role>(
                        new Role { Id = selectedRole.Id, Name = RoleName, Description = RoleDescription }, "Roles");

                    //変更の場合、一端クリアする
                    Users = null; //こっちも変わる
                    Roles = null;
                }

                await Update();

                Notification = "完了しました";
            }
            catch (ApplicationException ex)
            {
                Notification = ex.Message;
            }
        }
        #endregion


        #region DeleteRoleCommand
        private ListenerCommand<ConfirmationMessage> _DeleteRoleCommand;

        public ListenerCommand<ConfirmationMessage> DeleteRoleCommand
        {
            get
            {
                if (_DeleteRoleCommand == null)
                {
                    _DeleteRoleCommand = new ListenerCommand<ConfirmationMessage>(DeleteRole);
                }
                return _DeleteRoleCommand;
            }
        }

        public async void DeleteRole(ConfirmationMessage message)
        {
            if ( !message.Response.HasValue || !message.Response.Value)
            {
                return;
            }

            //待機するタスク
            var tasks = new List<Task>();

            var selectedRoles = Roles.Where(x => x.IsSelected == true);
            if(selectedRoles==null)
            {
                Notification = "ロールが選択されてません";
                return;
            }

            try
            {
                Notification = "削除中……";
                foreach (var role in selectedRoles)
                {
                    tasks.Add(_apiServer.DeleteByIdAsync(role.Id, "Roles"));
                }
                //ここで待つ
                while (tasks.Count > 0)
                {
                    var finishedTask = await Task.WhenAny(tasks);
                    tasks.Remove(finishedTask);
                }
                Notification = "削除完了しました";

                //リセット・リロード
                Roles = null;
                await Update();
            }
            catch(ApplicationException ex)
            {
                Notification = ex.Message;
            }
        }
        #endregion


        #region MapRolesToUserCommand
        private Livet.Commands.ViewModelCommand _MapRolesToUserCommand;

        public Livet.Commands.ViewModelCommand MapRolesToUserCommand
        {
            get
            {
                if (_MapRolesToUserCommand == null)
                {
                    _MapRolesToUserCommand = new Livet.Commands.ViewModelCommand(MapRolesToUser);
                }
                return _MapRolesToUserCommand;
            }
        }

        public async void MapRolesToUser()
        {
            var selectedUser = Users.SingleOrDefault(x=>x.IsSelected==true);
            var selectedRoles = Roles.Where(x => x.IsSelected == true);
            var unselectedRoles = Roles.Where(x => x.IsSelected == false);

            if (selectedUser == null)
            {
                Notification = "ユーザーが選択されていません";
                return;
            }

            //あるロールをユーザーが持っているか
            foreach (var role in selectedRoles)
            {
                var hit = selectedUser.Roles.SingleOrDefault(x => x.Id == role.Id);
                if (hit == null)
                {
                    selectedUser.Roles.Add(role);
                }
            }

            //選択されてないロールのリストからユーザーが持ってるものを取り除く
            foreach (var role in unselectedRoles)
            {
                var hit = selectedUser.Roles.SingleOrDefault(x => x.Id == role.Id);
                if (hit != null)
                {
                    var tempList = selectedUser.Roles.ToList();
                    tempList.RemoveAll(x => x.Id == hit.Id);
                    selectedUser.Roles = tempList;

                    //selectedUser.Roles = selectedUser.Roles.Where(x => !(x.Id == hit.Id)); //Collectionなのでうまくいかない
                }
            }

            try
            {
                var task = _apiServer.UpdateAsync(SelectableUserInfo.GetBase(selectedUser), "Account/UserInfo");

                Notification = "反映中です";

                await task;

                //ユーザーを一旦空にする
                Users = null;
                await Update();

                Notification = "反映しました";
            }
            catch(ApplicationException ex)
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
            UserId = null;
            _UserName = null;
            RaisePropertyChanged("UserName");
            UserId = null;
            _Password = null;
            RaisePropertyChanged("Password");
            _PasswordConfirm = null;
            RaisePropertyChanged("PasswordConfirm");
            EmailAddress = null;
            PhoneNumber = null;
            FirstName = null;
            FirstNameKana = null;
            LastName = null;
            LastNameKana = null;
            IsDeleted = false;
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

                await _apiServer.CurrentSession.LoginAsync("admin", pass);

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
            Task<ObservableCollection<SelectableUserInfo>> userReadTask;
            Task<ObservableCollection<Role>> roleReadTask;

            userReadTask = _apiServer.ReadAsync<ObservableCollection<SelectableUserInfo>>("Account/UserInfo");
            roleReadTask = _apiServer.ReadAsync<ObservableCollection<Role>>("Roles");

            Users = await userReadTask;
            Roles = await roleReadTask;
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
