﻿using Microsoft.AspNet.Identity;
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
    public class RolesController : BaseApiController
    {
        [Route("{id:guid}",Name = "GetBookById")]
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
            IdentityRole rs = db.Roles.First<IdentityRole>();
            IdentityRole rmRoles = this.RoleManager.Roles.First<IdentityRole>();
            var id = rmRoles.Id;

            var roles = this.RoleManager.Roles;

            var misono = db.Users.Single(m => m.UserName == "misono");
            var misonoRole = misono.Roles;

            return Ok(id);
        }

        [Route("Create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var result = await this.RoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetBookById", new { id = role.Id }));

            var factory = new ModelFactory();
            return Created(locationHeader, factory.Create(role));

        }

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
    }
}