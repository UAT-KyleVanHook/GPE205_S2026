[System.Serializable]

// Is not a monobehavior
public abstract class PowerUp 
{
    public float lifeSpan;
    
    //apply the powerup effect
    public abstract void Apply(Pawn target);

    //remove the powerup effect
    public abstract void Remove(Pawn target);

}
