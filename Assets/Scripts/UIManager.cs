using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text totalMoneyText;

    [HideInInspector]
    public int totalMoney;

    public int moneyToGet = 2;

    [Header("Level Texts")]
    public TMP_Text staminaLevelText;
    public TMP_Text speedLevelText;
    public TMP_Text incomeLevelText;

    [Header("Money Texts")]
    public TMP_Text staminaMoneyText;
    public TMP_Text speedMoneyText;
    public TMP_Text incomeMoneyText;

    [Header("Money Values")]
    public int staminaMoney = 1;
    public int speedMoney = 1;
    public int incomeMoney = 1;

    [Header("Levels")]
    public int staminaLevel = 1;
    public int speedLevel = 1;
    public int incomeLevel = 1;

    public GameObject gameOverPanel;

    public TMP_Text metersText;
    public TMP_Text progressBarMetersText;
    public float meters;

    public Slider progressSlider;

    public float bestDistance;
    public TMP_Text bestDistanceText;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("TotalMoney", totalMoney);
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 20);
        speedMoney = PlayerPrefs.GetInt("SpeedMoney", 1);
        incomeMoney = PlayerPrefs.GetInt("IncomeMoney", 1);
        staminaMoney = PlayerPrefs.GetInt("StaminaMoney", 1);
        staminaLevel = PlayerPrefs.GetInt("StaminaLevel", 1);
        speedLevel = PlayerPrefs.GetInt("SpeedLevel", 1);
        incomeLevel = PlayerPrefs.GetInt("IncomeLevel", 1);
        moneyToGet = PlayerPrefs.GetInt("MoneyToGet", 2);
        bestDistance = PlayerPrefs.GetFloat("BestDistance", 0);
        
        staminaLevelText.text = "Lv " + staminaLevel;
        speedLevelText.text = "Lv " + speedLevel;
        incomeLevelText.text = "Lv " + incomeLevel;

        staminaMoneyText.text = staminaMoney + " $";
        speedMoneyText.text = speedMoney + " $";
        incomeMoneyText.text = incomeMoney + " $";

        bestDistanceText.text = "Best: " + bestDistance;

        totalMoneyText.text = "$" + totalMoney.ToString();
        gameOverPanel.SetActive(false);

        meters = 0;
        metersText.text = meters.ToString() + "M";
        progressBarMetersText.text = meters.ToString("0");

        progressSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.instance.canStart)
        {
            meters += Time.deltaTime * 20;
            progressSlider.value = meters;
            metersText.text = meters.ToString("0") + "M";
            progressBarMetersText.text = progressSlider.value.ToString();
        }
    }
    public void StaminaIncreaseButton()
    {
        if(staminaMoney <= totalMoney)
        {
            PlayerMovement.instance.levelUpParticles.Play();
            PlayerMovement.instance.stamina += 20;
            PlayerPrefs.SetFloat("Stamina", PlayerMovement.instance.stamina);
            totalMoney -= staminaMoney;
            totalMoneyText.text = "$" + totalMoney.ToString();
            staminaLevel += 1;
            staminaLevelText.text = "Lv " + staminaLevel;
            staminaMoney += 3;
            staminaMoneyText.text = staminaMoney + " $";
        }
    }
    public void SpeedIncreaseButton()
    {
        if(speedMoney <= totalMoney)
        {
            PlayerMovement.instance.levelUpParticles.Play();
            if (PlayerMovement.instance.forwardSpeed <= 18)
            {
                PlayerMovement.instance.forwardSpeed += 0.5f;
            }
            else
                PlayerMovement.instance.forwardSpeed = 18;
            
            totalMoney -= speedMoney;
            totalMoneyText.text = "$" + totalMoney.ToString();
            speedLevel += 1;
            speedLevelText.text = "Lv " + speedLevel;
            speedMoney += 3;
            speedMoneyText.text = speedMoney + " $";
        }
    }
    public void IncomeIncreaseButton()
    {
        if(incomeMoney <= totalMoney)
        {
            PlayerMovement.instance.levelUpParticles.Play();
            moneyToGet += 5;
            totalMoney -= incomeMoney;
            totalMoneyText.text = "$" + totalMoney.ToString();
            incomeLevel += 1;
            incomeLevelText.text = "Lv " + incomeLevel;
            incomeMoney += 3;
            incomeMoneyText.text = incomeMoney + " $";
        }
    }
}
