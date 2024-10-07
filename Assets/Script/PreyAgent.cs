using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyAgent : MonoBehaviour
{
    public float speed = 5f;  // Speed of movement
    private Vector2 direction; // Current direction of movement
    private float changeDirectionTime;



    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;

    public SpriteRenderer objectSprite;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = objectSprite.bounds.extents.x; //extents = size of width / 2
        objectHeight = objectSprite.bounds.extents.y; //extents = size of height / 2

        StartCoroutine(ChangeDirection());
    }

    void FixedUpdate()
    {
        // Move the object
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;

        //direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    IEnumerator ChangeDirection()
    {
        // Generate a random direction
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        yield return new WaitForSeconds(Random.Range(2f, 10f));

        StartCoroutine(ChangeDirection());
    }
}
