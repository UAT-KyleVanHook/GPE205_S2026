using UnityEngine;


//make sure that this component requires a certain component
[RequireComponent(typeof(Collider))]
public class DamageOnOverlap_SelfDestruct : DamageOnOverlap
{
    public GameObject parentObject;

    //public float damageDone;
    //protected Collider mCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        //get collider
        mCollider = GetComponent<Collider>();
        //set this collider as a trigger
        mCollider.isTrigger = true;

        owner = parentObject.GetComponent<Pawn>();

    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        //get other objects health componenet
        HealthComponent otherHealth = other.GetComponent<HealthComponent>();

        if (other.CompareTag("Player") && !other.CompareTag("Projectile"))
        {
            //if it has a healthComp
            if (otherHealth != null)
            {
                //initiate damage on healthComp
                otherHealth.TakeDamage(damageDone, owner);
            }

            //Destroy projectile
            Destroy(parentObject);

        }

    }
}
