using UnityEngine;

public class AudioManager
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
                _instance = new AudioManager();
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

    private AudioManager()
    {
        GameObject obj = new GameObject();
        Object.DontDestroyOnLoad(obj);
        for (byte i = 0; i < Constants.SOUND_CHANNEL; ++i)
        {
            _audios[i] = obj.AddComponent<AudioSource>();
            AudioSettings(i);
        }
#if UNITY_EDITOR
        obj.name = "AudioManager";
#endif
    }


    private void AudioSettings(byte index)
    {
        _audios[index].playOnAwake = false;
    }
}
