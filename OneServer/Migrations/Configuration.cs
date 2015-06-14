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

            //misono�A�J�E���g
            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser() { UserName = "misono", Email = "misono@test.com", PasswordHash = new PasswordHasher().HashPassword("password") }
            );

            //�A�h�~��
            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser() { UserName = "admin", PasswordHash = new PasswordHasher().HashPassword("password") }
            );

            context.SaveChanges();

            //Get the UserId only if the SecurityStamp is not set yet.
            string userId = context.Users.Where(x => x.UserName == "misono" && string.IsNullOrEmpty(x.SecurityStamp)).Select(x => x.Id).FirstOrDefault();

            //If the userId is not null, then the SecurityStamp needs updating.
            if (!string.IsNullOrEmpty(userId)) userManager.UpdateSecurityStamp(userId);

            //���[��
            var adminRole = context.Roles.SingleOrDefault(x => x.Name == "Administrator");
            //admin���Ȃ����
            if (adminRole == null)
            {
                context.Roles.AddOrUpdate(r => r.Name,
                    new ApplicationRole() { Name = "Administrator", Description = "�Ǘ��҃��[���B�Ǘ��Ҍ����̑��삪�ł���B" });

                context.SaveChanges();

                //adminUser�����
                var admin = context.Users.Single(u => u.UserName == "admin");

                if (admin != null)
                {
                    //UserRole�őΉ��֌W�����
                    userManager.AddToRole(admin.Id, "Administrator"); //���̎��_��Adminstrator�͂���͂�
                }
            }

            //Memo
            var myId = context.Users.Single(x => x.UserName == "misono").Id;
            context.Memos.AddOrUpdate(
                m=>m.OwnerId,
                new OneServer.Models.Memo { OwnerId = myId, Content = "����������\n�Ă��Ă�" }
            );

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
