using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientTest.ViewModels;
using ClientTest.Models;
using System.Threading;

namespace UnitTester
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        [TestMethod]
        public void LoginTest()
        {
            var privateVM = new PrivateObject(new MainWindowViewModel());

            //初期状態でログインしても入れない
            privateVM.Invoke("Login");
            var internalServer = privateVM.GetFieldOrProperty("_apiServer") as ApiServer;
            Assert.IsFalse(internalServer.CurrentSession.IsLoggedIn);

            //misonoでログイン
            privateVM.SetFieldOrProperty("DisplayUserName", "misono");
            privateVM.SetFieldOrProperty("DisplayPassword", "password");
            privateVM.Invoke("Login");
            //Taskがないので、待てない。(タイマーを使った方がいいか？)
            Thread.Sleep(1000);
            internalServer = privateVM.GetFieldOrProperty("_apiServer") as ApiServer;
            Assert.IsTrue(internalServer.CurrentSession.IsLoggedIn);
        }
    }
}
