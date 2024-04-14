using UnityEngine;

public class Constants
{
    // Player Settings
    public const float PLAYER_VEL = 4.0f;
    public const float PLAYER_JUMP_SPEED = 12.0f;
    public const float PLAYER_MAX_FALLING_VEL = -10.0f;
    public const float PLAYER_FEET_SIZE = 0.1f;
    public const float PLAYER_KNOCKBACK_AMOUNT = 2.0f;
    public const float PLAYER_KNOCKBACK_ACC = 6.0f;
    public const float PLAYER_INTERACTION_DISTANCE = 0.01f;
    public const float PLAYER_RADIUS = 0.5f;
    public const float PLAYER_IMMUNE_TIME = 3.0f;
    public const float PLAYER_DEATH_STANDBY_TIME = 3.0f;
    public static readonly Vector2 PLAYER_ATKEFT_POS = new Vector2(0.4f, 0.6f);

    // Enemy Settings
    public const float ENEMY_SILENCE_TIMER = 3.0f;
    public const float ENEMY_URGENT_TIMER = 0.5f;
    public const float ENEMY_MOVE_POSI = 0.1f;
    public const float ENEMY_JUMP_POSI = 0.03f;
    public const float ENEMY_MOVESTOP_POSI = 0.4f;
    public const float ENEMY_URGENT_ACC_MULT = 5.0f;
    public const float ENEMY_URGENT_MOVE_POSI = 0.6f;
    public const float ENEMY_URGENT_MOVESTOP_POSI = 0.2f;
    public const float ENEMY_URGENT_JUMP_POSI = 0.4f;
    public const float ENEMY_ANIM_MULT = 2.0f;
    public const float ENEMY_URGENT_SPEED = 3.0f;
    public const float ENEMY_SUSPICIOUS_TIME = 30.0f;
    public const float ENEMY_SUSPICIOUS_RANGE = 2.0f;

    // Camera Settings
    public const float HOR_CAM_SPEED = 0.03f;
    public const float FD_CAM_SPEED = 0.03f;
    public const float FD_CAM_ROT_OFFSET = -2.0F;
    public static readonly Vector2 HOR_CAM_OFFSET = new Vector2(1.0f, 1.0f);
    public static readonly Vector2 FD_CAM_OFFSET = new Vector2(6.0f, -10.0f);

    // Dialogue Settings
    public const sbyte TEXT_DISPLAY_NUM = 1;
    public const float TEXT_SPACING = 150.0f;
    public const float TEXT_SPEED = 300.0f;
    public const float TEXT_FADE_POW = 5.0f;
    public const float TEXT_OFFSET = -350.0f;
    public const float IMAGE_SPEED = 300.0f;
    public const float IMAGE_MOVE_DIS = 100.0f;
    public const float IMAGE_LEFT_POS_END = -500.0f;

    // Joystick Settings
    public const float JOYSTICK_RELEASING_SPEED = 600.0f;
    public const float JOYSTICK_CONTROL_SPEED = 10.0f;
    public const float JOYSTICK_JUMP_WEIGHT = 0.3f;

    // Play Controller settings
    public const float PLAYCON_SELECTED_CHAR_SCALE = 1.2f;

    // Player Button Settings
    public const byte BUTTON_ATTACK = 0;
    public const byte BUTTON_INTERACT = 1;
    public const byte BUTTON_EXTRA = 2;

    // Story Scene Settins
    public const float CUTSCN_FADEIN_SPEED = 1.0f;
    public const float CUTSCN_SKIP_SPEED = 0.75f;

    // Horizontal Stage Settings
    public const byte HS_MAX_CHARACTER = 3;

    // Stage Message Settings
    public const float STGMSG_SPEED = 2.0f;
    public const float STGMSG_SHOW_TIMER = 3.0f;

    // Player Weapon Settings
    public const ushort WEAPON_PISTOL_DAMAGE = 1;
    public const float WEAPON_PISTOL_RANGE = 3.0f;
    public const float WEAPON_PISTOL_TIME = 0.75f;

