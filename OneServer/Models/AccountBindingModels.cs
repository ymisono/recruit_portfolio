﻿using ClientTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneServer.Models
{
    // AccountController アクションへのパラメーターとして使用されるモデル。

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "外部アクセス トークン")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "現在のパスワード")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} の長さは、{2} 文字以上である必要があります。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワード")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワードの確認入力")]
        [Compare("NewPassword", ErrorMessage = "新しいパスワードと確認のパスワードが一致しません。")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} の長さは、{2} 文字以上である必要があります。", MinimumLength = 4)]
        [Display(Name = "ユーザー名")]
        public string UserName { get; set; }

        [Display(Name = "電子メール")]
        //[EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} の長さは、{2} 文字以上である必要があります。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

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


        public ICollection<ClientTest.Models.Role> Roles { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "電子メール")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "ログイン プロバイダー")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "プロバイダー キー")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} の長さは、{2} 文字以上である必要があります。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワード")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワードの確認入力")]
        [Compare("NewPassword", ErrorMessage = "新しいパスワードと確認のパスワードが一致しません。")]
        public string ConfirmPassword { get; set; }
    }
}
