using Nethereum.Contracts.ContractHandlers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PreySpawner : MonoBehaviour
{
    public GameObject[] Prey;
    public int spawnTime;
    //public int max;
    //public int count;

    public Camera cam;

    public ContractHandler contractHandler;

    private void Start()
    {
        StartCoroutine(spawnEnemy());

        if (Prey.Length == 0) throw new Exception("Invalid Array Length");
    }

    IEnumerator spawnEnemy()
    {
        float x;
        float y;

        while (true)
        {
            x = Random.Range(-0.05f, 1.05f);
            y = Random.Range(-0.05f, 1.05f);

            //if (x < -.05)
            //{
            //    x = -0.05f;
            //}

            //if (y < -.05)
            //{
            //    y = .05f;
            //}

            Debug.Log(x + ", " + y);

            if (x < 0) { break; }
            else if (x > 1) { break; }
            else if (y < 0) { break; }
            else if (y > 1) { break; }

            //break;
        }

        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        v3Pos.z = 0;

        GameObject prey = Instantiate(Prey[Random.Range(0, Prey.Length)], v3Pos, Quaternion.identity);
        prey.GetComponent<PreyAgent>().MainCamera = cam;

        //contractHandler.spawnFish();

        //count++;
        //}

        yield return new WaitForSeconds(spawnTime);

        StartCoroutine(spawnEnemy());
    }
}
