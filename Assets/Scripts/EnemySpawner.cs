using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject[] spawnHolder;

    public GameObject EnemyHolder;
    int spawnNumber;
    float ratio;
    public Cinemachine.CinemachineVirtualCamera vCam;

    public float MaxEnemyNumber = 5;
    public float minSpawnTimer = 3;
    public float maxSpawnTimer = 6;

    float currentSpawnTimer;
    float trueTimer = 0;

    // Start is called before the first frame update
    
    void Start()
    {
        ratio = Camera.main.aspect;
        spawnNumber = spawnHolder.Length;
        currentSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
    }

    void RePositionSpawnPoint()
    {
        int side = 1;
        for (int i = 0; i < spawnNumber; i++)
        {
            
            if (i < spawnNumber / 2)
                side = -1;
            else
                side = 1;
            spawnHolder[i].transform.position = (Vector2)vCam.transform.position + new Vector2(side * vCam.m_Lens.OrthographicSize * ratio + 5 * side, i%2 == 0? -vCam.m_Lens.OrthographicSize / 4 : -vCam.m_Lens.OrthographicSize / 1.5f);
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos = spawnHolder[Random.Range(0, spawnNumber)].transform.position;
        GameObject tmpGO = Instantiate(enemyPrefab, pos, this.transform.rotation, EnemyHolder.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RePositionSpawnPoint();
        trueTimer += Time.deltaTime;
        if (trueTimer > currentSpawnTimer && EnemyHolder.transform.childCount < MaxEnemyNumber)
        {
            trueTimer = 0;
            currentSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
            SpawnEnemy();
        }
    }
}
