﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Security;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Windows;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientTest.Models
{
    /// <summary>
    /// 認証に必要なモデル
    /// </summary>
    [DataContract]
    public class Authorizer : NotificationObject
    {
        [DataMember(Name = "Email")]
        public String UserName { get; set; }

        [DataMember(Name = "Password")]
        public String Password { get; set; }

        [DataMember(Name = "ConfirmPassword")]
        public String ConfirmPassword { get; set; }

        public String Token { get; set; }

        public class ModelState
        {
            public List<string> Password { get; set; }
        }

        public class Response
        {
            public string Message { get; set; }
            public ModelState ModelState { get; set; }
        }

        public async void Register()
        {
            using (var client = new HttpClient())
            {
                var sendContent = JsonConvert.SerializeObject(this);

                var res = await client.PostAsync(
                    new Uri( App.Current.FindResource("APIServerPath") + "api/Account/Register"),
                    new StringContent(sendContent, Encoding.UTF8, "application/json"));

                if (res.IsSuccessStatusCode == true)
                {
                    MessageBox.Show(String.Format("登録しました！\nようこそ{0}さん",UserName));
                }
                else
                {
                    throw new ApplicationException(String.Format("登録できませんでした(コード：{0})。", res.StatusCode));
                }
            }
        }
        
        public async Task<TokenReceiver> Login(String username,String password)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> 
                { 
                    { "grant_type", "password"},
                    { "username", username  },
                    { "password", password}
                }
            );

            using(var client = new HttpClient())
            {
                var res = await client.PostAsync( App.Current.FindResource("APIServerPath") + "Token", content);

                res.EnsureSuccessStatusCode();

                if (res.IsSuccessStatusCode)
                {
                    var ser = new DataContractJsonSerializer(typeof(TokenReceiver));
                    var token = (TokenReceiver)ser.ReadObject(await res.Content.ReadAsStreamAsync());
                    return token;
                }
                else throw new ApplicationException("ログインできまんせん");
            }
        }
    }
}
