using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [HideInInspector]
    public Mover mover;

    [HideInInspector]
    public Controller controller;

    public float moveSpeed;

    public float turnSpeed;

    public abstract void Move(Vector3 directionToMove);
    public abstract void Rotate(Vector3 directionToRotate);
    public abstract void Shoot();

    public Controller GetController() { return controller; }

    public virtual void Start()
    {
        //get mover componenet
        mover = GetComponent<Mover>();
    }
}
