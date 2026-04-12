using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Controller : MonoBehaviour
{
    [HideInInspector]
    public Pawn pawn;

    [Header("Score")]
    public int currentScore;

    //how much this object is workth if killed
    public int scoreAmount;

    [Header("Lives")]
    public int lives;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        MakeDecisions();
    }

    //possess a pawn, assign its controller to this controller
    public void Possess(Pawn pawnToPossess)
    {
        pawnToPossess.controller = this;

        this.pawn = pawnToPossess;
    }

    //unpossess a pawn and 
    public void Unpossess()
    {
        pawn.controller = null;
        pawn = null;
    }

    //add to the score
    public void AddToScore(int amount)
    {
        currentScore += amount;
    }

    //set the specified input actions for this comtroller
    public abstract void SetInputActions(InputActionAsset prefabInputActions);


    public abstract void MakeDecisions();
}
