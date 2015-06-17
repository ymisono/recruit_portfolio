using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UserManageUtility.Validator
{
    class PhoneNumberValidator : ValidationRule
    {
        private static readonly Regex PhoneNumberPattern = new Regex(@"^((\(\d{3}\) ?)|(\d{3}-))?(\d{3}|\d{4})-\d{4}$", RegexOptions.Compiled | RegexOptions.Singleline);

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var targetNumber = value as String;

            //空は無視
            if (value == null || String.IsNullOrEmpty(targetNumber))
            {
                return new ValidationResult(true, null);
            }

            if(!PhoneNumberPattern.Match(targetNumber).Success)
            {
                return new ValidationResult(false, "正しい電話番号の形式ではありません。\n例：123-234-5678、(080)123-4568。");
            }

            return new ValidationResult(true, null);
        }
    }
}
