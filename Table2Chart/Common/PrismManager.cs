using System.Diagnostics;

namespace Table2Chart.Common
{
    /// <summary>
    /// PrismManager 导航管理
    /// </summary>
    public class PrismManager
    {
        public static readonly string AppName = Process.GetCurrentProcess().ProcessName;
        public static readonly string MainViewRegionName = "MainViewRegion";
        public static readonly string SettingsRegionName = "SettingsRegion";

        //导航名字
        public static readonly string ExcelReaderView = "ExcelReaderView";

        public static readonly string PlotsView = "PlotsView";
        public static readonly string SettingsView = "SettingsView";
        public static readonly string SystemSettingView = "SystemSettingView";
    }
}