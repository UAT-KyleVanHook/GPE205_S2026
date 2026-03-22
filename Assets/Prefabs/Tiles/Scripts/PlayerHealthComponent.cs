using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{
    public override void Start()
    {

        //currentHealth = maxHealth;

    }

    public override void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void TakeDamage(float amount)
    {

        currentHealth = currentHealth - amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public override void Heal(float amount)
    {

        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public override void IncreaseMaxHealth(float amount)
    {
        //increae max heatlh by the amount
        maxHealth += amount;

        //set the current health to the new max health
        currentHealth = maxHealth;
    }

    public override void Die()
    {
        //play sound clip at point
        AudioSource.PlayClipAtPoint(GameManager.instance.destructionClip, transform.position, 2.0f);

        //deincremnt player lives in GameManager
        GameManager.instance.playerLives -= 1;

        Debug.Log(gameObject.name + " has moved on to a better place.");
        Destroy(gameObject);

    }
}
