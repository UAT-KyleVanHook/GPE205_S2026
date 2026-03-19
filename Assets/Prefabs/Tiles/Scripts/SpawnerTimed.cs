using UnityEngine;

public class SpawnerTimed : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeBetweenSpawns;
    public bool bIsSpawnOnStart;
    private float countdownTimer;
    private GameObject spawnedObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //chekck if the object spawns on start
        if(bIsSpawnOnStart)
        {
            countdownTimer = 0;
        }
        else
        {
            countdownTimer = timeBetweenSpawns;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //check if the object is spawned
        if (spawnedObject == null)
        {
            countdownTimer -= Time.deltaTime;


            //every frame deincrement timer
            countdownTimer -= Time.deltaTime;

            if (countdownTimer <= 0)
            {
                //spawn object
                spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation) as GameObject;

                //reset timer
                countdownTimer = timeBetweenSpawns;
            }

        }
        
    }
}
