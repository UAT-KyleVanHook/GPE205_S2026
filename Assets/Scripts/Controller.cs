using UnityEngine;
using UnityEngine.UI;

public abstract class Controller : MonoBehaviour
{
    [HideInInspector]
    public Pawn pawn;

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
    public abstract void MakeDecisions();
}
