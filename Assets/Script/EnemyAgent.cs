using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform target;

    public SpriteRenderer Enemy;
    public Sprite eat;
    public Sprite idle;

    private PreySpawner preySpawner;

    private void Awake()
    {
        preySpawner = FindFirstObjectByType<PreySpawner>();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (angle > 0)
        {
            Enemy.flipX = true;
            transform.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else
        {
            Enemy.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Enemy.sprite = eat;

            StartCoroutine(resetAnimation());
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Prey")
        {
            Destroy(collision.gameObject);
            Enemy.sprite = eat;

            preySpawner.count--;

            StartCoroutine(resetAnimation());
        }
    }

    IEnumerator resetAnimation()
    {
        yield return new WaitForSeconds(.5f);

        Enemy.sprite = idle;

        Debug.Log("Reset");
    }
}
