using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientTest.ViewModels;
using ClientTest.Models;

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
            var vm = new MainWindowViewModel();
            vm.DisplayUserName = "misono";
            vm.DisplayPassword = "password";
            vm.Login();

            privateVM.SetFieldOrProperty("DisplayUserName", "misono");
            privateVM.SetFieldOrProperty("DisplayPassword", "password");
            privateVM.Invoke("Login");
            internalServer = privateVM.GetFieldOrProperty("_apiServer") as ApiServer;
            Assert.IsTrue(internalServer.CurrentSession.IsLoggedIn);
        }
    }
}
