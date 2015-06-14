using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using ClientTest.Models;

namespace UserManageUtility.Models
{
    public class SelectableUserInfo : UserInfo
    {
        public bool IsSelected { get; set; }
    }
}
