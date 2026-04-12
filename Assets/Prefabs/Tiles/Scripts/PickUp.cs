using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUp : MonoBehaviour
{

    public virtual void Awake()
    {
        GameManager.instance.pickUps.Add(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {

        //set our collider to be a trigger
        Collider theCollider = GetComponent<Collider>();    
        theCollider.isTrigger = true;

    }

    // Update is called once per frame
    public virtual void Update()
    {

        //TODO: Anything our powerup does everyframe - spin, bounce, etc.
        
    }

    
    public virtual void OnTriggerEnter(Collider other)
    {


        //base effects

    }

    public virtual void OnDestroy()
    {

        GameManager.instance.pickUps.Remove(this);
        //base effects
    }

}
