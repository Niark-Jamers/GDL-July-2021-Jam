using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteineSpawner : MonoBehaviour
{
    public GameObject proteinePrefab;
    public GameObject spawn;
    public GameObject proteineHolder;

    public Cinemachine.CinemachineVirtualCamera vCam;
    public float minSpawnTimer = 2;
    public float maxSpawnTimer = 4;
    float currentSpawnTimer;
    float trueTimer;
    float ratio;
    // Update is called once per frame
    private void Start()
    {

        ratio = Camera.main.aspect;
        currentSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
    }

    void RePositionSpawnPoint()
    {
        spawn.transform.position = (Vector2)vCam.transform.position + new Vector2(vCam.m_Lens.OrthographicSize * ratio + 5, -vCam.m_Lens.OrthographicSize / 2f);
    }

        void SpawnProteine()
    {
        Vector3 pos = spawn.transform.position + Vector3.down * Random.Range(-vCam.m_Lens.OrthographicSize / 2, vCam.m_Lens.OrthographicSize / 2);
        GameObject tmpGO = Instantiate(proteinePrefab, pos, this.transform.rotation, proteineHolder.transform);
    }

    private void Update()
    {
        RePositionSpawnPoint();
         trueTimer += Time.deltaTime;
         if (trueTimer > currentSpawnTimer)
        {
            trueTimer = 0;
            currentSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
            SpawnProteine();
        }
    }
}

