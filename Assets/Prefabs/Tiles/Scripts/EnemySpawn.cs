using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //[HideInInspector]
    public GameObject spawnedEnemy;

    private void Awake()
    {
        // This needs to be in awake as the tile map is not already made on start.
        GameManager.instance.enemySpawnPoints.Add(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        //remove from PlayerSpawn list
        GameManager.instance.enemySpawnPoints.Remove(this);

    }

    //set what the spawned enemy is to keep track of
    public void SetSpawnedEnemy(GameObject prefab)
    {
        spawnedEnemy = prefab;
    }

    //bool to check if this spawn point has spawned an enemy
    public bool IsSpawnedEnemy()
    {
        if (spawnedEnemy != null)
        {
            return true;
        }

        return false;
    }
}
