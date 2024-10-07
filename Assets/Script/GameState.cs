using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics.Contracts;

public class GameState : MonoBehaviour
{
    public GameObject player;

    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    private int time;
    public int score;

    private void Start()
    {
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(1);
        time++;

        StartCoroutine(timer());
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        scoreText.text = "Score: " + score;
        timeText.text = "Survive: " + time + " seconds";
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
