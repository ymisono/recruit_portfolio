namespace OneServer.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OneServer.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            //misonoアカウント
            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser() { UserName = "misono", Email = "misono@test.com", PasswordHash = new PasswordHasher().HashPassword("password") }
            );

            //アドミン
            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser() { UserName = "admin", PasswordHash = new PasswordHasher().HashPassword("password") }
            );

            context.SaveChanges();

            //Get the UserId only if the SecurityStamp is not set yet.
            string userId = context.Users.Where(x => x.UserName == "misono" && string.IsNullOrEmpty(x.SecurityStamp)).Select(x => x.Id).FirstOrDefault();

            //If the userId is not null, then the SecurityStamp needs updating.
            if (!string.IsNullOrEmpty(userId)) userManager.UpdateSecurityStamp(userId);

            //ロール
            var adminRole = context.Roles.SingleOrDefault(x => x.Name == "Administrator");
            //adminがなければ
            if (adminRole == null)
            {
                context.Roles.AddOrUpdate(r => r.Name,
                    new ApplicationRole() { Name = "Administrator", Description = "管理者ロール。管理者権限の操作ができる。" });

                context.SaveChanges();

                //adminUserを入手
                var admin = context.Users.Single(u => u.UserName == "admin");

                if (admin != null)
                {
                    //UserRoleで対応関係を作る
                    userManager.AddToRole(admin.Id, "Administrator"); //この時点でAdminstratorはあるはず
                }
            }

            //Memo
            var myId = context.Users.Single(x => x.UserName == "misono").Id;
            context.Memos.AddOrUpdate(
                m=>m.OwnerId,
                new OneServer.Models.Memo { OwnerId = myId, Content = "あああああ\nてすてす" }
            );

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
