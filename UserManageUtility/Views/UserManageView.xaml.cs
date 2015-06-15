using ClientTest.Models;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using System;
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
            }
        }

        private void RoleListItem_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                var selectedRole = ui_roleList.SelectedItem as Role;

                var vm = this.DataContext as UserManageViewModel;
                vm.RoleName = selectedRole.Name;
                vm.RoleDescription = selectedRole.Description;
            }
        }
    }
}