using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;

    public Transform PlayerTransform;
    public SpriteRenderer PlayerRenderer;

    public float playerSpeed = 1.5f;

    public float H;
    public float V;

    public float angle;

    void FixedUpdate()
    {
        H = joystick.Horizontal;
        V = joystick.Vertical;

        if (H != 0 && V != 0)
        {
            PlayerTransform.Translate(H * playerSpeed * Time.deltaTime, V * playerSpeed * Time.fixedDeltaTime, 0);

            angle = Mathf.Atan2(-H, V) * Mathf.Rad2Deg;

            if (angle > 0)
            {
                PlayerRenderer.flipX = true;
                PlayerRenderer.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
            else
            {
                PlayerRenderer.flipX = false;
                PlayerRenderer.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            }
        }
        else
        {
            PlayerRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
