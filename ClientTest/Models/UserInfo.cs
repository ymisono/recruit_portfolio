﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClientTest.Models
{
    public class UserInfo
    {
        public String Id { get; set; }

        public String UserName { get; set; }

        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public bool IsDeleted { get; set; }

        ///// <summary>
        ///// 比較オペレーター
        ///// </summary>
        //public bool Equals(UserInfo obj)
        //{
        //    if (obj == null) return false;

        //    //全項目（コレクションを除く）が一致すれば、等価
        //    return this.Id == obj.Id &&
        //        this.UserName == obj.UserName &&
        //        this.Email == obj.Email &&
        //        this.PhoneNumber == obj.PhoneNumber &&
        //        this.IsDeleted == obj.IsDeleted;
        //}

        //public override bool Equals(object obj)
        //{
        //    return this.Equals(obj as UserInfo);
        //}

        //public override int GetHashCode()
        //{
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = (int)2166136261;
        //        // Suitable nullity checks etc, of course :)
        //        hash = hash * 16777619 ^ Id.GetHashCode();
        //        hash = hash * 16777619 ^ UserName.GetHashCode();
        //        if (Email != null)
        //            hash = hash * 16777619 ^ Email.GetHashCode();
        //        return hash;
        //    }
        //}

        public override string ToString()
        {
            return String.Format(
                  CultureInfo.CurrentCulture
                , "{{ Id = {0}, UserName = {1} }}"
                , this.Id.ToString(CultureInfo.InvariantCulture)
                , this.UserName.ToString(CultureInfo.InvariantCulture)
                );
        }

        //public static bool operator ==(UserInfo leftOperand, UserInfo rightOperand)
        //{
        //    if (ReferenceEquals(null, leftOperand)) return ReferenceEquals(null, rightOperand);
        //    return leftOperand.Equals(rightOperand);
        //}

        //public static bool operator !=(UserInfo leftOperand, UserInfo rightOperand)
        //{
        //    return !(leftOperand == rightOperand);
        //}
    }
}
