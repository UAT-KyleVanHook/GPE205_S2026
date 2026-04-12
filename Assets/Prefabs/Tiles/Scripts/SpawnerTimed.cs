using UnityEngine;

public class SpawnerTimed : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeBetweenSpawns;
    public bool bIsSpawnOnStart;
    private float countdownTimer;
    private GameObject spawnedObject;

    [Header ("Audio")]
    private AudioSource audioSource;
    public AudioClip powerUpClip;

    void Awake()
    {
        GameManager.instance.powerUpSpawners.Add(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        //chekck if the object spawns on start
        if (bIsSpawnOnStart)
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
        //check if there is an object inserted into the objectToSpawn. If not null, allow spawning.
        if (objectToSpawn != null)
        {
            //check if the object is spawned
            if (spawnedObject == null)
            {

                //check if audioclip isn't null
                if (powerUpClip != null && countdownTimer >= timeBetweenSpawns)
                {

                    audioSource.PlayOneShot(powerUpClip);
                }

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
}
