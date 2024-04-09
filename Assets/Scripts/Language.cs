using System.Collections.Generic;

public class Language
{
    /* ==================== Fields ==================== */

    private static Language _instance = null;

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



    /* ==================== Public Methods ==================== */



    /* ==================== Private Methods ==================== */
}
