using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows修改注册表
{
    class Program
    {
        static void Main(string[] args)
        {
            //阻止安全检查对话框
            RegeditSet(HidRepairTool);
            //阻止Ie11浏览器重置后，弹出设置向导
            RegeditSet(DisableFirstRunCustomize);
            //阻止Ie11浏览器重置后，弹出询问“如果Internet Explorer 不是默认Web浏览器，则通知用户”
            RegeditSet(CheckAssociations);


        }


        /// <summary>
        /// 隐藏修复工具栏
        /// </summary>
        /// <param name="mreg"></param>
        private static void HidRepairTool(RegistryKey mreg)
        {
            try
            {
                var key = mreg.CreateSubKey(@"Software\Policies\Microsoft\Internet Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
                if (key != null)
                {
                    var security = key.CreateSubKey("Security");
                    if (security != null)
                    {
                        security.SetValue("DisableSecuritySettingsCheck", 1, RegistryValueKind.DWord);
                    }
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 阻止Ie11浏览器重置后，弹出设置向导，详情见链接：https://jingyan.baidu.com/article/9158e0001f7f8ba2541228cb.html
        /// </summary>
        /// <param name="mreg"></param>
        private static void DisableFirstRunCustomize(RegistryKey mreg)
        {
            try
            {
                var key = mreg.CreateSubKey(@"Software\Policies\Microsoft\Internet Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
                if (key != null)
                {
                    var security = key.CreateSubKey("Main");
                    if (security != null)
                    {
                        security.SetValue("DisableFirstRunCustomize", 1, RegistryValueKind.DWord);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 阻止Ie11浏览器重置后，弹出询问“如果Internet Explorer 不是默认Web浏览器，则通知用户”
        /// </summary>
        /// <param name="mreg"></param>
        private static void CheckAssociations(RegistryKey mreg)
        {
            try
            {
                var key = mreg.CreateSubKey(@"Software\Policies\Microsoft\Internet Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
                if (key != null)
                {
                    var security = key.CreateSubKey("Main");
                    if (security != null)
                    {
                        security.SetValue("Check_Associations", "no", RegistryValueKind.String);
                    }
                }
            }
            catch
            {
            }
        }

        static void RegeditSet(Action<RegistryKey> action)
        {
            using (RegistryKey hkeycu = Registry.CurrentUser)
            {
                action(hkeycu);
            }

            try
            {
                using (var hkeylm = Registry.LocalMachine)
                    action(hkeylm);
            }
            catch
            {
            }
        }
    }
}
