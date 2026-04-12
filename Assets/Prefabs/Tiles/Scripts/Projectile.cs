using UnityEngine;
using static Unity.VisualScripting.Member;

public class Projectile : MonoBehaviour
{
    public float lifespan;
    public AudioClip hitClip;

    protected Collider mCollider;


    public void Start()
    {
        mCollider = GetComponent<Collider>();
        mCollider.isTrigger = true;


        //destroy this object
        Destroy(gameObject, lifespan);
    }

    public void OnTriggerEnter(Collider other)
    {
        //AudioSource.PlayClipAtPoint(hitClip, transform.position);

        if(hitClip != null)
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position);
        }

    }

}
