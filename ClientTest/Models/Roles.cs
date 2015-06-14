using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientTest.Models
{
    public class Role
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        /// <summary>
        /// ListViewで選択される際に必要
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
