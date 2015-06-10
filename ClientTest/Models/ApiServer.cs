using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ClientTest.Models
{
    /// <summary>
    /// APIサーバーへの接続を抽象化する。
    /// </summary>
    public class ApiServer
    {
        /// <summary>
        /// サーバーのセッション
        /// </summary>
        public Session CurrentSession { get; private set; }

        //サーバーAPIのパス。APIを呼び出す基点になる。
        private String _apiPath;

        #region コンストラクタ
        public ApiServer()
        {
            CurrentSession = new Session();

#if DEBUG
            _apiPath = String.Format("{0}api/", ConfigurationManager.AppSettings["LocalAPIServerPath"]);
#else
            _apiPath = String.Format("{0}api/", ConfigurationManager.AppSettings["RemoteAPIServerPath"]);
#endif //DEBUG
        }
        #endregion

        /// <summary>
        /// 汎用的な取得メソッド
        /// </summary>
        /// <returns>返り値はJSONで</returns>
        public async Task<String> Read(String restResource)
        {
            String result = null;

            //ログインしているか確認
            if (!CurrentSession.IsLoggedIn) return result;

            //接続開始
            using (var client = new HttpClient())
            {
                //認証の為に、トークンをヘッダーにセット。
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentSession.AccessToken);

                var res = await client.GetAsync(String.Format(
                        "{0}{1}", _apiPath, restResource)
                        );

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);

                //中身を展開
                var content = await res.Content.ReadAsStringAsync();

                //中身をチェック
                if (!String.IsNullOrEmpty(content) && content != "null")
                {
                    result = content;
                }

                return result;
            }
        }

        public async Task Create<T>(T newObj, String restResource)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentSession.AccessToken);

                var sendContent = JsonConvert.SerializeObject(newObj);

                //POST
                var res = await client.PostAsync(
                        new Uri(_apiPath + restResource),
                        new StringContent(sendContent, Encoding.UTF8, "application/json")
                    );

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);
            }
        }

        public async Task Update<T>(T newObj, String restResource)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentSession.AccessToken);

                var sendContent = JsonConvert.SerializeObject(newObj);

                //PUT
                var res = await client.PutAsync(
                        new Uri(_apiPath + restResource),
                        new StringContent(sendContent, Encoding.UTF8, "application/json")
                    );

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);
            }
        }

        public async Task Register(String userName, String password, String email = "")
        {
            var sendObj = new { UserName = userName, Email = email, Password = password };

            using (var client = new HttpClient())
            {
                var sendContent = JsonConvert.SerializeObject(sendObj);

                var res = await client.PostAsync(
                    new Uri( _apiPath + "Account/Register"),
                    new StringContent(sendContent, Encoding.UTF8, "application/json"));

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);
            }
        }
    }
}
