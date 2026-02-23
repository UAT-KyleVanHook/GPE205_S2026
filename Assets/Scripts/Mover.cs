using UnityEngine;

public abstract class Mover : MonoBehaviour
{

    public abstract void Move(Vector2 moveDirection, float moveSpeed);
    public abstract void Rotate(Vector2 rotateDirection, float turnSpeed);

    //AI Functions
    public abstract void RotateTowards(Vector3 position, float turnSpeed);

    public abstract void RotateAI(Vector3 rotateDirection, float turnSpeed);

}
