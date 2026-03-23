using UnityEngine;
using UnityEngine.InputSystem;


public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    public Vector3 CameraOffset;

    public InputActionAsset inputActions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.instance.playerObject;

        //enable input actions
        inputActions.Enable();

    }

    // Update is called once per frame
    void Update()
    {

        //target = GameManager.instance.playerObject.transform;

        //show the current position of the camera target.
        //Debug.Log(target.transform.position);

        if(target == null)
        {
            target = GameManager.instance.playerObject;
        }

    }


    void LateUpdate()
    {
        //get the pawn to get the pawns turnspeed
        //PawnTank lookTarget = target.GetComponent<PawnTank>();

        //use the inputActions to get the direction that the camera should rotate.
       // Vector2 movementVector = inputActions["Move"].ReadValue<Vector2>();

        // rotate directional, based on the movementvector. Is multiplied by the target players turn speed.
        //CameraYaw = movementVector.x * (lookTarget.turnSpeed * Time.deltaTime);
        

        // returns the camera offset from local space to world space and sets the cameras transfrom position.
        transform.position = target.transform.TransformPoint(CameraOffset);

        //look at the palyer target. 
        //could set this at a point in front of the tank pawn later.
        transform.LookAt(target.transform.position);



    }

}
