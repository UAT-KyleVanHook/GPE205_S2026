using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PawnPlayer : Pawn
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move(Vector3 directionToMove)
    {

        Debug.Log("Moving!");

    }
    public override void Rotate(Vector3 directionToRotate)
    {
        Debug.Log("Rotating!");
    }

    public override void Shoot()
    {
        Debug.Log("Pew-Pew!");
    }

}
