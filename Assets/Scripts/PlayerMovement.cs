using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    
    
    public float forwardSpeed;

    public Color deathColor;

    
    public float sideWaySpeed = 200f;
    
    public float stamina;

    public Animator playerAnimator;

    public ParticleSystem levelUpParticles;

    public GameObject money;
    private float timer;

    [SerializeField]
    ParticleSystem bloodParticles;

    Vector3 firstTouchPosition, endTouchPosition;

    [HideInInspector]
    public bool canStart = false;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        playerAnimator.SetBool("Run", false);
        canStart = false;
        timer = 0.5f;
        forwardSpeed = PlayerPrefs.GetFloat("ForwardSpeed", 3);
        stamina = PlayerPrefs.GetFloat("Stamina", 40);
        //money.transform.GetComponent<TMP_Text>().text = "+ " + UIManager.instance.moneyToGet.ToString() + " $";
    }

    // Update is called once per frame
    void Update()
    {
        MovementSideWays();
        MovementEditorSide();
        MoveForward();
        MoneySpawner();
    }
    void MovementSideWays()
    {
        if (Input.touchCount > 0 && canStart)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                firstTouchPosition = Camera.main.ScreenToViewportPoint(touch.position);
                
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                endTouchPosition = Camera.main.ScreenToViewportPoint(touch.position);
                
                Vector3 difference = endTouchPosition - firstTouchPosition;

                transform.position += new Vector3(difference.x, 0, 0) * sideWaySpeed * Time.deltaTime;
                firstTouchPosition = endTouchPosition;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                firstTouchPosition = Vector3.zero;
                endTouchPosition = Vector3.zero;
            }
        }
    }
    void MovementEditorSide()
    {
        if (canStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstTouchPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                endTouchPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                Vector3 difference = endTouchPosition - firstTouchPosition;

                transform.position += new Vector3(difference.x, 0, 0) * sideWaySpeed * Time.deltaTime;
                firstTouchPosition = endTouchPosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                firstTouchPosition = Vector3.zero;
                endTouchPosition = Vector3.zero;
            }
        }
    }
    void MoveForward()
    {
        if (canStart)
        {
            transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
            playerAnimator.SetBool("Run", true);
            if (stamina < 10)
            {
                transform.GetChild(0).GetComponent<Renderer>().material.color = Color.Lerp(transform.GetChild(0).GetComponent<Renderer>().material.color, deathColor, 0.02f);
            }
            if (stamina > 0)
            {
                stamina -= Time.deltaTime * 5;
            }
            if(stamina <= 0)
            {
                canStart = false;
                transform.GetComponent<CapsuleCollider>().enabled = false;
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetTrigger("Die");
                PlayerPrefsDeclaration();
                StartCoroutine(GameOverPanel());
            }
        }
    }
    void MoneySpawner()
    {
        if (canStart)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                GameObject spawnedMoney = Instantiate(money, new Vector3(transform.position.x + 0.5f, transform.position.y + 1, transform.position.z), money.transform.rotation);
                UIManager.instance.totalMoney += UIManager.instance.moneyToGet;
                UIManager.instance.totalMoneyText.text = "$" + UIManager.instance.totalMoney.ToString();
                Destroy(spawnedMoney, 2f);
                timer = 0.5f;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            bloodParticles.Play();
            if (forwardSpeed > 2)
                forwardSpeed -= 0.5f;
            if (stamina > 30)
                stamina -= 10;
        }
    }
    void PlayerPrefsDeclaration()
    {
        PlayerPrefs.SetInt("TotalMoney", UIManager.instance.totalMoney);
        PlayerPrefs.SetInt("StaminaLevel", UIManager.instance.staminaLevel);
        PlayerPrefs.SetInt("SpeedLevel", UIManager.instance.speedLevel);
        PlayerPrefs.SetInt("IncomeLevel", UIManager.instance.incomeLevel);
        PlayerPrefs.SetInt("StaminaMoney", UIManager.instance.staminaMoney);
        PlayerPrefs.SetInt("SpeedMoney", UIManager.instance.speedMoney);
        PlayerPrefs.SetInt("IncomeMoney", UIManager.instance.incomeMoney);
        PlayerPrefs.SetInt("MoneyToGet", UIManager.instance.moneyToGet);
        PlayerPrefs.SetFloat("ForwardSpeed", forwardSpeed);
        if(UIManager.instance.progressSlider.value > UIManager.instance.bestDistance)
        {
            PlayerPrefs.SetFloat("BestDistance", UIManager.instance.progressSlider.value);
        }
        //PlayerPrefs.SetFloat("Stamina", stamina);
    }
    IEnumerator GameOverPanel()
    {
        yield return new WaitForSeconds(1f);
        UIManager.instance.gameOverPanel.SetActive(true);
    }
}
