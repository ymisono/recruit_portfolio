using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientTest.Models
{
    /// <summary>
    /// 接続を抽象化する。
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Web Apiのエンドポイントに渡すアクセストークン
        /// ログイン操作をすると貰える。
        /// </summary>
        public String AccessToken { get; private set; }

        /// <summary>
        /// 接続するサーバーのパス
        /// </summary>
        private String _serverPath;

        /// <summary>
        /// ログインしているか、否か。
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return !String.IsNullOrEmpty(AccessToken) ? true : false;
            }
        }

        #region コンストラクタ
        public Session()
        {
#if DEBUG
            _serverPath = ConfigurationManager.AppSettings["LocalAPIServerPath"];
#else
            _serverPath = ConfigurationManager.AppSettings["RemoteAPIServerPath"];
#endif //DEBUG
        }
        #endregion

        /// <summary>
        /// Web Apiエンドポイントに対してログインを行う
        /// </summary>
        /// <param name="username">ユーザー名</param>
        /// <param name="password">パスワード</param>
        public async Task Login(String username, String password)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> 
                { 
                    {"grant_type", "password"},
                    {"username", username},
                    {"password", password}
                }
            );

            using (var client = new HttpClient())
            {
                var res = await client.PostAsync( _serverPath + "Token", content);

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);

                dynamic deserializedContent = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync());
                AccessToken = deserializedContent.access_token;
            }
        }
    }
}
