using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics.Contracts;

using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;
using Solana.Unity.Rpc.Models;
using Nethereum.RPC.TransactionReceipts;

public class GameState : MonoBehaviour
{
    public GameObject player;

    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    private int time;
    public int score;


    public Solana.Unity.SDK.Web3 web3;
    public ContractHandler contractHandler;

    private void Start()
    {
        web3 = FindFirstObjectByType<Solana.Unity.SDK.Web3>();

        StartCoroutine(timer());

        //contractHandler.createOrLaunchGame("test");
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

        web3.Logout();
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IRpcClient rpcClient;
    private Wallet wallet;
}
