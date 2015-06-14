using Livet;
using Livet.Commands;
using Livet.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ui_roleList.SelectedItem = null;
        }
    }
}