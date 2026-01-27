using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPlayer : Controller
{
    //public KeyCode moveForwardKey;
    //public KeyCode moveBackKey;
    //public KeyCode rotateLeftKey;
    //public KeyCode rotateRightKey;
    //public KeyCode shootKey;
    //public KeyCode reloadKey;

    public InputActionAsset inputActions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //enable input actions
        inputActions.Enable();


    }

    // Update is called once per frame
    public override void Update()
    {
        //do what our parent class does in its function
        base.Update();
    }

    public override void MakeDecisions()
    {

        Vector2 movementVector = inputActions["Move"].ReadValue<Vector2>();

        //pass movement vector from inputAction into the pawns move and rotate functions
        pawn.Move(new Vector2(0, movementVector.y));
        pawn.Rotate(new Vector2(movementVector.x, 0));

        //Debug.Log("Move Vector: " + movementVector);

        if (inputActions["Shoot"].triggered)
        {
            pawn.Shoot();
        }

    }
}
