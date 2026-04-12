using UnityEngine;


public class UCreditsMenu : MonoBehaviour
{
    public float delayTime;
    private float countdownTimer;
    private bool startCountdown = false;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip powerUpClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        countdownTimer = delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountdown == true)
        {
            //every frame deincrement timer
            countdownTimer -= Time.deltaTime;
        }

        if (countdownTimer <= 0)
        {
            startCountdown = false;

            //reset timer
            countdownTimer = delayTime;

            GameManager.instance.ActivateMainMenuScreen();
        }

    }

    public void OnPressMenu()
    {

        if(powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        StartCountDown();

        //GameManager.instance.ActivateMainMenuScreen();
    }

    public void StartCountDown()
    {
        startCountdown = true;
    }
}
