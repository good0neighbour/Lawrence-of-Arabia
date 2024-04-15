using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutScene", menuName = "Lawrence of Arabia/CutScene")]
public class CutScene : ScriptableObject
{
    /* ==================== Fields ==================== */

    [SerializeField] private CutSceneInfo[] _actions = new CutSceneInfo[0];
    [SerializeField] private LanguageTypes _currentLanguage = LanguageTypes.English;
    [SerializeField] private string _nextScene = null;

#if UNITY_EDITOR
    public LanguageTypes CurrentLanguage
    {
        get
        {
            return _currentLanguage;
        }
        set
        {
            _currentLanguage = value;
        }
    }

    public string NextScene
    {
        get
        {
            return _nextScene;
        }
        set
        {
            _nextScene = value;
        }
    }
#endif



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// Fetchs cut scene actions and set language dictionary.
    /// </summary>
    public CutSceneInfo[] GetCutSceneActions()
    {
        if (_currentLanguage != GameManager.GameData.CurrentLanguage)
        {
            SetLanguage(GameManager.GameData.CurrentLanguage);
        }
        return _actions;
    }


    public string GetNextSceneName()
    {
        return _nextScene;
    }


#if UNITY_EDITOR
    public List<CutSceneInfo> GetActionsForEditor()
    {
        List<CutSceneInfo> result = new List<CutSceneInfo>();
        foreach (CutSceneInfo action in _actions)
        {
            result.Add(action);
        }
        return result;
    }


    public void SetActions(CutSceneInfo[] actions)
    {
        _actions = actions;
    }
#endif



    /* ==================== Private Methods ==================== */

    private void SetLanguage(LanguageTypes language)
    {
        Language.LanguageJson json = JsonUtility.FromJson<Language.LanguageJson>(Resources.Load($"Languages/{name}_{language.ToString()}").ToString());
        _currentLanguage = language;
        for (byte i = 0; i < _actions.Length; ++i)
        {
            _actions[i].Text = json.Text[i];
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct CutSceneInfo
    {
        public Sprite Image;
        public string Text;
        public AudioClip Audio;
        public float Duration;
    }
}
