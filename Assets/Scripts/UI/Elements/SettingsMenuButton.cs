using UnityEngine.UI;
using UnityEngine;
using CGD.Audio;

namespace CGD.Settings
{
    public class SettingsMenuButton : MenuButton
    {
        public SettingsMenu menu;
        public SettingsType settings;

        protected new void Awake()
        {
            base.Awake();

            GetComponent<Button>().onClick.AddListener(() =>
            {
                if(forward) 
                    menu.ApplySettings(settings);
                else
                    menu.RevertSettings(settings);
            });
        }

    }
}