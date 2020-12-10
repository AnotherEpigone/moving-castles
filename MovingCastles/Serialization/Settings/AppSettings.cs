using IniParser;
using IniParser.Model;

namespace MovingCastles.Serialization.Settings
{
    public class AppSettings : IAppSettings
    {
        private const string SettingsIniKey = "settings";
        private const string FullScreenSettingKey = "fullscreen";
        private const string DebugPasswordSettingKey = "debug-pw";
        private const string DebugPassword = "In principio erat verbum";

        private const string ConfigPath = "config.ini";

        private bool _fullScreen;
        private bool _debug;

        public AppSettings()
        {
            var iniParser = new FileIniDataParser();
            var data = iniParser.ReadFile(ConfigPath);
            var settings = data[SettingsIniKey];

            if (settings.ContainsKey(FullScreenSettingKey))
            {
                _fullScreen = bool.Parse(settings[FullScreenSettingKey]);
            }

            if (settings.ContainsKey(DebugPasswordSettingKey))
            {
                _debug = settings[DebugPasswordSettingKey] == DebugPassword;
            }
        }

        public bool FullScreen
        {
            get { return _fullScreen; }
            set
            {
                _fullScreen = value;
                Write();
            }
        }

        public bool Debug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                Write();
            }
        }

        private void Write()
        {
            var data = new IniData();
            data[SettingsIniKey][FullScreenSettingKey] = _fullScreen.ToString();

            if (_debug)
            {
                data[SettingsIniKey][DebugPasswordSettingKey] = DebugPassword;
            }

            var iniParser = new FileIniDataParser();
            iniParser.WriteFile(ConfigPath, data);
        }
    }
}
