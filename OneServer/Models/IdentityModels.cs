using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System;

namespace OneServer.Models
{
    // ApplicationUser クラスにプロパティを追加することでユーザーのプロファイル データを追加できます。詳細については、http://go.microsoft.com/fwlink/?LinkID=317594 を参照してください。
    public class ApplicationUser : IdentityUser
    {
        #region カスタムプロパディ

        /// <summary>
        /// 姓
        /// </summary>
        public String LastName { get; set; }
        /// <summary>
        /// 名
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// 姓（フリガナ）
        /// </summary>
        public String LastNameKana { get; set; }
        /// <summary>
        /// 名（フリナガ）
        /// </summary>
        public String FirstNameKana { get; set; }

        /// <summary>
        /// 削除フラグ。実際の運用ではDELETEをしない。
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion カスタムプロパディ

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // ここにカスタム ユーザー クレームを追加します
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DataBaseConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Memo> Memos { get; set; }

    }

}