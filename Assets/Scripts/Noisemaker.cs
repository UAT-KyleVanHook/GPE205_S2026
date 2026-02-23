using UnityEngine;

public class Noisemaker : MonoBehaviour
{
    public float noiseVolume = 0.0f;
    public float decayRate = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNoiseVolume(float amount)
    {

        noiseVolume = amount;

    }

    public void ResetNoiseAmount()
    {

        noiseVolume = 0;

    }
}
