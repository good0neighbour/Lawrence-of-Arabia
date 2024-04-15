using System.Collections.Generic;
using UnityEngine;

public class Language
{
    /* ==================== Fields ==================== */

    private static Language _instance = null;
    private Dictionary<string, string> _texts = new Dictionary<string, string>();

    public static Language Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Language();
            }
            return _instance;
        }
    }

    public string this[string en]
    {
        get
        {
            switch (GameManager.GameData.CurrentLanguage)
            {
                case LanguageTypes.English:
                    return en;

                default:
#if UNITY_EDITOR
                    if (!_texts.ContainsKey(en))
                    {
                        Debug.LogError($"Language: Doesn't contain key: {en}");
                        return null;
                    }
#endif
                    return _texts[en];
            }
        }
    }



    /* ==================== Public Methods ==================== */



    /* ==================== Private Methods ==================== */



    /* ==================== Struct ==================== */

    public struct LanguageJson
    {
        public string[] Text;
    }
}
