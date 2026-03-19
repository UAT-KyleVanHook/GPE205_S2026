using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PowerUpHealth : PowerUp
{
    public float amountToHeal;
    
    

    public override void Apply(Pawn target)
    {
        //TODO: Heal the pawn in target
        Debug.Log("Healed!");

        HealthComponent targetHealthComp = target.GetComponent<HealthComponent>();

        //check if the pawn  has a heatlhcomponent
        if (targetHealthComp != null)
        {
            //call its heal component function
            targetHealthComp.Heal(amountToHeal);

        }

    }

    public override void Remove(Pawn target)
    {
        //TODO: Nothing. We don't do anything when removing a healing powerup 
    }
}
