using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /* ==================== Fields ==================== */

    private static AudioManager _instance = null;
    private AudioSource[] _audios = new AudioSource[Constants.SOUND_CHANNEL];
    private byte _index = 0;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load("AudioManager")).GetComponent<AudioManager>();
                DontDestroyOnLoad(_instance.gameObject);
                for (byte i = 0; i < Constants.SOUND_CHANNEL; ++i)
                {
                    _instance._audios[i] = Instantiate(new GameObject(), _instance.transform).AddComponent<AudioSource>();
                    _instance.AudioSettings(i);
                }
            }
            return _instance;
        }
    }



    /* ==================== Public Methods ==================== */

    public void PlayAudio(AudioClip clip)
    {
        _audios[_index].clip = clip;
        _audios[_index].Play();
        ++_index;
        if (_index >= Constants.SOUND_CHANNEL)
        {
            _index = 0;
        }
    }



    /* ==================== Private Methods ==================== */

    private void AudioSettings(byte index)
    {
        _audios[index].playOnAwake = false;
    }
}
