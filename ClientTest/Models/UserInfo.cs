using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClientTest.Models
{
    public class UserInfo
    {
        public String Id { get; set; }

        public String UserName { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public String LastName { get; set; }
        /// <summary>
        /// 名
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// 本名（空白なし）
        /// </summary>
        public String FullName
        {
            get { return LastName + FirstName; }
        }

        /// <summary>
        /// 本名（半角空白）
        /// </summary>
        public String FullNameWithHalfSpace
        {
            get { return LastName + " " + FirstName; }
        }

        /// <summary>
        /// 姓（フリガナ）
        /// </summary>
        public String LastNameKana { get; set; }
        /// <summary>
        /// 名（フリナガ）
        /// </summary>
        public String FirstNameKana { get; set; }

        /// <summary>
        /// 本名（空白なし）
        /// </summary>
        public String FullNameKana
        {
            get { return LastNameKana + FirstNameKana; }
        }

        /// <summary>
        /// 本名（空白なし）
        /// </summary>
        public String FullNameKanaWithHalfSpace
        {
            get { return LastNameKana + " " + FirstNameKana; }
        }

        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return String.Format(
                  CultureInfo.CurrentCulture
                , "{{ Id = {0}, UserName = {1} }}"
                , this.Id.ToString(CultureInfo.InvariantCulture)
                , this.UserName.ToString(CultureInfo.InvariantCulture)
                );
        }

        /// <summary>
        /// 日本語形式でのフルネームを取得する
        /// </summary>
        /// <returns></returns>
        public String GetFullName(String separater = "")
        {
            return LastName + separater ?? "" + FirstName;
        }
    }
}
