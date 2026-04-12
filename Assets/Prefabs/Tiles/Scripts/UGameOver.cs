using TMPro;
using UnityEngine;

public class UGameOver : MonoBehaviour
{
    public TMP_Text highScoreText;
    private int scoreVariable;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip powerUpClip;

    public float delayTime;
    private float countdownTimer;
    private bool startCountdown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        countdownTimer = delayTime;

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.GameOverScreenObject.activeSelf)
        {

            SetScoreText();
    
        }

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

    public void OnResetScorePress()
    {
        Debug.Log("Reset Score!");

        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
    }

    public void OnPressMenu()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        StartCountDown();
    }

    public void OnPressExit()
    {

        GameManager.instance.GameQuit();
    }

    public void SetScoreText()
    {

        scoreVariable = PlayerPrefs.GetInt("HighScore");

        Debug.Log("High Score: " + scoreVariable);

        //set score text
        highScoreText.text = "Hi-Score: " + scoreVariable.ToString();

    }

    public void StartCountDown()
    {
        startCountdown = true;
    }
}
