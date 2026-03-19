using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller_AI_Patroller : Controller_AI
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {

        GameManager.instance.ai.Add(this);


        //do the base of start
        base.Start();

        ChangeState(AIState.Idle);

    }

    public override void Update()
    {
        base.Update();


    }

    public override void MakeDecisions()
    {

        //Debug.Log(currentState);

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

                //if the AI can see OR hear the player, start shooting at them.
                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.TurnAndShoot);
                }

                //if time has passed and the player hasn't been seen or heard go on patrol
                if (HasTimeElapsed(5))
                {

                    ChangeState(AIState.Turn);
                }


                break;

                //turn towards the currant patrol point
            case AIState.Turn:

                TurnTowardsCurrentWayPoint();

                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.TurnAndShoot);
                }


                if (HasTimeElapsed(2))
                {
                    ChangeState(AIState.Patrol);
                }

                break;

                //patrol to the currant patrol point
            case AIState.Patrol:


                Patrol();

                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.TurnAndShoot);
                }


                if (bStopPatrol)
                {

                    NearWayPoint();

                    ChangeState(AIState.Idle);
                }

                 break;

                //turn and shoot towards player
            case AIState.TurnAndShoot:

                TurnAndShoot(target);

                //if the AI can see OR hear the player, start shooting at them.
                if (!CanSee(target) || !CanHear(target))
                {
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

