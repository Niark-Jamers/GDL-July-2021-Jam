using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyPrefab;
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

    float[] powerTable;
    public Playerator playerScript;
    // Start is called before the first frame update
    GroundSlicer gc;
    void Start()
    {
        ratio = Camera.main.aspect;
        spawnNumber = spawnHolder.Length;
        currentSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
        gc = FindObjectOfType<GroundSlicer>();
        FillPowerTable();
    }

    void FillPowerTable()
    {
        powerTable = new float[enemyPrefab.Length];
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            powerTable[i] = enemyPrefab[i].GetComponent<EnemyController>().Level;
        }
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
        Vector2 tmp = (Vector2)vCam.transform.position + new Vector2(side * vCam.m_Lens.OrthographicSize * ratio + 5 * side, i%2 == 0? -vCam.m_Lens.OrthographicSize / 4 : -vCam.m_Lens.OrthographicSize / 1.5f);
        tmp.x = (tmp.x < gc.gLeft) ? gc.gLeft: tmp.x;
        tmp.y = (tmp.y > gc.gUp) ? gc.gUp: tmp.y;
            spawnHolder[i].transform.position = tmp;
        }
    }

    GameObject GetRandomEnemy()
    {
        int i = Random.Range(0, enemyPrefab.Length);
        float j = 100000;
        while (j >= playerScript.transform.localScale.x * 2)
        {
            i = Random.Range(0, enemyPrefab.Length);
            j = powerTable[i];
        }
        return enemyPrefab[i];
    }

    void SpawnEnemy()
    {
        Vector3 pos = spawnHolder[Random.Range(0, spawnNumber)].transform.position;
        GameObject tmpGO = Instantiate(GetRandomEnemy(), pos, this.transform.rotation, EnemyHolder.transform);
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
