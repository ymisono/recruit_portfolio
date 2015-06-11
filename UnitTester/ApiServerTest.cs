using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientTest.Models;
using System.Threading.Tasks;

namespace UnitTester
{
    [TestClass]
    public class ApiServerTest
    {
        [TestMethod]
        public async Task LoginTest()
        {
            ApiServer server = new ApiServer();
            await server.CurrentSession.LoginAsync("misono","password");

            //ログイン成功しているはず
            Assert.IsTrue(server.CurrentSession.IsLoggedIn);

            //パスワードが違う
            try
            {
                await server.CurrentSession.LoginAsync("misono", "まちがい");
                //ここに来ることはない
                Assert.Fail();
            }
            catch(ApplicationException)
            {
                //ここに来てほしい
            }

            //存在しないユーザー
            try
            {
                await server.CurrentSession.LoginAsync("誰ですかこれ？", "まちがい");
                Assert.Fail();
            }
            catch (ApplicationException){}
        }
    }
}
