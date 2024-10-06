using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using Thirdweb;
using Thirdweb.Unity;
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


    private ThirdwebChainData _chainDetails;
    public BigInteger ActiveChainId; //Manta
    public string mantaContractAddress = "0x2B47266fBBcC6BeA15C307DFcd5b2233e4275A18";
    public string scrollContractAddress = "0xDE6A35183197A6Fc0b27a18A9d54D1AD26f53f40";

    private async void Start()
    {
        try
        {
            _chainDetails = await Utils.GetChainMetadata(client: ThirdwebManager.Instance.Client, chainId: ActiveChainId);
        }
        catch
        {
            _chainDetails = new ThirdwebChainData()
            {
                NativeCurrency = new ThirdwebChainNativeCurrency()
                {
                    Decimals = 18,
                    Name = "ETH",
                    Symbol = "ETH"
                }
            };
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Prey")
        {
            Destroy(collision.gameObject);
            PlayerRenderer.sprite = eat;

            preySpawner.count--;
            score++;

            gameState.score = score;

            eatFishFunction();

            scoreText.text = "x " + score.ToString();

            StartCoroutine(resetAnimation());
        }
    }

    async void eatFishFunction()
    {
        ActiveChainId = PlayerPrefs.GetInt("chain", 3441006);

        Debug.Log("Chain: " + ActiveChainId);

        var contract = await ThirdwebManager.Instance.GetContract(address: ActiveChainId == 3441006 ? mantaContractAddress : scrollContractAddress, chainId: ActiveChainId);

        var wallet = ThirdwebManager.Instance.GetActiveWallet();
        BigInteger weiValue = BigInteger.Zero;

        await ThirdwebContract.Write(wallet, contract, "eatFish", weiValue);

        Debug.Log("Done minted");
    }

    IEnumerator resetAnimation()
    {
        yield return new WaitForSeconds(.5f);

        PlayerRenderer.sprite = idle;
    }
}
