using ClientTest.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
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
        /// ユーザーの情報。外部からは読み取り専用。
        /// </summary>
        public UserInfo UserInfo { get; private set; }

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
        public async Task LoginAsync(String username, String password)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> 
                { 
                    {"grant_type", "password"},
                    {"username", username},
                    {"password", password}
                }
            );

            //トークンの取得
            using (var client = new HttpClient())
            {
                var res = await client.PostAsync( _serverPath + "Token", content);

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);

                dynamic deserializedContent = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync());
                AccessToken = deserializedContent.access_token;
            }

            //ユーザー情報の読み取り
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessToken);

                var res = await client.GetAsync( _serverPath+"api/Account/LoginUserInfo");

                //エラーチェック
                await ApiServerResponseErrorHandler.CheckResponseStatus(res);

                //中身を展開
                var json = await res.Content.ReadAsStringAsync();

                //中身をチェック
                if (!String.IsNullOrEmpty(json) && json != "null")
                {
                    UserInfo = JsonConvert.DeserializeObject<UserInfo>(json);
                }
            }
        }

        public async Task LogoutAsync()
        {
            //なんか意味ないらしい。
            //何もしないAsync操作
            await Task.Run(() => { });

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", AccessToken);

            //    var res = await client.PostAsync(_serverPath + "api/Account/Logout",null);

            //    //エラーチェック
            //    await ApiServerResponseErrorHandler.CheckResponseStatus(res);
            //}

            //手元の情報をリセット
            AccessToken = null;
            UserInfo = null;
        }
    }
}
