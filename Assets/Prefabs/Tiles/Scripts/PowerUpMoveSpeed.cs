using UnityEngine;

[System.Serializable]
public class PowerUpMoveSpeed : PowerUp
{
    public float speedBostAmount;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Apply(Pawn target)
    {
        // Increase the pawn's move speed
        target.moveSpeed += speedBostAmount;
    }

    public override void Remove(Pawn target)
    {
        // reset the pawn's move speed
        target.moveSpeed -= speedBostAmount;
    }

}
