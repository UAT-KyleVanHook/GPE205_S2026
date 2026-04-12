using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Level level;

    public bool bIsSplitScreen;
    private bool bGameOver = false;

    //lives trackers
    [Header("Lives")]
    public int startingPlayerLives;
    public int currentPlayer1Lives;
    public int currentPlayer2Lives;

    //score trackers
    [Header("Score")]
    [HideInInspector] int highScore;
    public int currentPlayer1Score;
    public int currentPlayer2Score;

    [Header("Player Objects")]
    //objects to track players, controllers and cameras
    public GameObject player1Object;
    public Controller player1Controller;
    public GameObject player1Camera;
    public GameObject player2Object;
    public Controller player2Controller;
    public GameObject player2Camera;


    [Header("Prefabs")]
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;
    public GameObject cameraPrefab;

    [Header("Input Action Prefabs")]
    public InputActionAsset player1InputActionsprefab;
    public InputActionAsset player2InputActionsprefab;

    [Header("Up-To-Date Lists")]
    public List<Pawn> tanks;
    public List<Controller> players;
    public List<Controller_AI> ai;
    public List<PlayerSpawn> playerSpawnPoints;
    public List<EnemySpawn> enemySpawnPoints;
    public List<SpawnerTimed> powerUpSpawners;
    public List<PickUp> pickUps;

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
    public int scorePickupAmount;

    public float heatlhPickupSpawnTime;
    public float heatlhMaxPickupSpawnTime;
    public float moveSpeedPickupSpawnTime;
    public float scorePickupSpawnTime;

    private int powerupTotal;
    //used to track how many loops are left
    private int powerupTotalCount;

    [Header("UI-Menu's")]
    public GameObject TitleScreenObject;
    public GameObject MainMenuScreenObject;
    public GameObject OptionsScreenObject;
    public GameObject CreditsScreenObject;
    public GameObject GameplayScreenObject;
    public GameObject GameOverScreenObject;



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
        pickUps = new List<PickUp>();

        //enemies = new List<GameObject>(); 

        //total amount of powerups combined together
        powerupTotal = healthPickupAmount + healthMaxPickupAmount + moveSpeedPickupAmount + scorePickupAmount;

        //set powerupTotalCount to the same as powerupTotal. We will use powerupTotalCount to deincrement.
        powerupTotalCount = powerupTotal;

        bIsSplitScreen = false;

        bGameOver = false;

      
        //check that playerprefs has a highscore to track. If it doesn't make on.
        if(!PlayerPrefs.HasKey("HighScore"))
        {
            Debug.Log("Making HighScore");

            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
        }

        highScore = PlayerPrefs.GetInt("HighScore");

        currentPlayer1Lives = 1;

        if (bIsSplitScreen == true)
        {
            currentPlayer2Lives = 1;
        }


        ActivateTitleScreen();
    }

    public void ActivateTitleScreen()
    {
        //playerCamera.SetActive(false);

        DeactivateAllStates();
        TitleScreenObject.SetActive(true);
    }

    public void ActivateMainMenuScreen()
    {
        DeactivateAllStates();
        MainMenuScreenObject.SetActive(true);
    }

    public void ActivateOptionsScreen()
    {
        DeactivateAllStates();
        OptionsScreenObject.SetActive(true);
    }

    public void ActivateCreditsScreen()
    {
        DeactivateAllStates();
        CreditsScreenObject.SetActive(true);
    }

    public void ActivateGameplayScreen()
    {
        bGameOver = false;

        DeactivateAllStates();
        GameplayScreenObject.SetActive(true);

        //clear previous level

        //start game
        StartGame();

    }

    public void ActivateGameOverScreen()
    {
        DeactivateAllStates();
        GameOverScreenObject.SetActive(true);
    }

    public void GameQuit()
    {
        Debug.Log("Game Quit! Bye!");
        Application.Quit();

    }

    public void DeactivateAllStates()
    {

        TitleScreenObject.SetActive(false);
        MainMenuScreenObject.SetActive(false);
        OptionsScreenObject.SetActive(false);
        CreditsScreenObject.SetActive(false);
        GameplayScreenObject.SetActive(false);
        GameOverScreenObject.SetActive(false);

        //Camera camera = Camera.main;

    }

    void Start()
    {
        //set this to one so that we don't immediatly go to the title screen
        currentPlayer1Lives = 1;

        if(bIsSplitScreen == true)
        {
            currentPlayer2Lives = 1;
        }

        ActivateTitleScreen();

        //Start the game
        //StartGame();

    }

    void Update()
    {
        /*
        //test keys for menu states
        if (inputAction["ActivateTitleScreen"].triggered)
        {
            ActivateTitleScreen();
        }

        if (inputAction["ActivateMainMenuScreen"].triggered)
        {
            ActivateMainMenuScreen();
        }

        if (inputAction["ActivateOptionsScreen"].triggered)
        {
            ActivateOptionsScreen();
        }

        if (inputAction["ActivateCreditsScreen"].triggered)
        {
            ActivateCreditsScreen();
        }

        if (inputAction["ActivateGameplayScreen"].triggered)
        {
            ActivateGameplayScreen();
        }

        if (inputAction["ActivateGameOverScreen"].triggered)
        {
            ActivateGameOverScreen();
        }
        */

        //update player lives
        if (player1Controller != null)
        { 
            currentPlayer1Lives = player1Controller.lives;
        }

        if (bIsSplitScreen == true && player2Controller != null)
        {
            currentPlayer2Lives = player2Controller.lives;
        }


        //check if the player is dead and if the lives counter is larger than 0, and if the GamePlayScreen is active.
        //if true, respawn the player.
        if (GameplayScreenObject.activeSelf &&  currentPlayer1Lives >= 1 && player1Object == null)
        {
            RespawnPlayer1();
        }
        //respawn player 2
        if (GameplayScreenObject.activeInHierarchy && currentPlayer2Lives >= 1 && player2Object == null)
        {

            RespawnPlayer2();
        }


        //set score  if gameplay scen is active
        if (GameplayScreenObject.activeSelf)
        {

            currentPlayer1Score = player1Controller.currentScore;

            Canvas canvas1 = player1Camera.GetComponentInChildren<Canvas>();
            UScoreManager player1ScoreManger = canvas1.GetComponent<UScoreManager>();
            player1ScoreManger.SetLivesValue(currentPlayer1Lives);
            player1ScoreManger.SetScoreValue(currentPlayer1Score);


            if (bIsSplitScreen == true && GameplayScreenObject.activeSelf)
            {
                currentPlayer2Score = player2Controller.currentScore;

                Canvas canvas2 = player2Camera.GetComponentInChildren<Canvas>();
                UScoreManager player2ScoreManger = canvas2.GetComponent<UScoreManager>();
                player2ScoreManger.SetLivesValue(currentPlayer2Lives);
                player2ScoreManger.SetScoreValue(currentPlayer2Score);
            }
        }

        //if the playerLives if less than or equal to zero, and the game over bool is false, then display "Game OVer!" and flip bool to true.
        if (bIsSplitScreen == true && bGameOver == false && currentPlayer1Lives <= 0 && currentPlayer2Lives <= 0 && GameplayScreenObject.activeSelf)
        {
            Debug.Log("Game Over!!!");

            bGameOver = true;

            //check if player score is larger than playerPrefs
            if(currentPlayer1Score > highScore)
            {

                Debug.Log("Setting Hi-Score!");

                PlayerPrefs.SetInt("HighScore", currentPlayer1Score);
                PlayerPrefs.Save();

            }


            if (bIsSplitScreen == true && GameplayScreenObject.activeSelf)
            {
                if (currentPlayer2Score > highScore)
                {

                    Debug.Log("Setting Hi-Score!");

                    PlayerPrefs.SetInt("HighScore", currentPlayer2Score);
                    PlayerPrefs.Save();

                }
            }

            //reset all varaibles for the game
            ResetMap();

            //set gameover screen
            ActivateGameOverScreen();
        }
        else if(bIsSplitScreen == false && bGameOver == false && currentPlayer1Lives <= 0 && GameplayScreenObject.activeSelf)
        {
            //player one game over check

            Debug.Log("Game Over!!!");

            bGameOver = true;

            //check if player score is larger than playerPrefs
            if (currentPlayer1Score > highScore)
            {

                Debug.Log("Setting Hi-Score!");

                PlayerPrefs.SetInt("HighScore", currentPlayer1Score);
                PlayerPrefs.Save();

            }


            //reset all varaibles for the game
            ResetMap();

            //set gameover screen
            ActivateGameOverScreen();

        }

        //if all ai are dead, end game
        if (ai.Count <= 0 && GameplayScreenObject.activeSelf)
        {
            Debug.Log("Game Over!!!");

            bGameOver = true;

            //check if player score is larger than playerPrefs
            if (currentPlayer1Score > highScore)
            {

                Debug.Log("Setting Hi-Score!");

                PlayerPrefs.SetInt("HighScore", currentPlayer1Score);
                PlayerPrefs.Save();

            }

            if (bIsSplitScreen == true && GameplayScreenObject.activeSelf)
            {
                if (currentPlayer2Score > highScore)
                {

                    Debug.Log("Setting Hi-Score!");

                    PlayerPrefs.SetInt("HighScore", currentPlayer2Score);
                    PlayerPrefs.Save();

                }
            }


            //reset all varaibles for the game
            ResetMap();

            //set gameover screen
            ActivateGameOverScreen();

        }

        

    }

    public void StartGame()
    {
        //Do everything to start game

        //generate map
        level.mapGenerator.GenerateMap();

        //Spawn player
        SpawnPlayer1();

        if (bIsSplitScreen == true && GameplayScreenObject.activeInHierarchy)
        {
            SpawnPlayer2();
        }

        //if splitscreen is true, split the screen
        if (bIsSplitScreen == true && GameplayScreenObject.activeInHierarchy)
        {
           
            Camera cam1 = player1Camera.GetComponent<Camera>();
            cam1.rect = new Rect(0f, 0f, 0.5f, 1f);

            Camera cam2 = player2Camera.GetComponent<Camera>();
            cam2.rect = new Rect(0.5f, 0f, 0.5f, 1f);

        }
        else
        {
            Camera cam1 = player1Camera.GetComponent<Camera>();
            cam1.rect = new Rect(0f, 0f, 1f, 1f);
        }

            //set player controller's score to zero (set this a a varable later)
            player1Controller.currentScore = 0;

        if (bIsSplitScreen == true && GameplayScreenObject.activeSelf)
        {
            player2Controller.currentScore = 0;
        }



        //spawn an enemy for the designated enemySpawnCount amount
        do
        {

            //spawn enemy
            SpawnEnemy();

            //increment startingSpawnCount
            startingSpawnCount++;

        } while (startingSpawnCount < enemySpawnCount);


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

        //Debug.Log(PickUpHealth.count);
        //Debug.Log(PickUpMaxHealthUp.count);
        //Debug.Log(PickUpMoveSpeed.count);

    }

    public void SpawnPlayer1()
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
        player1Controller = tempPlayerController;

        //set the playerinput
        player1Controller.SetInputActions(player1InputActionsprefab);

        //set the lives of the player on spawn
        player1Controller.lives = startingPlayerLives;

        //set the current lives of the player
        currentPlayer1Lives = player1Controller.lives;

        //set controller for healthcomp
        PlayerHealthComponent tempHealthComp = tempTankPawn.GetComponent<PlayerHealthComponent>();
        tempHealthComp.AssignController(player1Controller);

        //add Audio Listener to pawn
        tempTankPawn.AddComponent<AudioListener>();

        //spawn and instantiate camera object
        player1Camera = SpawnCamera(cameraPrefab);


        // move to spawnpoint
        tempTankPawn.transform.position = playerSpawnPosition;

        //set the player to be used as a target for AI
        SetPlayer1Object(tempTankPawn.gameObject);

        //set camera target
        CameraFollow tempCamera = player1Camera.GetComponent<CameraFollow>();
        tempCamera.SetTarget(player1Object);


    }

    //set playerObject in gameManager
    public void SetPlayer1Object(GameObject target)
    {
        //Pawn tempPawn = target.GetComponent<Pawn>();    

        player1Object = target;
    }

    public GameObject SpawnCamera(GameObject prefab)
    {
        GameObject tempCameraObject = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
        return tempCameraObject;
    }

    //set the target for the playerCamera to the player pawn.
    //public void SetCameraTarget(GameObject target)
    //{
    // playerCamera = Camera.main;
    //} 

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

    public void RespawnPlayer1()
    {
        //player has died, respawn tank prefab and deincrement lives
        //currentPlayer1Lives -= 1;

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
        player1Controller.Possess(tempTankPawn);

        //set controller for healthcomp
        PlayerHealthComponent tempHealthComp = tempTankPawn.GetComponent<PlayerHealthComponent>();
        tempHealthComp.AssignController(player1Controller);

        //add Audio Listener to pawn
        tempTankPawn.AddComponent<AudioListener>();

        // move to spawnpoint
        tempTankPawn.transform.position = playerSpawnPosition;


        SetPlayer1Object(tempTankPawn.gameObject);

        CameraFollow tempCamera = player1Camera.GetComponent<CameraFollow>();
        tempCamera.SetTarget(player1Object);

    }




    //spawn player two
    public void SpawnPlayer2()
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
        player2Controller = tempPlayerController;

        //set the playerinput
        player2Controller.SetInputActions(player2InputActionsprefab);

        //set the lives of the player on spawn
        player2Controller.lives = startingPlayerLives;

        //set the current lives of the player
        currentPlayer2Lives = player2Controller.lives;

        //set controller for healthcomp
        PlayerHealthComponent tempHealthComp = tempTankPawn.GetComponent<PlayerHealthComponent>();
        tempHealthComp.AssignController(player2Controller);

        //add Audio Listener to pawn
        //tempTankPawn.AddComponent<AudioListener>();

        //spawn and instantiate camera object
        player2Camera = SpawnCamera(cameraPrefab);


        // move to spawnpoint
        tempTankPawn.transform.position = playerSpawnPosition;

        //set the player to be used as a target for AI
        SetPlayer2Object(tempTankPawn.gameObject);

        //set camera target
        CameraFollow tempCamera = player2Camera.GetComponent<CameraFollow>();
        tempCamera.SetTarget(player2Object);


    }


    public void SetPlayer2Object(GameObject target)
    {
        //Pawn tempPawn = target.GetComponent<Pawn>();    

        player2Object = target;
    }


    public void RespawnPlayer2()
    {
        //we need a check in the method as for some resaon it is spawning tanks when we flip form split screen back to single screen.
        if (bIsSplitScreen == true && GameplayScreenObject.activeInHierarchy)
        {


            //player has died, respawn tank prefab and deincrement lives
            //currentPlayer2Lives -= 1;

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
            player2Controller.Possess(tempTankPawn);

            //set controller for healthcomp
            PlayerHealthComponent tempHealthComp = tempTankPawn.GetComponent<PlayerHealthComponent>();
            tempHealthComp.AssignController(player2Controller);

            //add Audio Listener to pawn
            //tempTankPawn.AddComponent<AudioListener>();

            // move to spawnpoint
            tempTankPawn.transform.position = playerSpawnPosition;


            SetPlayer2Object(tempTankPawn.gameObject);

            CameraFollow tempCamera = player2Camera.GetComponent<CameraFollow>();
            tempCamera.SetTarget(player2Object);

        }

    }







    //spawn enemies //

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

                //Sentry AI
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

                //score pickup
                case 3:

                    if (PickUpScore.count < scorePickupAmount)
                    {
                        Debug.Log("A PickUpScore pickup has been set to a pickup spawner ");

                        tempPowerUpSpawner.objectToSpawn = tempPickUp;

                        tempPowerUpSpawner.timeBetweenSpawns = scorePickupSpawnTime;

                        powerupTotalCount--;
                        PickUpScore.count++;
                    }

                    break;
            }


        } while (powerupTotalCount > 0);

    }

    //reset the variables of the game
    public void ResetMap()
    {

        Debug.Log("Reset Game.");

        powerupTotalCount = powerupTotal;
        startingSpawnCount = 0;

        bGameOver = false;

        //reset player trackers
        if (GameplayScreenObject.activeSelf)
        {
            Destroy(player1Object.gameObject);
            Destroy(player1Controller.gameObject);
            Destroy(player1Camera.gameObject);

            player1Object = null;
            player1Controller = null;
            player1Camera = null;
        }


        if (bIsSplitScreen == true && GameplayScreenObject.activeSelf)
        {

            Destroy(player2Object.gameObject);
            Destroy(player2Controller.gameObject);
            Destroy(player2Camera.gameObject);

            player2Object = null;
            player2Controller = null;
            player2Camera = null;
        }
        
        //reset health and score
        currentPlayer1Score = 0;
        if (bIsSplitScreen == true)
        {
            currentPlayer2Score = 0;
        }

        currentPlayer1Lives = 1;
        if (bIsSplitScreen == true)
        {
            currentPlayer2Lives = 1;
        }


        //go through each list and destroy the object
        
        for(int i = tanks.Count - 1; i >= 0; i--)
        {
           
            Destroy(tanks[i].gameObject);
            tanks.RemoveAt(i);
        }  
          
         
        for(int i = players.Count - 1; i >= 0; i--)
        {
            Destroy(players[i].gameObject);
            players.RemoveAt(i);
        }

        for (int i = ai.Count - 1; i >= 0; i--)
        {
            Destroy(ai[i].gameObject);
            ai.RemoveAt(i);
        }

        for (int i = powerUpSpawners.Count - 1; i >= 0; i--)
        {
            Destroy(powerUpSpawners[i].gameObject);
            powerUpSpawners.RemoveAt(i);
        }

        for (int i = pickUps.Count - 1; i >= 0; i--)
        {
            Destroy(pickUps[i].gameObject);
            pickUps.RemoveAt(i);
        }

        for (int i = playerSpawnPoints.Count - 1; i >= 0; i--)
        {
            Destroy(playerSpawnPoints[i].gameObject);
            playerSpawnPoints.RemoveAt(i);
        }

        for (int i = enemySpawnPoints.Count - 1; i >= 0; i--)
        {
            Destroy(enemySpawnPoints[i].gameObject);
            enemySpawnPoints.RemoveAt(i);
        }


        //remove currnetly spawned map
        if (GameplayScreenObject.activeSelf)
        {
            level.mapGenerator.RemoveMap();
        }
        



        PickUpHealth.count = 0;
        PickUpMaxHealthUp.count = 0;
        PickUpMoveSpeed.count = 0;
        PickUpScore.count = 0;




    }


}


