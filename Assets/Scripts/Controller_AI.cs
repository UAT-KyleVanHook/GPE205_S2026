using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum AIState { Idle, Chase, Flee, ChaseAndShoot, Patrol, Turn, TurnAndShoot, Shoot, ChooseTarget, Roam }

public abstract class Controller_AI : Controller
{

    private Quaternion roamDirection;
    private float lastChangeStateTime;
    protected AIState currentState = AIState.Idle;

    public GameObject target; //originally a Transform
    public float fleeDistance = 10.0f;
    public float hearingDistance = 1.0f;
    public float visionDistance = 10.0f;
    public float FOVAngle = 60.0f;

    //patrol variables
    private int currentWaypoint = 0; 
    public Transform[] waypoints;
    public float waypointStopDistance;

    protected bool bStopPatrol = false;

    //closest roam point
    protected Transform closetPoint;

    public override void Start()
    {

        //save our transition time as when we started
        lastChangeStateTime = Time.time;
        
        //get the pawn attached to this controller
        Pawn tempPawn = gameObject.GetComponent<Pawn>();

        pawn = tempPawn;

        Possess(pawn);

    }

    public void DoIdle()
    {
        
        //Debug.Log(currentState);
        //do nothing

    }

    public void DoChase()
    {
        //start calculations to find position to move towards
        Seek(target);

    }

    //doesn't work
    public void Turn(Vector3 rotateVector)
    {
        
       //Debug.Log(currentState);
       pawn.RotateAI(rotateVector);

    }

    public void Seek(Vector3 position)
    {

        //turn towards 
        pawn.RotateTowards(position);

        //move forward
        pawn.Move(new Vector2(0, 1));

    }

