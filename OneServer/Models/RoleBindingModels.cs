using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace OneServer.Models
{
    public class CreateUpdateRoleBindingModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class UsersInRoleModel
    {
        public string Id { get; set; }
        public List<string> EnrolledUsers { get; set; }
        public List<string> RemovedUsers { get; set; }
    }

    public class ModelFactory
    {
        //Code removed for brevity
        public static RoleReturnModel Create(ApplicationRole appRole)
        {
            return new RoleReturnModel
            {
                //Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name,
                Description = appRole.Description
            };
        }
    }

    public class RoleReturnModel
    {
        //public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}