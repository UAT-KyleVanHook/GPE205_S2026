using UnityEngine;

//make sure that this component requires a certain component
[RequireComponent (typeof(Collider))]

public class DamageOnOverlap : MonoBehaviour
{
    public float damageDone;
    private Collider mCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get collider
        mCollider = GetComponent<Collider> ();
        //set this collider as a trigger
        mCollider.isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        //get other objects health componenet
        HealthComponent otherHealth = other.GetComponent<HealthComponent>();

        //if it has a healthComp
        if (otherHealth != null)
        {
            //initiate damage on healthComp
            otherHealth.TakeDamage(damageDone);
        }

        //Destroy projectile
        Destroy(gameObject);
    }
}
