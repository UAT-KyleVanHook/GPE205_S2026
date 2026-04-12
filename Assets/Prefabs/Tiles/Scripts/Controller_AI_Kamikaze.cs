using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller_AI_Kamikaze : Controller_AI
{


    public override void Start()
    {

        GameManager.instance.ai.Add(this);

        //do the base of start
        base.Start();


        ChangeState(AIState.Idle);


    }

    public override void Update()
    {
        if (target == null)
        {
            //set enemy targets
            target = GameManager.instance.player1Object;
        }

        base.Update();




    }

    public override void MakeDecisions()
    {
        //Debug.Log("closest point:" + closetPoint);

        //if the pawn we are attached to is null, destroy this controller
        if (pawn == null)
        {
            Destroy(gameObject);
        }


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

                    ChangeState(AIState.Turn);
                }



                //check if player has been spotted OR heard
                if (CanSee(target) || CanHear(target))
                {

                    ChangeState(AIState.Chase);

                }
                break;


            case AIState.Turn:

                Turn(new Vector3(0, 1, 0));




                //check if player has been spotted OR heard
                if (CanSee(target) || CanHear(target))
                {

                    ChangeState(AIState.Chase);

                }

                break;



            //get the player as a target
            case AIState.Chase:

                DoChase();


                //check if the enemy can't see or hear the player
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

