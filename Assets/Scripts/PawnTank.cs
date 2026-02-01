using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PawnTank : Pawn
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        
        //Save tank to gamemanager
        GameManager.instance.tanks.Add(this);

        //Do what all pawns do
        base.Start();

    }

    public void OnDestroy()
    {
        
        //Remove tank from gamemanager
        GameManager.instance.tanks.Remove(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move(Vector3 directionToMove)
    {

        //Debug.Log("Moving!");
        mover.Move(directionToMove, moveSpeed);
        

    }
    public override void Rotate(Vector3 directionToRotate)
    {
        //Debug.Log("Rotating!");
        mover.Rotate(directionToRotate, turnSpeed);
    }

    public override void Shoot()
    {
        Debug.Log("Pew-Pew!");
    }

}
