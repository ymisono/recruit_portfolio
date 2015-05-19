using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ClientTest.Models
{
    [DataContract]
    public class UserInfo : NotificationObject
    {
        [DataMember(Name="Email")]
        public String Username { get; set; }

        public async Task Fetch(TokenReceiver token)
        {
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);

                var res = await client.GetAsync("http://oneserversite.azurewebsites.net/api/Account/UserInfo");

                var content = await res.Content.ReadAsStringAsync();

                var ser = new DataContractJsonSerializer(typeof(UserInfo));
                var newUserInfo = (UserInfo)ser.ReadObject(await res.Content.ReadAsStreamAsync());
                this.Username = newUserInfo.Username;
            }
        }
    }
}
