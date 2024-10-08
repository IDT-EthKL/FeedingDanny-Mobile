using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    public int time;
    public GameState gameState;
    public TextMeshProUGUI timer;


    void Start()
    {
        StartCoroutine(countdown());
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(1);

        time--;

        timer.text = time.ToString() + "s";

        if (time < 0)
        {
            Time.timeScale = time;

            gameState.GameOver();
        }
        else
        {
            StartCoroutine(countdown());
        }
    }
}
