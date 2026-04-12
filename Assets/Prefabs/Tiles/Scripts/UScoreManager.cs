using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Unity.Mathematics;

public class UScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text livesText;

    public int livesValue;
    public int scoreValue;

    [Header("Timer")]
    public float delayTime;
    private float countdownTimer;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdownTimer = delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        //check that the Gameplay screen object is active
        if (GameManager.instance.GameplayScreenObject.activeSelf)
        {
  
            //set score text
            scoreText.text = "Score: " + scoreValue.ToString();

            //set lives text
            livesText.text = "Lives: " + livesValue.ToString();


            // random color!
            if(countdownTimer <= 0)
            {
                Color color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                scoreText.color = color;
                livesText.color = color;

                countdownTimer = delayTime;
            }

            countdownTimer -= Time.deltaTime;

        }

    }

    public void SetScoreValue(int value)
    {
        scoreValue = value;
    }

    public void SetLivesValue(int value)
    {
        livesValue = value;
    }

}
