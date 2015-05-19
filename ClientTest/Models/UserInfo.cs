using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientTest.Models
{
    [DataContract]
    public class UserInfo : NotificationObject
    {
        [DataMember(Name="Id")]
        public String Id { get; set; }

        [DataMember(Name="Email")]
        public String UserName { get; set; }

        public async Task Fetch()
        {
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.Current.Properties["Token"] as String);

                var res = await client.GetAsync( App.Current.Properties["APIServerPath"] + "api/Account/UserInfo");

                var content = await res.Content.ReadAsStringAsync();

                var tempThis = JsonConvert.DeserializeObject<UserInfo>(content);
                if (tempThis != null)
                {
                    this.Id = tempThis.Id;
                    this.UserName = tempThis.UserName;
                }

                //var ser = new DataContractJsonSerializer(typeof(UserInfo));
                //var newUserInfo = (UserInfo)ser.ReadObject(await res.Content.ReadAsStreamAsync());
                //this.UserName = newUserInfo.UserName;
            }
        }
    }
}
