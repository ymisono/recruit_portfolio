using ClientTest.Models;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UserManageUtility.Models;
using UserManageUtility.ViewModels;

namespace UserManageUtility.Views
{
    /* 
     * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// AccountRegisterView.xaml の相互作用ロジック
    /// </summary>
    public partial class UserManageView : Window
    {
        public UserManageView()
        {
            InitializeComponent();
        }

        private void UnSelectUser_Click(object sender, RoutedEventArgs e)
        {
            ui_userList.SelectedItem = null;

            var vm = this.DataContext as UserManageViewModel;
            vm.ClearUserInput();
        }

        private void UnselectRole_Click(object sender, RoutedEventArgs e)
        {
            ui_roleList.SelectedItem = null;

            var vm = this.DataContext as UserManageViewModel;
            vm.ClearRoleInput();
        }

        //VMでどうしても取れない
        private void ListViewItem_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                var selectedUser = ui_userList.SelectedItem as SelectableUserInfo;

                //VMにアクセス
                var vm = this.DataContext as UserManageViewModel;
                vm.UserId = selectedUser.Id;
                vm.UserName = selectedUser.UserName;
                vm.EmailAddress = selectedUser.Email;
                vm.PhoneNumber = selectedUser.PhoneNumber;
                vm.LastName = selectedUser.LastName;
                vm.FirstName = selectedUser.FirstName;
                vm.LastNameKana = selectedUser.LastNameKana;
                vm.FirstNameKana = selectedUser.FirstNameKana;

                ui_IsDeletedCheckBox.IsChecked = selectedUser.IsDeleted;


                #region RoleList
                //該当するロールを選択
                var selectedRolesOfUser = selectedUser.Roles;
                //一度も選択されてないなら、RoleListから選択をはずす
                if (selectedRolesOfUser.Count == 0)
                {
                    ui_roleList.SelectedItem = null;
                    return;
                }
                //選択されたロールをRoleListに反映
                ui_roleList.SelectedItems.Clear();
                foreach(var r1 in selectedRolesOfUser)
                {
                    var allRoles = ui_roleList.Items;
                    foreach(Role r2 in allRoles)
                    {
                        if (r1.Id == r2.Id)
                        {
                            ui_roleList.SelectedItems.Add(r2);
                            break;
                        }
                    }
                }
                #endregion RoleList
            }
        }

        private void RoleListItem_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as UserManageViewModel;

            //複数選択されてた場合、ボックスを無効化する
            if (ui_roleList.SelectedItems.Count > 1)
            {
                vm.RoleName = "";
                vm.RoleDescription = "";
                return;
            }

            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                var selectedRole = ui_roleList.SelectedItem as Role;

                vm.RoleName = selectedRole.Name;
                vm.RoleDescription = selectedRole.Description;
            }
        }

        #region サイドバー
        private void ui_userManagementButton_Click(object sender, RoutedEventArgs e)
        {
            ui_userManagementContent.Visibility = Visibility.Visible;

        }

        private void ui_agencyManagementButton_Click(object sender, RoutedEventArgs e)
        {
            ui_userManagementContent.Visibility = Visibility.Collapsed;
        }

        #endregion サイドバー
    }
}