using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OneServer.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace OneServer.Controllers
{
    [RoutePrefix("api/Roles")]
    [Authorize(Roles = "Administrators")]
    public class RolesController : BaseApiController
    {
        [Route("{id:guid}",Name = "GetRole")]
        public async Task<IHttpActionResult> GetRole(string Id)
        {
            var role = await this.RoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                var factory = new ModelFactory();
                return Ok(factory.Create(role));
            }

            return NotFound();

        }

        [Route("")]
        public IHttpActionResult GetAllRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            //var rs = db.Roles.First<ApplicationRole>();
            var rmRoles = this.RoleManager.Roles.First<ApplicationRole>();
            var id = rmRoles.Id;

            var roles = this.RoleManager.Roles;

            return Ok(roles);
        }

        //POST /api/Roles
        [Route("",Name="PostRole")]
        [HttpPost]
        public async Task<IHttpActionResult> PostRole(CreateUpdateRoleBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            var role = new ApplicationRole { Name = model.Name, Description = model.Description };

            var result = await this.RoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("PostRole", new { id = role.Id }));

            var factory = new ModelFactory();
            return Created(locationHeader, factory.Create(role));
        }

        //PUT /api/Roles/{guid}
        [Route("{Id:guid}")]
        [HttpPut]
        public IHttpActionResult PutRole([FromUri] String Id, [FromBody] CreateUpdateRoleBindingModel model)
        {
            if( String.IsNullOrEmpty(Id) || !ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            var role = RoleManager.FindById(Id);
            //変更
            role.Name = model.Name;
            role.Description = model.Description;
            var result = RoleManager.Update(role);

            if(!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        //DELETE /api/Roles/{guid}
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string Id)
        {
            var role = await this.RoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                IdentityResult result = await this.RoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        /*
        [Route("ManageUsersInRole")]
        [HttpPost]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var role = await this.RoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
            {
                var appUser = await userManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!userManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var appUser = await userManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
         * */
    }
}