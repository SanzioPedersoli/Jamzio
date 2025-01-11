using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static AudioSettingsManager;

public class AudioSavedSettingsLoader : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] List<string> _mixerParamenterNames;
    private void Start() 
    {
        foreach (var name in _mixerParamenterNames)        
            _audioMixer.SetFloat(name, PlayerPrefs.GetFloat($"{AUDIO_SETTINGS_PREFIX}_{name}"));
    }
}
