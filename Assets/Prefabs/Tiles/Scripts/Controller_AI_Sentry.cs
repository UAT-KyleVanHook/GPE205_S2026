using UnityEngine;

public class Controller_AI_Sentry : Controller_AI
{

    private bool canTurn;
    private float targetRotationY;
    private float tolerance = 0.1f;


    public override void Start()
    {

        GameManager.instance.ai.Add(this);

        //do the base of start
        base.Start();


        ChangeState(AIState.Idle);

        canTurn = true;

        //get pawns transform rotation at start
        targetRotationY = pawn.transform.eulerAngles.y;

    }

    public override void Update()
    {
        base.Update();

    }

    public override void MakeDecisions()
    {
        //Debug.Log("Pawn's Y eulerAngle:" + pawn.transform.eulerAngles.y);
        //Debug.Log("targetRotationY Y eulerAngle:" + targetRotationY);

        switch (currentState)
        {
            //idle state
            case AIState.Idle:

                //do nothing
                DoIdle();

                //check for a target
                if (IsHasTarget() == false)
                {
                    ChangeState(AIState.ChooseTarget);
                }

                
                //check if time has elapsed
                if (HasTimeElapsed(5))
                {

                    canTurn = true;

                    //set new target rotation
                    targetRotationY = pawn.transform.eulerAngles.y + 90;

                    ChangeState(AIState.Turn);
                }
                
                
                //check if player has been spotted OR heard
                if (CanSee(target) || CanHear(target))
                {
                    canTurn = true;

                    //set new target rotation
                    targetRotationY = pawn.transform.eulerAngles.y + 90;

                    ChangeState(AIState.TurnAndShoot);
                }

                break;

                //turn 90 degrees
            case AIState.Turn:

                //check for a target
                if (IsHasTarget() == false)
                {
                    ChangeState(AIState.ChooseTarget);
                }

                //check if the pawn can turn
                if (canTurn)
                {

                    Turn(new Vector3(0, 1, 0));

                    float currentY = Mathf.Repeat(pawn.transform.eulerAngles.y, 360f);
                    float currentTargetY = Mathf.Repeat(targetRotationY, 360f);

                    if ((Mathf.Abs(currentY - currentTargetY) <= tolerance))
                    {
                        canTurn = false;
                    }

                }


                //check if player has been spotted OR heard
                if (CanSee(target) || CanHear(target))
                {
                    canTurn = true;

                    //set new target rotation
                    targetRotationY = pawn.transform.eulerAngles.y + 90;

                    ChangeState(AIState.TurnAndShoot);
                }


                //has finished turn
                if (HasTimeElapsed(5))
                {
                    canTurn = true;

                    //set new target rotation
                    targetRotationY = pawn.transform.eulerAngles.y + 90;

                    ChangeState(AIState.Idle);
                }

                break;

                //turn and shoot
            case AIState.TurnAndShoot:

                TurnAndShoot(target);

                //check for a target
                if (IsHasTarget() == false)
                {
                    ChangeState(AIState.ChooseTarget);
                }

                //can't see player
                if (!CanSee(target) || !CanHear(target))
                {
                    canTurn = true;

                    //set new target rotation
                    targetRotationY = pawn.transform.eulerAngles.y + 90;

                    ChangeState(AIState.Idle);
                }

                break;

                //get the player as a target
            case AIState.ChooseTarget:

                TargetPlayerOne();

                ChangeState(AIState.Idle);

                break;

        }


    }

    public void OnDestroy()
    {

        //Remove tank from gamemanager
        GameManager.instance.ai.Remove(this);

    }


}
