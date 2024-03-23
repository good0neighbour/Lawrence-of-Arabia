using UnityEngine;

public class Constants
{
    // Character Settings
    public const float CHAR_VEL = 4.0f;
    public const float CHAR_JUMP_SPEED = 12.0f;
    public const float CHAR_MAX_FALLING_VEL = -12.0f;
    public const float CHAR_FEET_SIZE = 0.1f;

    // Enemy Settings
    public const float ENEMY_SILENCE_TIMER = 5.0f;
    public const float ENEMY_URGENT_TIMER = 1.0f;
    public const float ENEMY_MOVE_POSI = 0.1f;
    public const float ENEMY_JUMP_POSI = 0.03f;
    public const float ENEMY_MOVESTOP_POSI = 0.4f;
    public const float ENEMY_URGENT_ACC_MULT = 5.0f;
    public const float ENEMY_URGENT_MOVE_POSI = 0.6f;
    public const float ENEMY_URGENT_MOVESTOP_POSI = 0.4f;
    public const float ENEMY_URGENT_JUMP_POSI = 0.3f;
    public const float ENEMY_ANIM_MULT = 2.0f;

    // Camera Settings
    public const float HOR_CAM_SPEED = 0.03f;
    public const float FD_CAM_SPEED = 0.3f;
    public const float FD_CAM_ROT_OFFSET = -2.0F;
    public static readonly Vector2 HOR_CAM_OFFSET = new Vector2(1.0f, 1.0f);
    public static readonly Vector2 FD_CAM_OFFSET = new Vector2(6.0f, -10.0f);

    // Dialogue Settings
    public const sbyte TEXT_DISPLAY_NUM = 2;
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

    // Player Button Setting
    public const byte BUTTON_ATTACK = 0;
    public const byte BUTTON_INTERACT = 1;
    public const byte BUTTON_EXTRA = 2;

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
    public const byte ENEMY_URGENT = 1;
    public const byte ENEMY_ATTACK = 2;

    // Dialogue State
    public const sbyte DIALOGUE_TEXT_MOVING = 0b01;
    public const sbyte DIALOGUE_TEXT_END = ~DIALOGUE_TEXT_MOVING;
    public const sbyte DIALOGUE_IMAGE_MOVING = 0b10;
    public const sbyte DIALOGUE_IMAGE_END = ~DIALOGUE_IMAGE_MOVING;

    // Behaviour Tree
    public const byte SUCCESS = 1;
    public const byte FAILURE = 0;

    // Precalculation
    public const int LAYER_B_GROUND = 1 << LAYER_D_GROUND;
    public const int LAYER_B_WALL = 1 << LAYER_D_WALL;
    public const int LAYER_B_PLAYER = 1 << LAYER_D_PLAYER;
    public const int LAYER_B_TERRAIN = LAYER_B_GROUND | LAYER_B_WALL;
    public const float PRECAL_TEXT_ALPHA = TEXT_SPACING - TEXT_OFFSET;
    public const float IMAGE_LEFT_POS_START = IMAGE_LEFT_POS_END - IMAGE_MOVE_DIS;
    public const float IMAGE_RIGHT_POS_END = -IMAGE_LEFT_POS_END;
    public const float IMAGE_RIGHT_POS_START = -IMAGE_LEFT_POS_START;
    public const float ENEMY_MOVE_POSI_DOUBLE = ENEMY_MOVE_POSI * 2.0f;
    public const float ENEMY_JUMP_UP_POSI = ENEMY_MOVE_POSI + ENEMY_MOVE_POSI_DOUBLE + ENEMY_JUMP_POSI;
    public const float ENEMY_JUMP_DOWN_POSI = ENEMY_JUMP_UP_POSI + ENEMY_JUMP_POSI;
    public const float ENEMY_URGENT_JUMP_POSI_ACTUAL = ENEMY_URGENT_MOVE_POSI + ENEMY_URGENT_JUMP_POSI;
}
