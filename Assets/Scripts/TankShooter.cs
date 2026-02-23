using UnityEngine;

public class TankShooter : Shooter
{
    public GameObject bulletPrefab;
    private PawnTank pawn;
    public float fireRate; // how many shots per second we can fire
    private float nextShootTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get TankPawn
        pawn = GetComponent<PawnTank>();
        nextShootTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
    {
        if (Time.time >= nextShootTime)
        {
            AudioSource source = gameObject.GetComponent<AudioSource>();

            if (source != null)
            {
                source.PlayOneShot(GameManager.instance.shootingClip);
            }

            Shoot(pawn.shootForce);

            //update nextShootTime to when the player can shoot again
            nextShootTime = Time.time + (1 / fireRate); // Invert our fireRate to turn (shots/sec to seconds/shot)

        }
    }

    //override for Shoot
    public override void Shoot(float shootForce)
    {
        //Instatnitate bullet
        GameObject bulletObject = Instantiate<GameObject>(bulletPrefab, muzzleLocation.position, muzzleLocation.rotation);

        //push it forward
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(muzzleLocation.forward * shootForce);

        //reset the pawns noise amount
        pawn.noisemaker.ResetNoiseAmount();
    }

    //AI Functions
    public override void AIShoot()
    {
        if (Time.time >= nextShootTime)
        {

            AudioSource source = gameObject.GetComponent<AudioSource>();

            if (source != null)
            {
                source.PlayOneShot(GameManager.instance.shootingClip);
            }

            AIShoot(pawn.shootForce);

            //update nextShootTime to when the player can shoot again
            nextShootTime = Time.time + (1 / fireRate); // Invert our fireRate to turn (shots/sec to seconds/shot)
        }
    }

    //override for Shoot
    public override void AIShoot(float shootForce)
    {
        //Instatnitate bullet
        GameObject bulletObject = Instantiate<GameObject>(bulletPrefab, muzzleLocation.position, muzzleLocation.rotation);

        //push it forward
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(muzzleLocation.forward * shootForce);

        //reset the pawns noise amount
        pawn.noisemaker.ResetNoiseAmount();
    }
}
