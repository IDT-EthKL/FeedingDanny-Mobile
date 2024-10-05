using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public int spawnTime;

    public GameObject player;

    private void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        int x1 = Random.Range(0, 2) == 0 ? -1 : 1;
        int x2 = Random.Range(0, 2) == 0 ? -1 : 1;

        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1, 2f) * x1, Random.Range(1, 2f) * x2, 0));
        v3Pos.z = 0;

        GameObject enemy = Instantiate(Enemy, v3Pos, Quaternion.identity);
        enemy.GetComponent<EnemyAgent>().target = player.transform;

        yield return new WaitForSeconds(spawnTime);

        StartCoroutine(spawnEnemy());
    }
}
