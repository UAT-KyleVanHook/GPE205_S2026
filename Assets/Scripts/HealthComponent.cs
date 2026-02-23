using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [HideInInspector] public float currentHealth;

    public float maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {

        currentHealth = currentHealth - amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public void Heal(float amount)
    {

        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public void Die()
    {

        Debug.Log(gameObject.name + " has moved on to a better place.");
        Destroy(gameObject);

    }
}
