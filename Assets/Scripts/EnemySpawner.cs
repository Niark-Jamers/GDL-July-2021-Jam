using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject[] spawnHolder;

    public GameObject EnemyHolder;
    float spawnNumber;
    float ratio;
    public Cinemachine.CinemachineVirtualCamera vCam;

    public float MaxEnemyNumber = 5;
    public float minSpawnTimer = 3;
    public float maxSpawnTimer = 6;

    // Start is called before the first frame update
    
    void Start()
    {
        ratio = Camera.main.aspect;
        spawnNumber = spawnHolder.Length;
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

    // Update is called once per frame
    void Update()
    {
        RePositionSpawnPoint();
    }
}
