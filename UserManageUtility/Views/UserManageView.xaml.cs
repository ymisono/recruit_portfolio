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

        private void Button_Click(object sender, RoutedEventArgs e)
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
                vm.UserName = selectedUser.UserName;
                vm.EmailAddress = selectedUser.Email;
                vm.PhoneNumber = selectedUser.PhoneNumber;

                ui_IsDeletedCheckBox.IsChecked = selectedUser.IsDeleted;

                //該当するロールを選択
                var selectedRolesOfUser = selectedUser.Roles;
                //一度も選択されてないなら、選択をはずす
                if (selectedRolesOfUser.Count == 0)
                {
                    ui_roleList.SelectedItem = null;
                    return;
                }

                foreach(var r1 in selectedRolesOfUser)
                {
                    var allRoles = ui_roleList.Items;//.SingleOrDefault(x => x.Id == r.Id);
                    foreach(Role r2 in allRoles)
                    {
                        if (r1.Id == r2.Id)
                        {
                            ui_roleList.SelectedItem = r2;
                        }
                    }
                }
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
    }
}