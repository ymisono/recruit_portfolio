using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientTest.Models
{
    public class Role : IEquatable<Role>
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        /// <summary>
        /// ListViewで選択される際に必要
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 比較オペレーター
        /// </summary>
        public bool Equals(Role obj)
        {
            if (obj == null) return false;

            //IDとUserNameが一致すれば、等価
            return this.Id == obj.Id && this.Name == obj.Name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Role);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                hash = hash * 16777619 ^ Id.GetHashCode();
                hash = hash * 16777619 ^ Name.GetHashCode();
                if (Description != null)
                    hash = hash * 16777619 ^ Description.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
