using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Lawrence of Arabia/GameData")]
public class GameData : ScriptableObject
{
    public LanguageTypes CurrentLanguage;
    public MainFaction MainFaction;
    public string CurrentHeist;
    public byte CurrentPreperation;
}
