using System;
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

        public async void Register()
        {
            using (var client = new HttpClient())
            {

                var serializer = new DataContractJsonSerializer(typeof(Authorizer));
                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, this);
                    var tempJson = Encoding.UTF8.GetString(ms.ToArray());
                    var content = new StringContent(tempJson,Encoding.UTF8,"application/json");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                    var url = new Uri("http://oneserversite.azurewebsites.net/api/Account/Register");
                    var res = await client.PostAsync(url, content);
                    if (res.IsSuccessStatusCode == true)
                    {
                        MessageBox.Show(String.Format("登録しました！\nようこそ{0}さん",UserName));
                    }
                    else
                    {
                        MessageBox.Show("登録できませんでした。");
                    }
                }
            }
        }
        
        public async void Login()
        {

        }
    }
}
