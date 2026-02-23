using System.Text.RegularExpressions;
using UnityEngine;

public class Controller_AI_Flee : Controller_AI
{
    //how close player can get before AI flees
    public float fleeRange = 10.0f;

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

                //if the AI can see OR hear the player
                if (CanSee(target) || CanHear(target))
                {
                    ChangeState(AIState.TurnAndShoot);
                }

                //if the player is within range, change to flee state
                if (IsObjectInRange(target.transform, fleeRange))
                {
                    ChangeState(AIState.Flee);
                }

                break;

            //run away state
            case AIState.Flee:

                DoFlee();

                //go back to idle
                if(!IsObjectInRange(target.transform, fleeRange))
                {

                    ChangeState(AIState.Idle);

                }

                break;


            //turn to player and shoot state
            case AIState.TurnAndShoot:

                TurnAndShoot(target);

                //check for a target
                if (IsHasTarget() == false)
                {
                    ChangeState(AIState.ChooseTarget);
                }

                //if the player is within range, change to flee state
                if (IsObjectInRange(target.transform, fleeRange))
                {
                    ChangeState(AIState.Flee);
                }

                //if the AI can't see OR hear the player, go back to idle
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
