using UnityEngine;

public class PickUpMaxHealthUp : PickUp
{

    public PowerUpMaxHealthUp powerup;

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