    // Character Settings
    public const byte CHAR_MID_TRUST = 40;
    public const byte CHAR_HIGH_TRUST = 80;

    // Audio Settings
    public const byte SOUND_CHANNEL = 4;

    // Physics
    public const float GRAVITY_ACCELERATION = -50.0f;
    public const float DELTA_TIME_LIMIT = 0.02f;

    // UI
    public const float CANVAS_WIDTH = 1920.0f;
    public const float CANVAS_HIGHT = 1080.0f;

    // Layer
    public const int LAYER_D_GROUND = 3;
    public const int LAYER_D_WALL = 6;
    public const int LAYER_D_PLAYER = 7;
    public const int LAYER_D_ENEMY = 8;

    // Character Movement State
    public const byte MOVE_LEFT_START = 0;
    public const byte MOVE_LEFT_END = 1;
    public const byte MOVE_RIGHT_START = 2;
    public const byte MOVE_RIGHT_END = 3;

    // Character Jump State
    public const byte JUMP_NONE = 0;
    public const byte JUMP_UP = 1;
    public const byte JUMP_DOWN = 2;

    // Joystick Control State
    public const byte JOYSTICK_STANDINGBY = 0;
    public const byte JOYSTICK_DRAGGING = 1;
    public const byte JOYSTICK_KEYBOARD = 2;
    public const byte JOYSTICK_GAMEPAD = 3;
    public const byte JOYSTICK_UNAVAILABLE = 4;

    // Enemy State
    public const byte ENEMY_SILENCE = 0;
    public const byte ENEMY_SUSPICIOUS = 1;
    public const byte ENEMY_URGENT = 2;
    public const byte ENEMY_ATTACK = 3;

    // Dialogue State
    public const sbyte DIALOGUE_TEXT_MOVING = 0b01;
    public const sbyte DIALOGUE_TEXT_END = ~DIALOGUE_TEXT_MOVING;
    public const sbyte DIALOGUE_IMAGE_MOVING = 0b10;
    public const sbyte DIALOGUE_IMAGE_END = ~DIALOGUE_IMAGE_MOVING;

    // Cut Scene State
    public const byte CUTSCENE_FADEIN = 0;
    public const byte CUTSCENE_ACTION = 1;
    public const byte CUTSCENE_FADEOUT = 2;

    // Behaviour Tree
    public const byte SUCCESS = 1;
    public const byte FAILURE = 0;

    // Precalculation
    public const int LAYER_B_PLATFORM = 1 << LAYER_D_GROUND;
    public const int LAYER_B_WALL = 1 << LAYER_D_WALL;
    public const int LAYER_B_PLAYER = 1 << LAYER_D_PLAYER;
    public const int LAYER_B_ENEMY = 1 << LAYER_D_ENEMY;
    public const int LAYER_B_TERRAIN = LAYER_B_PLATFORM | LAYER_B_WALL;
    public const float PRECAL_TEXT_ALPHA = TEXT_SPACING - TEXT_OFFSET;
    public const float IMAGE_LEFT_POS_START = IMAGE_LEFT_POS_END - IMAGE_MOVE_DIS;
    public const float IMAGE_RIGHT_POS_END = -IMAGE_LEFT_POS_END;
    public const float IMAGE_RIGHT_POS_START = -IMAGE_LEFT_POS_START;
    public const float ENEMY_MOVE_POSI_DOUBLE = ENEMY_MOVE_POSI * 2.0f;
    public const float ENEMY_JUMP_UP_POSI = ENEMY_MOVE_POSI + ENEMY_MOVE_POSI_DOUBLE + ENEMY_JUMP_POSI;
    public const float ENEMY_JUMP_DOWN_POSI = ENEMY_JUMP_UP_POSI + ENEMY_JUMP_POSI;
    public const float ENEMY_URGENT_JUMP_POSI_ACTUAL = ENEMY_URGENT_MOVE_POSI + ENEMY_URGENT_JUMP_POSI;
}
