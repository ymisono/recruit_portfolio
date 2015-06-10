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

        public UserInfo Deserialize(String json)
        {
            var deserializedObj = JsonConvert.DeserializeObject<UserInfo>(json);
            if (deserializedObj != null)
            {
                this.Id = deserializedObj.Id;
                this.UserName = deserializedObj.UserName;
                this.Email = deserializedObj.Email;
            }

            return this;
        }
    }
}
