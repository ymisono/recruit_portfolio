using System;

using Livet;
using Newtonsoft.Json;

namespace ClientTest.Models
{
    public class UserInfo : NotificationObject
    {
        public String Id { get; set; }

        public String UserName { get; set; }

        public String Email { get; set; }

        public static UserInfo Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<UserInfo>(json);
        }
    }
}
