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
        private Session _session;
        /// <summary>
        /// サーバーのセッション
        /// </summary>
        public Session MySession { get; private set; }

        //サーバーAPIのパス。APIを呼び出す基点になる。
        private String _apiPath;

        ApiServer()
        {
            MySession = new Session();
            _apiPath = String.Format("{0}api/", App.Current.Properties["APIServerPath"]);
        }

        /// <summary>
        /// 汎用的な取得メソッド
        /// </summary>
        /// <typeparam name="T">データを詰めたいDTOを指定。</typeparam>
        public async Task<T> Fetch<T>(String controller)
        {
            //ログインしているか確認
            if (!MySession.IsLoggedIn) return default(T);

            ////接続開始
            //using (var client = new HttpClient())
            //{
            //    //認証の為に、トークンをヘッダーにセット。
            //    client.DefaultRequestHeaders.Authorization =
            //        new AuthenticationHeaderValue("Bearer", MySession.AccessToken);

            //    var res = await client.GetAsync(String.Format(
            //            "{0}{1}", _apiPath, controller)
            //            );

            //    MySession.CheckResponseStatus(res);

            //    //中身を展開
            //    var content = await res.Content.ReadAsStringAsync();

            //    //中身をチェック
            //    if (!String.IsNullOrEmpty(content) && content != "null")
            //    {
            //        //辞書型の列挙に詰める
            //        var dics = JsonConvert.DeserializeObject<Dictionary<string, T>>(content);
                    
            //        foreach(var dic in dics)
            //        {
                        
            //        }
            //     }
            //}
        }
    }
}
