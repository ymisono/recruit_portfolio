using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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
        /// <typeparam name="T">データを詰めたいDTOを指定。</typeparam>
        public async Task<String> Fetch(String controller)
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
                        "{0}{1}", _apiPath, controller)
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
        
    }
}
