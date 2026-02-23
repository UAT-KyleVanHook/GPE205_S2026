using NUnit.Framework.Constraints;
using System.Text.RegularExpressions;
using UnityEngine;

public class Controller_AI_Pursuer : Controller_AI
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
        base.Update();

    }

    public override void MakeDecisions()
    {
        Debug.Log("closest point:" + closetPoint);


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

                    ChangeState(AIState.ChaseAndShoot);

                }
                break;


            case AIState.Turn:

                Turn(new Vector3(0, 1, 0));




                //check if player has been spotted OR heard
                if (CanSee(target) || CanHear(target))
                {

                    ChangeState(AIState.ChaseAndShoot);

                }

                break;



            //get the player as a target
            case AIState.ChaseAndShoot:

                DoAttackState();

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
