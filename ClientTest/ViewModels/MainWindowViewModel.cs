using ClientTest.Models;
using ClientTest.Utility;
using ClientTest.Views;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Windows;

namespace ClientTest.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        private ApiServer _apiServer = new ApiServer();
        private Memo _memo = new Memo();


        #region DisplayUserName変更通知プロパティ
        private string _DisplayUserName;

        public string DisplayUserName
        {
            get
            { return _DisplayUserName; }
            set
            { 
                if (_DisplayUserName == value)
                    return;
                _DisplayUserName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region DisplayPassword変更通知プロパティ
        private String _DisplayPassword;

        public String DisplayPassword
        {
            get
            { return _DisplayPassword; }
            set
            { 
                if (_DisplayPassword == value)
                    return;
                _DisplayPassword = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MyName変更通知プロパティ
        private string _MyName;

        public string MyName
        {
            get
            { return _MyName; }
            set
            { 
                if (_MyName == value)
                    return;
                _MyName = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region memoText変更通知プロパティ
        private string _memoText;

        public string memoText
        {
            get
            { return _memoText; }
            set
            { 
                if (_memoText == value)
                    return;
                _memoText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region RegisterCommand
        private ViewModelCommand _RegisterCommand;

        public ViewModelCommand RegisterCommand
        {
            get
            {
                if (_RegisterCommand == null)
                {
                    _RegisterCommand = new ViewModelCommand(Register, CanRegister);
                }
                return _RegisterCommand;
            }
        }

        public bool CanRegister()
        {
            return true;
        }

        //イベントハンドラのみasyncでvoidで返す
        public async void Register()
        {
            if ( String.IsNullOrWhiteSpace(DisplayUserName)
                 || String.IsNullOrWhiteSpace(DisplayPassword) )
            {
                MessageBox.Show("空欄があります。","未入力", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                await _apiServer.RegisterAsync(DisplayUserName,DisplayPassword);
                MessageBox.Show(String.Format("登録しました！\nようこそ{0}さん", DisplayUserName));

                //成功したらログインも
                Login();
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show(String.Format("登録に失敗しました。\n{0}", ex.Message),
                   "登録失敗", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
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
            //ログイン
            try
            {
                await _apiServer.CurrentSession.LoginAsync(DisplayUserName, DisplayPassword);
                
                DisplayUserName = "";
                DisplayPassword = "";

                if (!String.IsNullOrEmpty(_apiServer.CurrentSession.UserInfo.UserName))
                    MyName = String.Format("ログイン中：{0}", _apiServer.CurrentSession.UserInfo.UserName);

                //前回のログイン名を覚えておく
                LocalSettings.AddUpdateAppSettings("RememberUserName", _apiServer.CurrentSession.UserInfo.UserName);
            }
            catch(ApplicationException e)
            {
                Messenger.Raise(new InformationMessage(
                    String.Format("ログインできません。\n{0}",e.Message),
                    "ログイン失敗", MessageBoxImage.Exclamation,"Error")
                    );
                return;
            }

            //メモのロード
            try
            {
                var json = await _apiServer.ReadAsync<Memo>("Memos?ownerid=" + _apiServer.CurrentSession.UserInfo.Id);
                if (json!=null)
                {
                     memoText = json.Content;
                     _memo = json;
                }
                else //何も入ってない場合
                {
                    memoText = "";
                    _memo.Id = -1;
                }
            }

            catch (ApplicationException e)
            {
                MessageBox.Show( 
                    String.Format("メモ帳をロードできませんでした。\n{0}",e.Message),
                    "ロード失敗", MessageBoxButton.OK, MessageBoxImage.Exclamation
                );
            }
        }
        #endregion

        #region SaveCommand
        private ViewModelCommand _SaveCommand;

        public ViewModelCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new ViewModelCommand(Save);
                }
                return _SaveCommand;
            }
        }

        public async void Save()
        {
            try
            {
                if (_memo == null) return;

                _memo.Content = memoText;
                _memo.OwnerId = _apiServer.CurrentSession.UserInfo.Id;

                //まだ作られてない場合、作製
                if (_memo.IsFirstTime)
                {
                    await _apiServer.CreateAsync<Memo>(_memo, "Memos");
                }
                else
                {
                    await _apiServer.UpdateAsync<Memo>(_memo, String.Format("Memos/{0}", _memo.Id));
                }

                MessageBox.Show("保存しました。");
            }
            catch (ApplicationException e)
            {
                MessageBox.Show(
                    String.Format("メモ帳を保存できませんでした。\n{0}", e.Message),
                    "保存失敗", MessageBoxButton.OK, MessageBoxImage.Exclamation
                );
            }
        }
        #endregion


        #region LogoutCommand
        private ViewModelCommand _LogoutCommand;

        public ViewModelCommand LogoutCommand
        {
            get
            {
                if (_LogoutCommand == null)
                {
                    _LogoutCommand = new ViewModelCommand(Logout, CanLogout);
                }
                return _LogoutCommand;
            }
        }

        public bool CanLogout()
        {
            return true;
        }

        public async void Logout()
        {
            try
            {
                await _apiServer.CurrentSession.LogoutAsync();

                _memo = null;
                memoText = "";
                MyName = "";
            }
            catch(ApplicationException)
            {

            }
        }
        #endregion

       

        public void Initialize()
        {
            try
            {
                DisplayUserName = LocalSettings.ReadSetting("RememberUserName");
            }
            catch(ApplicationException)
            {
                DisplayUserName = "";
            }

            using (var vm = new LoginViewModel(_apiServer.CurrentSession))
            {
                Messenger.Raise(new TransitionMessage(vm, "Transition"));
            }
        }
    }
}
