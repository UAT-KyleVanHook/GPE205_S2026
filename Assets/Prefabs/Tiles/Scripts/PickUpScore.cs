using UnityEngine;

public class PickUpScore : PickUp
{
    public static int count;
    public PowerUpScore powerup;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {


        //increment static count
        count++;

        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        //Check if the other object has a PowerUpManagar;

        PowerUpManager otherManager = other.GetComponent<PowerUpManager>();

        if (otherManager != null)
        {
            //If yes, add this to the powerup manager
            otherManager.Add(powerup);

            //Destroy this object
            Destroy(this.gameObject);

        }

        base.OnTriggerEnter(other);

    }
}