    //overload for Seek, using Transform
    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        Seek(targetTransform.position);
    }

    //overload for Seek, using Pawn
    public void Seek(GameObject targetPawn)
    {
        // Seek the pawn's transform!
        Seek(targetPawn.transform);
    }


    //shoot function
    public void Fire()
    {

        pawn.AIShoot();
    }

    //move and shoot
    protected void DoAttackState()
    {
        // Chase
        Seek(target);
        // Shoot
        Fire();
    }

    //turn and shoot
    protected void TurnAndShoot(GameObject targetPawn)
    {
        //turn towards 
        pawn.RotateTowards(targetPawn.transform.position);

        pawn.AIShoot();
    }


    protected void TurnTowardsPoint(Vector3 targetPosition)
    {
        pawn.RotateTowards(targetPosition);
    }

    //TunrandShhot overload
    protected void TurnTowardsPoint(Transform targetTransform)
    {
        //turn towards 
        TurnTowardsPoint(targetTransform.position);
    }

    //turn towards a point, overload
    protected void TurnTowardsPoint(GameObject targetPawn)
    {
        //turn towards 
        TurnTowardsPoint(targetPawn.transform);
    }

    public void DoFlee()
    {
        //pick a point a given distance away from the player!
        //Find vector to the player
        Vector3 vectorToTarget = pawn.transform.position - target.transform.position;

        float distanceToPlayer = vectorToTarget.magnitude;

        //Reverse it!
        Vector3 flippedVectorToTarget = -vectorToTarget;

        //Find distance to flee
        flippedVectorToTarget.Normalize(); // was vectorToTarget

        float percentOfFleeDistance = distanceToPlayer / fleeDistance;

        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);

        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        float newFleeDistance = flippedPercentOfFleeDistance * fleeDistance;

        Vector3 targetPosition = pawn.transform.position + (flippedVectorToTarget * newFleeDistance);

        //find new target position
        Seek(targetPosition);

    }

    public void ChangeState(AIState newState)
    {
        //change the state
        currentState = newState;

        //save the time we changed states
        lastChangeStateTime = Time.time;
    }


    public bool CanMoveForward(float distance)
    {
        //TODO: Raycast forward for the distance we passed in
        //TODO: If hit, return false,

        // Create the ray we are going to cast
        Ray theRay = new Ray();

        theRay.origin = pawn.transform.position;

        theRay.direction = pawn.transform.forward;

        // Create a varaible that holds information about the raycast hit
        RaycastHit hitData = new RaycastHit();

        // If our ray hit something 
        if (Physics.Raycast(theRay, out hitData, distance))
        {
            // There is something in our way, so return false
            return false;
        }

        //otherwise, return true
        return true;
    }

    public bool IsObjectInRange(Transform objectToCheck, float range)
    {

        //Find distance between our pawn and the object we are checking
        if(Vector3.Distance(objectToCheck.position, pawn.transform.position) < range)
        {
            //if < range return true
            return true;
        }
         
        //otherwise, false
        return false;
    }


    public bool IsRoamDirectionChosen()
    {

        if(roamDirection != Quaternion.identity)
        {
            // If yes, return true
            return true;
        }
        else
        {
            // Otherwise, return false
            return false;
        }
           
    }

    public bool HasTimeElapsed(float seconds)
    {
        //If the current time minus the time we last changed is > the time we are waiting
        if (Time.time - lastChangeStateTime >= seconds)
        {
            return true;
        }

        //otherwise, the time has not yet passed
        return false;

    }


    //get the player as the target
    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if (GameManager.instance != null)
        {
            // And the array of players exists
            if (GameManager.instance.players != null)
            {
                // And there are players in it
                if (GameManager.instance.players.Count > 0)
                {
                    //Then target the gameObject of the pawn of the first player controller in the list
                    target = GameManager.instance.players[0].pawn.gameObject;
                }
            }
        }
    }


    //check the we have a target
    protected bool IsHasTarget()
    {
        // return true if we have a target, false if we don't
        return (target != null);
    }


    //vision
    public bool CanSee(GameObject target)
    {
        RaycastHit hit;

        //field of view check

        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - transform.position;

        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

        // if that angle is less than our field of view
        if (angleToTarget < FOVAngle)
        {

            //line of sight check
            Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

            if (Physics.Raycast(pawn.transform.position, vectorToTarget, out hit, visionDistance))
            {
                if (hit.collider.gameObject == target)
                {

                    return true;

                }

                return false;

            }

        }

        //nothing was hit
        return false;

    }

    //hearing
    public bool CanHear(GameObject target)
    {
        //check if target has noisemaker
       
        Noisemaker targetNoisemaker = target.GetComponent<Noisemaker>();

        if(targetNoisemaker == null)
        {
            return false;
        }

        //If so, are they making noise (>0)
        if(targetNoisemaker.noiseVolume > 0)
        {
            //If so, is the distance between the two centers smaller than the two radi added together
            float totalDistance = Vector3.Distance(target.transform.position, pawn.transform.position);

            if(totalDistance <= targetNoisemaker.noiseVolume + hearingDistance)
            {

                return true;

            }
        }

        //otherwise, return false
        return false;
    }


    //patrol function
    protected void Patrol()
    {
  
        // If we have a enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint)
        {
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {

                NearWayPoint();
                currentWaypoint++;

                //having a tank pause this cn cause an IndexOverfloaw, so once currant Waypoint is larger than the length of the array, we reset it.
                if (currentWaypoint >= waypoints.Length)
                {
                    RestartPatrol();
                }



            }
        }
        else
        {


            RestartPatrol();
        }

    }

    //restart the patrol to the original waypoint
    protected void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }


    //tell bool NearWayPoint() to turn false or true
    protected void NearWayPoint()
    {
        if(bStopPatrol == false)
        {
            bStopPatrol = true;
        }
        else
        {
            bStopPatrol = false;
        }

  
    }


    //function to rotate pawn towards the current waypoint
    protected void TurnTowardsCurrentWayPoint()
    {
        //Debug.Log(waypoints[currentWaypoint]);

        pawn.RotateTowards(waypoints[currentWaypoint].position);
    }

    //didn't have time to finish
    /*

    // Find the closest waypoint in this pawns list of waypoints
    protected void FindRoamPoint()
    {
        closetPoint = waypoints[0];
        float closetPointDistance = Vector3.Distance(pawn.transform.position, closetPoint.transform.position);

        // Iterate through them one at a time
        foreach (Transform point in waypoints)
        {
            // If this one is closer than the closest
            if (Vector3.Distance(pawn.transform.position, point.transform.position) <= closetPointDistance)
            {
                // It is the closest
                closetPoint = point;
                closetPointDistance = Vector3.Distance(pawn.transform.position, closetPoint.transform.position);
            }
        }

    }

    //check if the pawmn has reached the closest way point
    protected bool HasReachedRoamPoint()
    {

        if (Vector3.Distance(pawn.transform.position, closetPoint.transform.position) <= waypointStopDistance)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    */

    //helper function in case we need it.
    //finds the nearest PawnTank
    protected void TargetNearestTank()
    {
        // Get a list of all the tanks (pawns)
        Pawn[] allTanks = FindObjectsByType<PawnTank>(FindObjectsSortMode.None);

        // Assume that the first tank is closest
        Pawn closestTank = allTanks[0];
        float closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);

        // Iterate through them one at a time
        foreach (Pawn tank in allTanks)
        {
            // If this one is closer than the closest
            if (Vector3.Distance(pawn.transform.position, tank.transform.position) <= closestTankDistance)
            {
                // It is the closest
                closestTank = tank;
                closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
            }
        }

        // Target the closest tank
        target = closestTank.gameObject;
    }
}
