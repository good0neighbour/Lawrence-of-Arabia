using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Lawrence of Arabia/GameData")]
public class GameData : ScriptableObject
{
    public MainFaction MainFactionStage = MainFaction.FirstStart;
}
