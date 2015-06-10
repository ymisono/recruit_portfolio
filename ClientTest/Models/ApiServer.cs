using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.ModelBinding;

namespace ClientTest.Models
{
    /// <summary>
    /// APIサーバーへの接続を抽象化する。
    /// </summary>
    public class ApiServer : NotificationObject
    {
        /// <summary>
        /// サーバーのセッション
        /// </summary>
        public Session CurrentSession { get; private set; }

        //サーバーAPIのパス。APIを呼び出す基点になる。
        private String _apiPath;

        public ApiServer()
        {
            CurrentSession = new Session();
            _apiPath = String.Format("{0}api/", App.Current.Properties["APIServerPath"]);
        }

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

                await CurrentSession.CheckResponseStatus(res);

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

                await CurrentSession.CheckResponseStatus(res);
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

                await CurrentSession.CheckResponseStatus(res);
            }
        }
    }

    public class ErrorObject
    {
        public String Message;
        [JsonProperty(PropertyName = "ModelState")]
        public Dictionary<string, string[]> modelstate;

        //public static void ErrorHandle()
        //{
        //    // Sometimes, there may be Model Errors:
        //    if (deserializedErrorObject.ModelState != null)
        //    {
        //        var errors =
        //            deserializedErrorObject.ModelState
        //                                    .Select(kvp => string.Join(". ", kvp.Value));
        //        for (int i = 0; i < errors.Count(); i++)
        //        {
        //            // Wrap the errors up into the base Exception.Data Dictionary:
        //            ex.Data.Add(i, errors.ElementAt(i));
        //        }
        //    }
        //    // Othertimes, there may not be Model Errors:
        //    else
        //    {
        //        var error =
        //            JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
        //        foreach (var kvp in error)
        //        {
        //            // Wrap the errors up into the base Exception.Data Dictionary:
        //            ex.Data.Add(kvp.Key, kvp.Value);
        //        }
        //    }
        //}
    }
}
