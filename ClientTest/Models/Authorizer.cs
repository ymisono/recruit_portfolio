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
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.ModelBinding;

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

        public async Task Register()
        {
            using (var client = new HttpClient())
            {
                var sendContent = JsonConvert.SerializeObject(this);

                var res = await client.PostAsync(
                    new Uri( App.Current.Properties["APIServerPath"] + "api/Account/Register"),
                    new StringContent(sendContent, Encoding.UTF8, "application/json"));

                if (res.IsSuccessStatusCode == true)
                {
                    MessageBox.Show(String.Format("登録しました！\nようこそ{0}さん",UserName));
                }
                else
                {
                    var body = await res.Content.ReadAsStringAsync();
                    var state = JsonConvert.DeserializeObject<Receiver>(body);
                    //state.modelstate.

                    throw new ApplicationException(String.Format("登録できませんでした(コード：{0})。\n理由：{1}", res.StatusCode,body));
                }
            }
        }
        
        public async Task Login(String username,String password)
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
                var res = await client.PostAsync( App.Current.Properties["APIServerPath"] + "Token", content);

                res.EnsureSuccessStatusCode();

                App.Current.Properties["Token"] = (JsonConvert.DeserializeObject<TokenReceiver>(await res.Content.ReadAsStringAsync())).Token;
            }
        }

    }
}

public class Receiver
{
    public String Message;
    [JsonProperty(PropertyName = "ModelState")]
    public ModelState modelstate;
}

