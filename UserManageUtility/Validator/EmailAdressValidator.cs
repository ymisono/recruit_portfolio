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
    class EmailAdressValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var targetName = value as String;

            //空は無視
            if (value == null || String.IsNullOrEmpty(targetName.Trim()))
            {
                return new ValidationResult(true, null);
            }

            //組み込みクラスを頼りに正規のアドレスか判定する
            try
            {
                var m = new MailAddress(targetName);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "正しいアドレス形式ではありません");
            }

            //正しかった
            return new ValidationResult(true, null);
        }
    }
}
