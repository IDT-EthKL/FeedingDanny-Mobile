using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChainSelector : MonoBehaviour
{
    public Sprite Manta;
    public Sprite Scroll;

    public Button chainBtn;

    private int mantaChain = 3441006;
    private int scrollChain = 534351;

    public void Start()
    {
        if (PlayerPrefs.GetInt("chain", 3441006) == 534351)
        {
            chainBtn.image.sprite = Scroll;
        }
    }

    public void changeChain()
    {
        if (chainBtn.image.sprite == Manta)
        {
            chainBtn.image.sprite = Scroll;

            PlayerPrefs.SetInt("chain", scrollChain);
        }
        else
        {
            chainBtn.image.sprite = Manta;

            PlayerPrefs.SetInt("chain", mantaChain);
        }

        PlayerPrefs.Save();
    }
}
