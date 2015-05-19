using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneServer.Models
{
    public class Memo
    {
        public int Id { get; set; }

        public virtual ApplicationUser ApplicationUserId { get; set; }

        public String Content { get; set; }
    }
}