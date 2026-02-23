using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PawnTank : Pawn
{
    
    private TankShooter shooter;

    public float shootForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        
        //Save tank to gamemanager
        GameManager.instance.tanks.Add(this);

        //Do what all pawns do
        base.Start();

        //get the shooter attached to the pawn
        shooter = gameObject.GetComponent<TankShooter>();


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

        //set noise amount
        noisemaker.SetNoiseVolume(5);

    }
    public override void Rotate(Vector3 directionToRotate)
    {
        //Debug.Log("Rotating!");
        mover.Rotate(directionToRotate, turnSpeed);


        //set noise amount
        noisemaker.SetNoiseVolume(5);
    }

    public override void Shoot()
    {
        //Debug.Log("Pew-Pew!");
  
        shooter.Shoot();

        //set noise amount
        noisemaker.SetNoiseVolume(20);
    }


    //AI methods
    public override void RotateTowards(Vector3 position)
    {
        mover.RotateTowards(position, turnSpeed);
    }

    public override void RotateAI(Vector3 rotateDirection)
    {
        mover.RotateAI(rotateDirection, turnSpeed);
    }

    public override void AIShoot()
    {
        shooter.AIShoot();
    }

}
