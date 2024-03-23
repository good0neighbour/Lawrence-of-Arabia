using UnityEngine;

public class FourDirectionMovement
{
    /* ==================== Fields ==================== */

    private Rigidbody _rigidbody = null;
    private float _isFlipNum = 0.0f;



    /* ==================== Public Methods ==================== */

    public FourDirectionMovement(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }


    /// <summary>
    /// Sets local position of character
    /// </summary>
    /// <returns>Returns rotation of direction</returns>
    public float SetPosition(float weightX, float weightY)
    {
        _rigidbody.velocity = new Vector3(
            weightX * Constants.CHAR_VEL,
            0.0f,
            weightY * Constants.CHAR_VEL
        );

        if (weightX > 0.0f)
        {
            _isFlipNum = 0.0f;
        }
        else if (weightX < 0.0f)
        {
            _isFlipNum = -180.0f;
        }

        return _isFlipNum;
    }
}
