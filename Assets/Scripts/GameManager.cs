using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Prefabs")]
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;

    [Header("Up-To-Date Lists")]
    public List<Pawn> tanks;
    public List<Controller> players;


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

    }

    void Start()
    {
        //Start the game
        StartGame();
   
    }

    public void StartGame()
    {
        //Do everything to start game

        //Spawn player
        SpawnPlayer();


    }

    public void SpawnPlayer()
    {

        //Spawn tank pawn (and store it in tanks)

        Pawn tempTankPawn = SpawnTank(playerPawnPrefab);

        //Spawn a player controller (and store it in players)
        Controller tempPlayerController = SpawnPlayerController(playerControllerPrefab);

        //Have player possess pawn
        tempPlayerController.Possess(tempTankPawn);

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
