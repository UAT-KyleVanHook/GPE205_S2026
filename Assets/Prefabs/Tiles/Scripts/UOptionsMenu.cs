using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UOptionsMenu : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public Slider mainVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle toggleButton;
    public InputField inputField;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip powerUpClip;

    public float delayTime;
    private float countdownTimer;
    private bool startCountdown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //onMainVolumeSliderChange();
        //onSFXVolumeSliderChange();
        //onMusicVolumeSliderChange();

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

    public void onMainVolumeSliderChange()
    {
        //start with the current slider value
        float newVolume = mainVolumeSlider.value;

        if(newVolume <= 0)
        {
            //if we are at zero, set volume to lowest value
            newVolume = -80;
        }
        else
        {

            newVolume = Mathf.Log10(newVolume);

            //make it in the 0-20fb range (instead of 1-2db range)
            newVolume = newVolume * 20;
        }

        mainAudioMixer.SetFloat("MainVolume", newVolume);


    }

    public void onSFXVolumeSliderChange()
    {
        //start with the current slider value
        float newVolume = sfxVolumeSlider.value;

        if (newVolume <= 0)
        {
            //if we are at zero, set volume to lowest value
            newVolume = -80;
        }
        else
        {

            newVolume = Mathf.Log10(newVolume);

            //make it in the 0-20fb range (instead of 1-2db range)
            newVolume = newVolume * 20;
        }

        mainAudioMixer.SetFloat("SFXVolume", newVolume);


    }

    public void onMusicVolumeSliderChange()
    {
        //start with the current slider value
        float newVolume = musicVolumeSlider.value;

        if (newVolume <= 0)
        {
            //if we are at zero, set volume to lowest value
            newVolume = -80;
        }
        else
        {

            newVolume = Mathf.Log10(newVolume);

            //make it in the 0-20fb range (instead of 1-2db range)
            newVolume = newVolume * 20;
        }

        mainAudioMixer.SetFloat("MusicVolume", newVolume);


    }

    public void OnTogglePressed(bool toggleVlaue)
    {

        if (!toggleVlaue)
        {
         

            GameManager.instance.bIsSplitScreen = toggleVlaue;
            Debug.Log("Is Split-Screen On: " + GameManager.instance.bIsSplitScreen.ToString());
            Debug.Log(GameManager.instance.bIsSplitScreen);
        }
        else
        {

            GameManager.instance.bIsSplitScreen = toggleVlaue;
            Debug.Log("Is Split-Screen On: " + GameManager.instance.bIsSplitScreen.ToString());
            Debug.Log(GameManager.instance.bIsSplitScreen);
        }

    }




    //on press go back to main menu
    public void OnPressMenu()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        StartCountDown();
    }



    public void OnRandomPressed()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        Debug.Log("Map Generation set to: Random");

        GameManager.instance.level.mapGenerator.randomType = RandomType.Random;
    }

    public void OnMOTDPressed()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        Debug.Log("Map Generation set to: Map of the Day");

        GameManager.instance.level.mapGenerator.randomType = RandomType.MapOfTheDay;
    }

    public void OnSeededPressed()
    {
        if (powerUpClip != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        Debug.Log("Map Generation set to: Seeded");

        GameManager.instance.level.mapGenerator.randomType = RandomType.Seeded;
    }

    public void OnEditEnded(string  userSeed)
    {


        int parsedSeed = userSeed.GetHashCode();

        Debug.Log(parsedSeed);

        GameManager.instance.level.mapGenerator.seed = parsedSeed;

    }



    public void StartCountDown()
    {
        startCountdown = true;
    }


}
