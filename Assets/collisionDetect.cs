using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class collisionDetect : MonoBehaviour
{
    public SpriteRenderer PlayerRenderer;
    public Sprite eat;
    public Sprite idle;

    public PreySpawner preySpawner;
    public GameState gameState;

    public int score;
    public TextMeshProUGUI scoreText;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Prey")
        {
            Destroy(collision.gameObject);
            PlayerRenderer.sprite = eat;

            preySpawner.count--;
            score++;

            gameState.score = score;

            scoreText.text = "x " + score.ToString();

            StartCoroutine(resetAnimation());
        }
    }

    IEnumerator resetAnimation()
    {
        yield return new WaitForSeconds(.5f);

        PlayerRenderer.sprite = idle;
    }
}
