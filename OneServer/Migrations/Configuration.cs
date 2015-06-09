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

            context.Users.AddOrUpdate(
                u => u.UserName,
                new ApplicationUser() { UserName = "misono", Email = "misono@test.com", PasswordHash = new PasswordHasher().HashPassword("password") }
            );

            context.SaveChanges();

            //Get the UserId only if the SecurityStamp is not set yet.
            string userId = context.Users.Where(x => x.UserName == "misono" && string.IsNullOrEmpty(x.SecurityStamp)).Select(x => x.Id).FirstOrDefault();

            //If the userId is not null, then the SecurityStamp needs updating.
            if (!string.IsNullOrEmpty(userId)) userManager.UpdateSecurityStamp(userId);

            //Memo
            var myId = context.Users.Single(x => x.UserName == "misono").Id;
            context.Memos.AddOrUpdate(
                m=>m.Id,
                new OneServer.Models.Memo { OwnerId = myId, Content = "‚ ‚ ‚ ‚ ‚ \n‚Ä‚·‚Ä‚·" }
            );

            context.SaveChanges();

            //base.Seed(context);
        }
    }
}
