using UnityEngine;


[System.Serializable]
public class PowerUpMaxHealthUp : PowerUp
{
    public float amountToIncrease;

    public override void Apply(Pawn target)
    {
        

        HealthComponent targetHealthComp = target.GetComponent<HealthComponent>();

        Debug.Log("Increased Max Health of: " + targetHealthComp.name + "!");
        Debug.Log("Old Max Health of: " + targetHealthComp.maxHealth);

        //check if the pawn  has a heatlhcomponent
        if (targetHealthComp != null)
        {
            //call its Increase Max Health component function
            targetHealthComp.IncreaseMaxHealth(amountToIncrease);

        }

        Debug.Log("Current Max Health of: " + targetHealthComp.maxHealth);

    }

    public override void Remove(Pawn target)
    {
        //TODO: Nothing. We don't do anything when removing a healing powerup 
    }
}
