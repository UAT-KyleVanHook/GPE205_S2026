using UnityEngine;
using UnityEngine.InputSystem;


public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    public Vector3 CameraOffset;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target = GameManager.instance.playerObject;


    }

    // Update is called once per frame
    void Update()
    {

        //target = GameManager.instance.playerObject.transform;

        //show the current position of the camera target.
        //Debug.Log(target.transform.position);

        //check if the target is null and the GameplayScene is set active
        //if(target == null && GameManager.instance.GameplayScreenObject.activeSelf)
        //{
            //target = GameManager.instance.playerObject;
        //}

    }


    void LateUpdate()
    {
        //get the pawn to get the pawns turnspeed
        //PawnTank lookTarget = target.GetComponent<PawnTank>();

        //use the inputActions to get the direction that the camera should rotate.
        // Vector2 movementVector = inputActions["Move"].ReadValue<Vector2>();

        // rotate directional, based on the movementvector. Is multiplied by the target players turn speed.
        //CameraYaw = movementVector.x * (lookTarget.turnSpeed * Time.deltaTime);



        //check if the GameplayScreenObject is set as active
        if (GameManager.instance.GameplayScreenObject.activeSelf && target != null)
        {

            // returns the camera offset from local space to world space and sets the cameras transfrom position.
            transform.position = target.transform.TransformPoint(CameraOffset);

            //look at the player target. 
            //could set this at a point in front of the tank pawn later.
            transform.LookAt(target.transform.position);

        }





    }

    public void SetTarget(GameObject gameObject)
    {
        target = gameObject;
    }

}
