using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    public List<PowerUp> powerups;
    private Pawn pawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        //get the pawn this powerupmanager is working with
        pawn = GetComponent<Pawn>();

        //initialize the list of powerups
        powerups = new List<PowerUp>();

    }

    // Update is called once per frame
    public void Update()
    {

        //update the powerup life spans
        UpdatePowerUpLifeSpans();

        //check for expired powers and remove them
        CheckForExpiredPowerUps();
        
    }

    public void UpdatePowerUpLifeSpans()
    {

        foreach (PowerUp powerup in powerups)
        {
        
            powerup.lifeSpan -= Time.deltaTime;

        }

    }

    public void CheckForExpiredPowerUps()
    {
        //make a list called powerups we need to remove
        List<PowerUp> powerupsToRemove = new List<PowerUp>();

        foreach (PowerUp powerup in powerups)
        {
            if (powerup.lifeSpan <= 0)
            {
                //add powerups to list
                powerupsToRemove.Add(powerup);
            }
        }

        //go through list and remove listed power ups
        // -- This way, you aren't iterating through the main list when you remove them
        foreach (PowerUp powerup in powerupsToRemove)
        {
            Remove(powerup);
        }

    }

    public void Add(PowerUp powerup)
    {
        //apply the powerups effects
        powerup.Apply(pawn);

        //check if the lifespan is larger than zero. 
        //If it is zero or negative, then this powerup is permant
        if(powerup.lifeSpan >= 0)
        {
            //add it to our list
            powerups.Add(powerup);
        }

    }

    public void Remove(PowerUp powerup)
    {
        //remove powerup effects
        powerup.Remove(pawn);

        //remove it form our list
        powerups.Remove(powerup);
    }
}
