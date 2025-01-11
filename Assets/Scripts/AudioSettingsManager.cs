using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public const string AUDIO_SETTINGS_PREFIX= "AudioSettings";

    [Serializable] public struct MixerSlider
    {
        public Slider Slider;
        public string MixerParamenterName;
    }

    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] List<MixerSlider> _mixerSliders;
    [SerializeField] Vector2 _minMaxVolume = new(-80f, 20f);

    private void Awake()
    {
        foreach (var mixerSlider in _mixerSliders)
        {
            mixerSlider.Slider.minValue = _minMaxVolume.x;
            mixerSlider.Slider.maxValue = _minMaxVolume.y;
            mixerSlider.Slider.value = PlayerPrefs.GetFloat($"{AUDIO_SETTINGS_PREFIX}_{mixerSlider.MixerParamenterName}");
            mixerSlider.Slider.onValueChanged.AddListener(value => OnSliderValueChanged(mixerSlider, value));
        }
    }

    private void OnSliderValueChanged(MixerSlider mixerSlider, float value)
    {
        _audioMixer.SetFloat(mixerSlider.MixerParamenterName, value);
        PlayerPrefs.SetFloat($"{AUDIO_SETTINGS_PREFIX}_{mixerSlider.MixerParamenterName}", value);
        PlayerPrefs.Save();
    }
}