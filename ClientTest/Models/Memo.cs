using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Fetch(UserInfo user)
        {
            var temp = new Memo();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", App.Current.Properties["Token"] as String);

                var res = await client.GetAsync(App.Current.Properties["APIServerPath"] + "api/Memos");

                var content = await res.Content.ReadAsStringAsync();
            }
        }
    }
}
