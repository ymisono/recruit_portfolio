using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest.Models
{
    public class Memo
    {
        public int Id { get; set; }

        public String OwnerId { get; set; }

        public String Content { get; set; }

        public Memo()
        {
            Id = -1; //一回も保存されてない場合、-1
        }

        public async Task Fetch(UserInfo user)
        {
            var temp = new Memo();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", App.Current.Properties["Token"] as String);

                var res = await client.GetAsync( String.Format(
                        "{0}api/Memos?ownerid={1}",
                        App.Current.Properties["APIServerPath"],
                        user.Id
                    ));


                var content = await res.Content.ReadAsStringAsync();

                //メモがなかったらそのまま終わる
                if (!String.IsNullOrEmpty(content) && content != "null")
                {
                    var tempMe = Newtonsoft.Json.JsonConvert.DeserializeObject<Memo>(content);

                    this.Id = tempMe.Id;
                    this.OwnerId = tempMe.OwnerId;
                    this.Content = tempMe.Content;
                }
            }
        }

        public async Task Store(UserInfo user,String text)
        {
            this.Content = text;
            this.OwnerId = user.Id;

            using (var client = new HttpClient())
            {
                HttpResponseMessage res = null;

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.Current.Properties["Token"] as String);

                //ここにJSON
                var sendContent = Newtonsoft.Json.JsonConvert.SerializeObject(this);

                //最初の時はPOST（追記）。idがある場合はそのままPUT（更新）
                if (this.Id == -1)
                {
                    //POST
                    res = await client.PostAsync(
                            new Uri(App.Current.Properties["APIServerPath"] + "api/Memos"),
                            new StringContent(sendContent, Encoding.UTF8, "application/json")
                        );
                }
                else
                {
                    //PUT
                    res = await client.PutAsync(
                            String.Format("{0}api/Memos/{1}",App.Current.Properties["APIServerPath"], this.Id),
                            new StringContent(sendContent, Encoding.UTF8, "application/json")
                        );
                }

                res.EnsureSuccessStatusCode();
            }
        }
    }
}
