using UnityEngine;

public class UMainMenu : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip powerUpClip;

    public float delayTime;
    private float gameplayCountdownTimer;
    private bool startGameplayCountdown = false;
    private bool startSettingsCountdown = false;
    private bool startCreditsCountdown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameplayCountdownTimer = delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        //start
        if (startGameplayCountdown == true)
        {
            //every frame deincrement timer
            gameplayCountdownTimer -= Time.deltaTime;
        }

        if (gameplayCountdownTimer <= 0)
        {
            startGameplayCountdown = false;

            //reset timer
            gameplayCountdownTimer = delayTime;

            GameManager.instance.ActivateGameplayScreen();
        }


        //settings
        if (startSettingsCountdown == true)
        {
            //every frame deincrement timer
            gameplayCountdownTimer -= Time.deltaTime;
        }

        if (gameplayCountdownTimer <= 0)
        {
            startSettingsCountdown = false;

            //reset timer
            gameplayCountdownTimer = delayTime;

            GameManager.instance.ActivateOptionsScreen();
        }


        //credits
        if (startCreditsCountdown == true)
        {
            //every frame deincrement timer
            gameplayCountdownTimer -= Time.deltaTime;
        }

        if (gameplayCountdownTimer <= 0)
        {
            startCreditsCountdown = false;

            //reset timer
            gameplayCountdownTimer = delayTime;

            GameManager.instance.ActivateCreditsScreen();
        }

    }

    //when button is pressed, start the gameplay scene
    public void OnPressStart()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        StartGameplayCountDown();
    }

    public void OnPressSettings()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        StartSettingsCountDown();
    }

    public void OnPressCredits()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        StartCreditsCountDown();
    }

    public void OnPressExit()
    {

        GameManager.instance.GameQuit();
    }



    public void StartGameplayCountDown()
    {
        startGameplayCountdown = true;
    }

    public void StartSettingsCountDown()
    {
        startSettingsCountdown = true;
    }

    public void StartCreditsCountDown()
    {
        startCreditsCountdown = true;
    }
}
