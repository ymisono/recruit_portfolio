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

        public static UserInfo GetBase(SelectableUserInfo b)
        {
            return new UserInfo
            {
                Id = b.Id,
                UserName = b.UserName,
                Email = b.Email,
                PhoneNumber = b.PhoneNumber,
                Roles = b.Roles,
                IsDeleted = b.IsDeleted
            };
        }
    }
}
