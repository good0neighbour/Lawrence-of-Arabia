using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Lawrence of Arabia/GameData")]
public class GameData : ScriptableObject
{
    [Header("Game Settings")]
    public LanguageTypes CurrentLanguage;

    [Header("Factions")]
    public MainFaction MainFaction;

    [Header("Progress")]
    public string CurrentHeist;
    public byte CurrentPreperation;

    [Header("Items")]
    public ushort LevelAvailable;
}
