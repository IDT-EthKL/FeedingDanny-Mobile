using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemy;
    public int spawnTime;

    public GameObject player;

    private void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        //(Random.Range(0, 2) * 2 - 1) positive or negative number
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

        //Random.Range(0.1f, 0.3f)

        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        v3Pos.z = 0;

        GameObject enemy = Instantiate(Enemy[Random.Range(0, Enemy.Length)], v3Pos, Quaternion.identity);
        enemy.GetComponent<EnemyAgent>().target = player.transform;
        enemy.GetComponent<collisionDetect>().gameState = this.gameObject.GetComponent<GameState>();

        enemy.transform.parent = this.gameObject.transform;

        yield return new WaitForSeconds(spawnTime);

        StartCoroutine(spawnEnemy());
    }
}
