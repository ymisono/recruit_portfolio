using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OneServer.Models;
using ClientTest.Utility;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Configuration;

namespace OneServer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<OneServer.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
#if DEBUG
            AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(OneServer.Models.ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            #region admin設定
            //アドミン(ローカルでのみシードは実行)
            var newUserName = "admin";
            var adminPass = WebConfigurationManager.AppSettings["AdminPass"];
            var admin = new ApplicationUser() { UserName = newUserName, PasswordHash = new PasswordHasher().HashPassword(adminPass) };
            context.Users.AddOrUpdate( u => u.UserName, admin );

            #endregion admin設定

            //ロール
            var adminRole = context.Roles.SingleOrDefault(x => x.Name == "Administrators");
            //adminがなければ
            if (adminRole == null)
            {
                context.Roles.AddOrUpdate(r => r.Name,
                    new ApplicationRole() { Name = "Administrators", Description = "管理者ロール。管理者権限の操作ができる。" });
            }

            context.SaveChanges();

            //adminにAdministratorsRoleを追加
            if (!userManager.IsInRole(admin.Id, "Administrators"))
            {
                userManager.AddToRole(admin.Id, "Administrators");
            }

            //Get the UserId only if the SecurityStamp is not set yet.
            string userId = context.Users.Where(x => x.UserName == newUserName && string.IsNullOrEmpty(x.SecurityStamp)).Select(x => x.Id).FirstOrDefault();

            //If the userId is not null, then the SecurityStamp needs updating.
            if (!string.IsNullOrEmpty(userId)) userManager.UpdateSecurityStamp(userId);


            ////Memo
            //var myId = context.Users.Single(x => x.UserName == "misono").Id;
            //context.Memos.AddOrUpdate(
            //    m=>m.OwnerId,
            //    new OneServer.Models.Memo { OwnerId = myId, Content = "あああああ\nてすてす" }
            //);


            base.Seed(context);
        }
    }
}
