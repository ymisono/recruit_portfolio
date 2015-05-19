using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Runtime.Serialization;

namespace ClientTest.Models
{
    [DataContract]
    public class TokenReceiver : NotificationObject
    {
        [DataMember(Name="access_token")]
        public String Token { get; set; }
    }
}