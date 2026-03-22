using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float currentHealth;
    //[HideInInspector]
    public float maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {

        //currentHealth = maxHealth;
        
    }

    public virtual void Awake()
    {
      currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float amount)
    {

        currentHealth = currentHealth - amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public virtual void Heal(float amount)
    {

        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public virtual void IncreaseMaxHealth(float amount)
    {
        //increae max heatlh by the amount
        maxHealth += amount;

        //set the current health to the new max health
        currentHealth = maxHealth;
    }

    public virtual void Die()
    {
        //play sound clip at point
        AudioSource.PlayClipAtPoint(GameManager.instance.destructionClip, transform.position, 2.0f);

        Debug.Log(gameObject.name + " has moved on to a better place.");
        Destroy(gameObject);

    }
}
