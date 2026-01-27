using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public Mover mover;

    public Controller controller;

    public abstract void Move(Vector3 directionToMove);
    public abstract void Rotate(Vector3 directionToRotate);
    public abstract void Shoot();

}
