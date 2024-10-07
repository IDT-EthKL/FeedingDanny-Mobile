using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class collisionDetect : MonoBehaviour
{
    private SpriteRenderer objectRenderer;
    public Sprite eat;
    public Sprite idle;

    public string[] EdiblePreyTag;

    //public PreySpawner preySpawner;
    public GameState gameState;

    public int score;
    public TextMeshProUGUI scoreText;


    //private ThirdwebChainData _chainDetails;
    private BigInteger ActiveChainId; //Manta
    private string mantaContractAddress = "0x2B47266fBBcC6BeA15C307DFcd5b2233e4275A18";
    private string scrollContractAddress = "0x6696283e07ce0619f6d88626a77a41978517dd1f";



    private void Start()
    {
        objectRenderer = GetComponent<SpriteRenderer>();

        //try
        //{
        //    _chainDetails = await Utils.GetChainMetadata(client: ThirdwebManager.Instance.Client, chainId: ActiveChainId);
        //}
        //catch
        //{
        //    _chainDetails = new ThirdwebChainData()
        //    {
        //        NativeCurrency = new ThirdwebChainNativeCurrency()
        //        {
        //            Decimals = 18,
        //            Name = "ETH",
        //            Symbol = "ETH"
        //        }
        //    };
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == gameState.player)
        {
            objectRenderer.sprite = eat;

            StartCoroutine(resetAnimation());

            Time.timeScale = 0;

            collision.gameObject.SetActive(false);
            gameState.GameOver();
        }
        else if (EdiblePreyTag.Contains(collision.gameObject.tag) && this.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            objectRenderer.sprite = eat;

            //preySpawner.count--;
            score++;

            gameState.score = score;

            eatFishFunction();

            scoreText.text = "x " + score.ToString();

            StartCoroutine(resetAnimation());
        }
        else if (EdiblePreyTag.Contains(collision.gameObject.tag))
        {
            Destroy(collision.gameObject);
            objectRenderer.sprite = eat;

            //preySpawner.count--;

            StartCoroutine(resetAnimation());
        }
    }

    async void eatFishFunction()
    {
        ActiveChainId = PlayerPrefs.GetInt("chain", 3441006);

        Debug.Log("Chain: " + ActiveChainId);

        var wallet = ThirdwebManager.Instance.GetActiveWallet();
        BigInteger weiValue = BigInteger.Zero;

        var contract = await ThirdwebManager.Instance.GetContract(address: ActiveChainId == 3441006 ? mantaContractAddress : scrollContractAddress, chainId: ActiveChainId, "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_rewardToken\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_fishNFT\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_hyperlaneMailbox\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_igp\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"OwnableInvalidOwner\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"OwnableUnauthorizedAccount\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"uint32\",\"name\":\"originChain\",\"type\":\"uint32\"},{\"indexed\":false,\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"fishId\",\"type\":\"uint256\"}],\"name\":\"CrossChainInteraction\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"fishSize\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"reward\",\"type\":\"uint256\"}],\"name\":\"FishEaten\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"newSize\",\"type\":\"uint256\"}],\"name\":\"PlayerGrown\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"BASE_FISH_COST\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"BASE_REWARD\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"destinationChain\",\"type\":\"uint32\"},{\"internalType\":\"uint256\",\"name\":\"fishId\",\"type\":\"uint256\"}],\"name\":\"crossChainEat\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"eatFish\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"fishNFT\",\"outputs\":[{\"internalType\":\"contract IFishGameNFT\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"_origin\",\"type\":\"uint32\"},{\"internalType\":\"bytes32\",\"name\":\"_sender\",\"type\":\"bytes32\"},{\"internalType\":\"bytes\",\"name\":\"_body\",\"type\":\"bytes\"}],\"name\":\"handle\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"hyperlaneMailbox\",\"outputs\":[{\"internalType\":\"contract IMailbox\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"igp\",\"outputs\":[{\"internalType\":\"contract IInterchainGasPaymaster\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerStates\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"playerScore\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"fishSize\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"lastInteractionTime\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"rewardToken\",\"outputs\":[{\"internalType\":\"contract IERC20Reward\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"_domain\",\"type\":\"uint32\"},{\"internalType\":\"bytes32\",\"name\":\"_trustedRemote\",\"type\":\"bytes32\"}],\"name\":\"setTrustedRemote\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint32\",\"name\":\"\",\"type\":\"uint32\"}],\"name\":\"trustedRemotes\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"tokenAddress\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawERC20\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]");

        await ThirdwebContract.Write(wallet, contract, "eatFish", weiValue);

        Debug.Log("Done minted");
    }

    IEnumerator resetAnimation()
    {
        yield return new WaitForSeconds(.5f);

        objectRenderer.sprite = idle;
    }
}
