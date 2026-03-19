using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Level level;

    public AudioClip shootingClip;
    public AudioClip destructionClip;

    public GameObject playerObject;

    public Camera playerCamera;

    [Header("Prefabs")]
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;

    [Header("Up-To-Date Lists")]
    public List<Pawn> tanks;
    public List<Controller> players;
    public List<Controller_AI> ai;
    public List<PlayerSpawn> playerSpawnPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //check if there is an instance of the GameManager.
        //If there isn't one, make a new instance and tell the game to not destroy on load.
        //If there is one, destroy the one currently alive.
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Clear our up to date list objects (not just memory locations, but actual lists)
        tanks = new List<Pawn>();
        players = new List<Controller>();
        ai = new List<Controller_AI>();
        playerSpawnPoints = new List<PlayerSpawn>();
    }

    void Start()
    {


        //Start the game
        StartGame();
   
    }

    public void StartGame()
    {
        //Do everything to start game

        //generate map
        //level.mapGenerator.GenerateMap();

        //Spawn player
        SpawnPlayer();


    }

    public void SpawnPlayer()
    {
        Vector3 playerSpawnPosition;

        //choose a spawnpoint from the list
        if (playerSpawnPoints.Count > 0)
        {
            Debug.Log("Spawn point was chosen!");

            Transform spawnPoint = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].transform;

            playerSpawnPosition = spawnPoint.position;
        }
        else
        {
            Debug.Log("Spawm point was not chosen!");
            playerSpawnPosition = Vector3.zero;
        }

        //Spawn tank pawn (and store it in tanks)
        Pawn tempTankPawn = SpawnTank(playerPawnPrefab);

        //Spawn a player controller (and store it in players)
        Controller tempPlayerController = SpawnPlayerController(playerControllerPrefab);

        //Have player possess pawn
        tempPlayerController.Possess(tempTankPawn);

        // move to spawnpoint
        tempTankPawn.transform.position = playerSpawnPosition;

        
        SetPlayerObject(tempTankPawn.gameObject);

        SetCameraTarget(tempTankPawn.gameObject);
    }

    //set playerObject in gameManager
    public void SetPlayerObject(GameObject target)
    {
        //Pawn tempPawn = target.GetComponent<Pawn>();    

        playerObject = target;
    }

    //set the target for the playerCamera to the player pawn.
    public void SetCameraTarget(GameObject target)
    {
        playerCamera.GetComponent<CameraFollow>().target = target;
    } 

    public Pawn SpawnTank(GameObject prefab)
    {

        //Spawn tank pawn (and store it in tanks)
        GameObject tempTankObject = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
        return tempTankObject.GetComponent<Pawn>();

    }

    public Controller SpawnPlayerController (GameObject prefab)
    {

        GameObject tempPlayer = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
        return tempPlayer.GetComponent<Controller>();

    }

}
