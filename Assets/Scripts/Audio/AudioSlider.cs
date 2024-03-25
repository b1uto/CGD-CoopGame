using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace CGD.Audio
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup mixerGroup;

        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(SetExposedParameter);
        }

        private void OnEnable()
        {
            slider.value = PlayerPrefs.GetFloat(mixerGroup.name, 1f);
        }

        /// <summary>
        /// Sets the value of the parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetExposedParameter(float value)
        {
            if (value == 0f) return;

            PlayerPrefs.SetFloat(mixerGroup.name, value);
            PlayerPrefs.Save();

            mixerGroup.audioMixer.SetFloat(mixerGroup.name, Mathf.Log10(value) * 20);
        }
    }
}