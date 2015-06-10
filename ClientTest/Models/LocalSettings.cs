using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest.Models
{
    /// <summary>
    /// ローカルに保存する設定を管理するクラス。
    /// </summary>
    public class LocalSettings
    {
        public static String ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var result = appSettings[key];
                if (result == null) throw new ApplicationException("設定ファイルの値が存在しません。");
                return appSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                throw new ApplicationException("設定ファイルの読み込みに失敗しました。");
            }
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                throw new ApplicationException("設定ファイルの書き込みに失敗しました。");
            }
        }
    }
}
