using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneServer.Models
{
    public class Memo
    {
        public int Id { get; set; }

        public String OwnerId { get; set; }

        public String Content { get; set; }
    }
}