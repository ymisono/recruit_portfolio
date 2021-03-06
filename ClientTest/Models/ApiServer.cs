﻿using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ClientTest.Utility;
using System.Collections;
using System.Collections.Generic;

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

            //設定ファイルからサーバーのパスを持ってくる
            //DEBUG時はローカルホストのサーバーを使い、Release環境ならばAzureのリモートサーバーを使う
#if DEBUG
            _apiPath = String.Format("{0}api/", ConfigurationManager.AppSettings["LocalAPIServerPath"]);
#else
            _apiPath = String.Format("{0}api/", ConfigurationManager.AppSettings["RemoteAPIServerPath"]);
#endif //DEBUG
        }
        #endregion

        /// <summary>
        /// 汎用的な取得メソッド。
        /// </summary>
        /// <returns>返り値はJSONで</returns>
        public async Task<T> ReadAsync<T>(String restResource)
        {
            T result = default(T);

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
                var json = await res.Content.ReadAsStringAsync();

                //中身をチェック
                if (!String.IsNullOrEmpty(json) && json != "null")
                {
                    result = JsonConvert.DeserializeObject<T>(json);
                }

                return result;
            }
        }

        public async Task<T> CreateAsync<T>(T newObj, String restResource)
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

                var content = await res.Content.ReadAsStringAsync();
                //作ったものをそっくり返す
                return JsonConvert.DeserializeObject<T>(content);

            }
        }

        public async Task UpdateAsync<T>(T newObj, String restResource)
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

        /// <summary>
        /// 単体のエンティテイを更新する
        /// </summary>
        public async Task UpdateByIdAsync<T>(T newObj, String restResource)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentSession.AccessToken);

                var sendContent = JsonConvert.SerializeObject(newObj);

                dynamic entity = newObj;

                //PUT
                var res = await client.PutAsync(
                        new Uri(_apiPath + restResource + "/" + entity.Id),
                        new StringContent(sendContent, Encoding.UTF8, "application/json")
                    );

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);
            }
        }

        public async Task DeleteByIdAsync(String id, String restResource)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", CurrentSession.AccessToken);

                var res = await client.DeleteAsync(
                        new Uri(_apiPath + restResource + "/" + id)
                    );

                await ApiServerResponseErrorHandler.CheckResponseStatus(res);
            }
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task RegisterAsync(
            String userName, String password,
            String lastName = null, String firstName = null,
            String lastNameKana = null, String firstNameKana = null,
            String email = null, String phoneNumber = null,
            IEnumerable<Role> roles = null)
        {
            var sendObj = new
            { 
                UserName = userName,
                Password = password,
                LastName = lastName, FirstName = firstName,
                LastNameKana = lastNameKana, FirstNameKana = firstNameKana,
                Email = email, PhoneNumber = phoneNumber,
                Roles = roles
            };

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
