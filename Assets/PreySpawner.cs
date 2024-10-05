using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreySpawner : MonoBehaviour
{
    public GameObject Prey;
    public int spawnTime;
    public int max;
    public int count;

    public Camera cam;

    private void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        if (count < max)
        {
            int x1 = Random.Range(0, 2) == 0 ? -1 : 1;
            int x2 = Random.Range(0, 2) == 0 ? -1 : 1;

            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1, 2f) * x1, Random.Range(1, 2f) * x2, 0));
            v3Pos.z = 0;

            GameObject prey = Instantiate(Prey, v3Pos, Quaternion.identity);
            prey.GetComponent<PreyAgent>().MainCamera = cam;

            count++;
        }

        yield return new WaitForSeconds(spawnTime);

        StartCoroutine(spawnEnemy());
    }
}
