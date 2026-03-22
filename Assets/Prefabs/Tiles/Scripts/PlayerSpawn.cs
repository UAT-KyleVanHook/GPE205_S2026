using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        // This needs to be in awake as the tile map is not already made on start.
        GameManager.instance.playerSpawnPoints.Add(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //add to PlayerSpawn list
        //GameManager.instance.playerSpawnPoints.Add(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        //remove from PlayerSpawn list
        GameManager.instance.playerSpawnPoints.Remove(this);

    }
}
