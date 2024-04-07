public enum DialogueTypes
{
    Narration,
    Talk,
    Selection
}


public enum NameColours
{
    black,
    blue,
    green,
    orange,
    purple,
    red,
    white,
    yellow
}


public enum CharImageDir
{
    Left,
    Right
}


public enum TriggerTypes
{
    Enter,
    Exit,
    Interact
}


public enum ActionTypes
{
    Enable,
    Disable,
    Delete,
    PlayerTeleport,
    StartEventScene,
    LoadNextScene
}


public enum ConditionTypes
{
    Destroyed,
    Disabled,
    Enabled
}


public enum EvenSceneActions
{
    CameraMove,
    NPCMove,
    NPCJump,
    NPCLookAt,
    PlayerLookAt,
    Enable,
    Disable,
    Destroy,
    StartDialogue,
    CloseDialogue,
    LoadNextScene
}


public enum CutSceneActions
{
    FadeIn,
    FadeOut,
    Enable,
    Disable,
    Destroy,
    LoadScene,
    Wait
}

public enum FDInteractionTypes
{
    Enable,
    StartEventScene
}