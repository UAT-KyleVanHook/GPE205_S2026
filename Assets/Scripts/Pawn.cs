using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [HideInInspector]
    public Mover mover;

    [HideInInspector]
    public Controller controller;

    [HideInInspector]
    public Noisemaker noisemaker;


    public float moveSpeed;

    public float turnSpeed;


    public abstract void Move(Vector3 directionToMove);
    public abstract void Rotate(Vector3 directionToRotate);
    public abstract void Shoot();

    public Controller GetController() { return controller; }

    //AI Functions
    public abstract void RotateTowards(Vector3 position);

    public abstract void RotateAI(Vector3 rotateDirection);

    public abstract void AIShoot();

    //

    public virtual void Start()
    {
        //get mover componenet
        mover = GetComponent<Mover>();

        noisemaker = GetComponent<Noisemaker>();
    }
}
