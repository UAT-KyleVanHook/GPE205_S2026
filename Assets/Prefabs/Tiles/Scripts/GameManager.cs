using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Level level;

    public float playerLives;
    private bool bGameOver = false;

    public GameObject playerObject;
    public Controller playerController;

    public Camera playerCamera;

    [Header("Audio Clips")]
    public AudioClip shootingClip;
    public AudioClip destructionClip;

    [Header("Prefabs")]
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;

    [Header("Up-To-Date Lists")]
    public List<Pawn> tanks;
    public List<Controller> players;
    public List<Controller_AI> ai;
    public List<PlayerSpawn> playerSpawnPoints;
    public List<EnemySpawn> enemySpawnPoints;
    public List<SpawnerTimed> powerUpSpawners;

    [Header("Enemy Type List: Add Enemies to Spawn")]
    public int enemySpawnCount;
    private int startingSpawnCount = 0;
    public List<GameObject> enemies;

    [Header("Enemy AI Controller List: Make sure that the AI Controllers are in the same order as the enemy list above.")]
    public List<GameObject> enemiesAIController;

    [Header("PowerUp List: Add powerups to Spawn")]
    public List<GameObject> powerups;
    public int healthPickupAmount;
    public int healthMaxPickupAmount;
    public int moveSpeedPickupAmount;

    public float heatlhPickupSpawnTime;
    public float heatlhMaxPickupSpawnTime;
    public float moveSpeedPickupSpawnTime;

    private int powerupTotal;
    //used to track how many loops are left
    private int powerupTotalCount;

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
        enemySpawnPoints = new List<EnemySpawn>();
        powerUpSpawners = new List<SpawnerTimed>();

        //enemies = new List<GameObject>(); 

        //total amount of powerups combined together
        powerupTotal = healthPickupAmount + healthMaxPickupAmount + moveSpeedPickupAmount;
        //set powerupTotalCount to the same as powerupTotal. We will use powerupTotalCount to deincrement.
        powerupTotalCount = powerupTotal;
    }

    void Start()
    {


        //Start the game
        StartGame();
   
    }

    void Update()
    {
        //check if the player is dead and if the lives counter is larger than 0.
        //if true, respawn the player.
        if (playerLives > 0 && playerObject == null)
        {
            RespawnPlayer();
        }

        //if the playerLives if less than or equal to zero, and the game over bool is false, then display "Game OVer!" and flip bool to true.
        if(playerLives <= 0 && bGameOver == false)
        {
            Debug.Log("Game Over!!!");

            bGameOver = true;
        }

        //Debug.Log(PickUpHealth.count);
        
    }

    public void StartGame()
    {
        //Do everything to start game

        //generate map
        level.mapGenerator.GenerateMap();

        //Spawn player
        SpawnPlayer();

        //spawn an enemy for the designated enemySpawnCount amount
        do
        {

            //spawn enemy
            SpawnEnemy();

            //increment startingSpawnCount
            startingSpawnCount++;

        } while (startingSpawnCount < enemySpawnCount);

        Debug.Log(PickUpHealth.count);
        Debug.Log(PickUpMaxHealthUp.count);
        Debug.Log(PickUpMoveSpeed.count);


        //check that the total amount of powerups to spawn is less than or equal to the total amoiunt of spawners.
        if (powerupTotal <= powerUpSpawners.Count)
        {
            //set the objects for the powerups spawners
            SetPowerUp();
        }
        else
        {
            Debug.Log("Amount of desired powerups is more than the total amount of spawners avaialable.");
        }

        Debug.Log(PickUpHealth.count);
        Debug.Log(PickUpMaxHealthUp.count);
        Debug.Log(PickUpMoveSpeed.count);
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

        //set the player contoller as the main controller to remember
        playerController = tempPlayerController;    

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
        playerCamera = Camera.main;
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

    //respawn player 

    public void RespawnPlayer()
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

        //Have player possess pawn
        playerController.Possess(tempTankPawn);

        // move to spawnpoint
        tempTankPawn.transform.position = playerSpawnPosition;


        SetPlayerObject(tempTankPawn.gameObject);

    }


    //spawn enemies

    public void SpawnEnemy()
    {

        Vector3 enemySpawnPosition;

        //choose a spawnpoint from the list
        if (enemySpawnPoints.Count > 0)
        {
            Debug.Log("Enemy Spawn point was chosen!");

            //get spawn point
            EnemySpawn spawnPoint;

            //check if this enemySpawnPoint has already spawned an object
            do
            {
                //set randomly selected spawn point to this enemyspawn variable
                spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)];

            } while (spawnPoint.IsSpawnedEnemy() == true);

            //set this spanw points transform to a transform variable
            Transform spawnPointTransform = spawnPoint.transform;

            enemySpawnPosition = spawnPointTransform.position;

            //keep track of the index of the enemy 
            int enemyIndex = Random.Range(0, enemies.Count);

            //get a random enemy prfab from list using enemyIndex.
            GameObject tempEnemyObject = enemies[enemyIndex];

            //Spawn tank pawn (and store it in tanks)
            Pawn tempEnemyTankPawn = SpawnEnemyTank(tempEnemyObject);

            //set the pawn tank as the object to be traceked whne spawned.
            spawnPoint.SetSpawnedEnemy(tempEnemyObject);

            //create temp enemy controller
            Controller_AI tempEnemyController;

            //switch case to figure out which controller to assign to the spawned tank
            // MAKE SURE THAT THE ENEMY LIST AND THE AI CONTROLLER ARE IN THE SAME ORDER!!!
            switch (enemyIndex)
            {
                //flee AI
                case 0:

                    //set controller for the enemy tank
                    tempEnemyController = SpawnEnemyController(enemiesAIController[enemyIndex]);

                    //Have player possess pawn
                    tempEnemyController.Possess(tempEnemyTankPawn);

                    break;

                //Kamikaze AI
                case 1:

                    //set controller for the enemy tank
                    tempEnemyController = SpawnEnemyController(enemiesAIController[enemyIndex]);

                    //Have player possess pawn
                    tempEnemyController.Possess(tempEnemyTankPawn);

                    break;

                //Pursuer AI
                case 2:

                    //set controller for the enemy tank
                    tempEnemyController = SpawnEnemyController(enemiesAIController[enemyIndex]);

                    //Have player possess pawn
                    tempEnemyController.Possess(tempEnemyTankPawn);

                    break;

                //Semtry AI
                case 3:

                    //set controller for the enemy tank
                    tempEnemyController = SpawnEnemyController(enemiesAIController[enemyIndex]);

                    //Have player possess pawn
                    tempEnemyController.Possess(tempEnemyTankPawn);

                    break;



            }




            // move to spawnpoint
            tempEnemyTankPawn.transform.position = enemySpawnPosition;
        }
        else
        {
            Debug.Log("Enemy Spawn point was not chosen!");
            enemySpawnPosition = Vector3.zero;


        }

        //get a random enemy prfab from list
        //GameObject tempEnemyObject = enemies[Random.Range(0, enemies.Count)];

        //Spawn tank pawn (and store it in tanks)
        //Pawn tempEnemyTankPawn = SpawnEnemyTank(tempEnemyObject);

        //spawnPoint.SetSpawnedEnemy()

        // move to spawnpoint
        //tempEnemyTankPawn.transform.position = enemySpawnPosition;

    }

    public Pawn SpawnEnemyTank(GameObject prefab)
    {

        //Spawn tank pawn (and store it in tanks)
        GameObject tempTankObject = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
        return tempTankObject.GetComponent<Pawn>();

    }

    public Controller_AI SpawnEnemyController(GameObject prefab)
    {

        GameObject tempEnemy = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
        return tempEnemy.GetComponent<Controller_AI>();

    }

    //spawn power ups

    public void SetPowerUp()
    {
        //set powers until the amount has been reached
        do
        {
            //spawner to hold the spawner we will check
            SpawnerTimed tempPowerUpSpawner;

            //check if this enemySpawnPoint has already spawned an object
            do
            {
                //set randomly selected spawn point to this enemyspawn variable
                tempPowerUpSpawner = powerUpSpawners[Random.Range(0, powerUpSpawners.Count)];

            } while (tempPowerUpSpawner.objectToSpawn != null);


            GameObject tempPickUp;

            int tempIndex = Random.Range(0, powerups.Count);

            tempPickUp = powerups[tempIndex];

            /*
            //check if the pickup health's static instance count is less than the desired pickup amount.
            //I.E.:check how many currently spawned pickuphealths currently exist.
            //Also check that the componenet of tempPickUp is the correct component
            if (tempPickUp.GetComponent<PickUpHealth>() == true && PickUpHealth.count < healthPickupAmount)
            {
                Debug.Log("A PickUpHealth pickup has been set to a pickup spawner ");

                tempPowerUpSpawner.objectToSpawn = tempPickUp;

                powerupTotalCount--;
            }


            //check if the PickUpMaxHealthUp static instance count is less than the desired pickup amount.
            //I.E.:check how many currently spawned PickUpMaxHealthUp currently exist.
            //Also check that the componenet of tempPickUp is the correct component
            if (tempPickUp.GetComponent<PickUpMaxHealthUp>() == true && PickUpMaxHealthUp.count < healthMaxPickupAmount)
            {
                Debug.Log("A PickUpMaxHealthUp pickup has been set to a pickup spawner ");

                tempPowerUpSpawner.objectToSpawn = tempPickUp;

                powerupTotalCount--;
            }


            //check if the PickUpMoveSpeed static instance count is less than the desired pickup amount.
            //I.E.:check how many currently spawned PickUpMoveSpeed currently exist.
            //Also check that the componenet of tempPickUp is the correct component
            if (tempPickUp.GetComponent<PickUpMoveSpeed>() == true && PickUpMoveSpeed.count < moveSpeedPickupAmount)
            {
                Debug.Log("A PickUpMoveSpeed pickup has been set to a pickup spawner ");

                tempPowerUpSpawner.objectToSpawn = tempPickUp;

                powerupTotalCount--;
            }
            */


            
       

            switch (tempIndex)
            {
                //health pickup
                case 0:

                    if (PickUpHealth.count < healthPickupAmount)
                    {
                        Debug.Log("A PickUpHealth pickup has been set to a pickup spawner ");

                        tempPowerUpSpawner.objectToSpawn = tempPickUp;

                        tempPowerUpSpawner.timeBetweenSpawns = heatlhPickupSpawnTime;

                        powerupTotalCount--;
                        PickUpHealth.count++;
                    }

                    break;

                    //max health pickup
                case 1:

                    if (PickUpMaxHealthUp.count < healthMaxPickupAmount)
                    {
                        Debug.Log("A PickUpHealth pickup has been set to a pickup spawner ");

                        tempPowerUpSpawner.objectToSpawn = tempPickUp;

                        tempPowerUpSpawner.timeBetweenSpawns = heatlhMaxPickupSpawnTime;

                        powerupTotalCount--;
                        PickUpMaxHealthUp.count++;
                    }

                    break;

                    //move speed pickup
                case 2:

                    if (PickUpMoveSpeed.count < moveSpeedPickupAmount)
                    {
                        Debug.Log("A PickUpMoveSpeed pickup has been set to a pickup spawner ");

                        tempPowerUpSpawner.objectToSpawn = tempPickUp;

                        tempPowerUpSpawner.timeBetweenSpawns = moveSpeedPickupSpawnTime;

                        powerupTotalCount--;
                        PickUpMoveSpeed.count++;
                    }

                    break;
            }


        } while (powerupTotalCount > 0);

    }

}


