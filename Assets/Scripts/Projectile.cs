using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan;

    public void Start()
    {
        //destroy this object
        Destroy(gameObject, lifespan);
    }

}
