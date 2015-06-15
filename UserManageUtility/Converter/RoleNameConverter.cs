using ClientTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UserManageUtility.Converter
{
    public class RoleNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var roles = value as ICollection<Role>;

            if (roles.Count == 0) return null;

            String resultString = "";
            foreach(var r in roles)
            {
                resultString += String.IsNullOrEmpty(resultString) ? r.Name : ", " + r.Name;
            }

            return resultString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
