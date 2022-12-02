using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Transform spawnLeftLimit;
    public Transform spawnRightLimit;

    public Transform enemyParent;
    private float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer>2)
        {
            var pos = new Vector3(Random.Range(spawnLeftLimit.position.x, spawnRightLimit.position.x), spawnRightLimit.position.y, spawnRightLimit.position.z);
            var temp = Instantiate(enemyPrefab, pos, quaternion.identity, enemyParent);
            temp.transform.forward = Vector3.forward * -1;
            timer = 0;
            EventManager.EnemySpawned(temp.GetComponent<EnemyController>());
        }
    }
}
