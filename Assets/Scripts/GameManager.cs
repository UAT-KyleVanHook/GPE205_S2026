using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Controller playerOne;
    public Pawn startPawn;

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
        
    }

    void Start()
    {
        playerOne.Possess(startPawn);
    }


}
