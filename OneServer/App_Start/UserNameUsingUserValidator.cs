using Microsoft.AspNet.Identity;
using OneServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OneServer.App_Start
{
    /// <summary>
    /// デフォだとメールアドレスを使うが、これはUserNameをユーザー名をして扱う検証器。
    /// A replacement for the <see cref="UserValidator"/> which requires that an email 
    /// address be used for the <see cref="IUser.UserName"/> field.
    /// </summary>
    /// <typeparam name="TUser">Must be a type derived from <see cref="Microsoft.AspNet.Identity.IUser"/>.</typeparam>
    /// <remarks>
    /// This validator check the <see cref="IUser.UserName"/> property against the simple email regex provided at
    /// http://www.regular-expressions.info/email.html. If a <see cref="UserManager"/> is provided in the constructor,
    /// it will also ensure that the email address is not already being used by another account in the manager.
    /// 
    /// To use this validator, just set <see cref="UserManager.UserValidator"/> to a new instance of this class.
    /// </remarks>
    public class UserNameUsingUserValidator<TUser> : IIdentityValidator<TUser>
        where TUser : class, Microsoft.AspNet.Identity.IUser
    {
        private static readonly Regex EmailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex AlphaNumUnderscoreRegex = new Regex(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
        private readonly UserManager<TUser> _manager;

        public UserNameUsingUserValidator()
        {
        }

        public UserNameUsingUserValidator(UserManager<TUser> manager)
        {
            _manager = manager;
        }

        public async Task<IdentityResult> ValidateAsync(TUser item)
        {
            var errors = new List<string>();

            //ユーザー名が英数字のみかチェック
            if (!AlphaNumUnderscoreRegex.IsMatch(item.UserName))
            {
                errors.Add("ユーザー名には半角英数字とアンダースコア(_)のみが使えます。");
            }

            var appUser = item as ApplicationUser;
            if (appUser != null)
            {
                if ( !String.IsNullOrEmpty(appUser.Email) && //空ではなく
                    !EmailRegex.IsMatch(appUser.Email) ) //有効なアドレスではない時
                {
                    errors.Add("有効なメールアドレスの形式ではありません。");
                }
            }

            if (_manager != null)
            {
                var otherAccount = await _manager.FindByNameAsync(item.UserName);
                if (otherAccount != null && otherAccount.Id != item.Id)
                    errors.Add("既に存在するユーザー名です。");
            }

            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }
    }
}