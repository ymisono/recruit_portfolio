using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneServer.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name) : base(name) { }

        public ApplicationRole() { }

        public string Description { get; set; }
    }
}