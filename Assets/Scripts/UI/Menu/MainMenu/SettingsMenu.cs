using UnityEngine;
using UnityEngine.UI;


namespace CGD.UI
{
    public enum SettingsType
    {
        All,
        Controls,
        Audio,
        Quality
    }

    public class SettingsMenu : MenuPanel
    {
        [Header("Input Settings")]
        [SerializeField] private Slider xSensitivity;
        [SerializeField] private Slider ySensitivity;
        [SerializeField] private Toggle invertY;

        public void InputPanelOpen()
        {
            var inputSettings = ItemCollection.Instance.InputSettings;
            xSensitivity.value = inputSettings.SensitivityX;
            xSensitivity.value = inputSettings.SensitivityY;
            invertY.isOn = inputSettings.InvertY == -1;
        }

        public void ApplySettings(SettingsType settings)
        {
            switch (settings)
            {
                case SettingsType.Controls:
                    ApplyInputSettings();
                    break;
            }
        }

        public void RevertSettings(SettingsType settings)
        {
            switch (settings)
            {
                case SettingsType.Controls:
                    RevertInputSettings();
                    break;
            }
        }

        private void ApplyInputSettings()
        {
            ItemCollection.Instance.InputSettings.ApplySettings(xSensitivity.value,
                ySensitivity.value, invertY.isOn);
        }

        private void RevertInputSettings()
        {
            var defaults = ItemCollection.Instance.DefaultInputSettings;
            
            xSensitivity.value = defaults.SensitivityX;
            ySensitivity.value = defaults.SensitivityX;
            invertY.isOn = defaults.InvertY == -1;
            
            
            ItemCollection.Instance.InputSettings.ApplySettings(defaults.SensitivityX,
                defaults.SensitivityY, defaults.InvertY == -1);
        }
    }
}