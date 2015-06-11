using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientTest.Validator
{
    class UserNameValidator : ValidationRule
    {
        private static readonly Regex AlphaNumUnderscoreRegex = new Regex(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //if (value.ToString().Length < 1)
            //{
            //    return new ValidationResult(false, "Age field cannot be empty");
            //}

            if (!AlphaNumUnderscoreRegex.IsMatch(value.ToString()))
            {
                return new ValidationResult(false, "半角英数字とアンダースコア(_)のみ使用可能です");
            }
            else
            {
                //正しかった
                return new ValidationResult(true, null);
            }
        }
    }
}
