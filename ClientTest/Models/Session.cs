using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace ClientTest.Models
{
    /// <summary>
    /// 接続を抽象化する。
    /// </summary>
    public class Session : NotificationObject
    {
        /// <summary>
        /// Web Apiのエンドポイントに渡すアクセストークン
        /// ログイン操作をすると貰える。
        /// </summary>
        public String AccessToken { get; private set; }

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
                var res = await client.PostAsync(App.Current.Properties["APIServerPath"] + "Token", content);

                await CheckResponseStatus(res);

                dynamic deserializedContent = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync());
                AccessToken = deserializedContent.access_token;
                //var deserializedContent = JsonConvert.DeserializeObject<Dictionary<string, object>>(await res.Content.ReadAsStringAsync());
                //_accessToken = deserializedContent["access_token"] as string;
            }
        }

        /// <summary>
        /// HTTP応答をチェックする。
        /// 応答が正常でなければ、例外を投げる
        /// </summary>
        /// <param name="res">HttpClientから応答を受け取る</param>
        /// <exception cref="ApplicationException">500:サーバー内の状態が不正。</exception>
        public async Task CheckResponseStatus(HttpResponseMessage res)
        {
            //応答ステータスチェック
            if(!res.IsSuccessStatusCode)
            {
                //中身をロード
                var content = await res.Content.ReadAsStringAsync();

                switch(res.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new ApplicationException(String.Format("不正な入力です(401)。\n理由：{0}", content));
                    case HttpStatusCode.Unauthorized:
                        throw new ApplicationException(String.Format("認証に失敗しました(401)。\n再ログインしてください。\n理由：{0}", content));
                    case HttpStatusCode.InternalServerError:
                        throw new ApplicationException(String.Format("サーバー内で不正な処理が発生しました(500)。\n理由：{0}",content));
                    case HttpStatusCode.ServiceUnavailable:
                        throw new ApplicationException(String.Format("サーバーが一時的に利用できません(503)。\n理由：{0}",content));

                    default:
                        throw new ApplicationException("想定してないエラー");
                }
            }
        }
    }
}
