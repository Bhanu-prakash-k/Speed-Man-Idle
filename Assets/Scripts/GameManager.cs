using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject tapToStartPanel;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tapToStartPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TapToStartButton()
    {
        tapToStartPanel.SetActive(false);
        PlayerMovement.instance.canStart = true;
        PlayerMovement.instance.money.transform.GetComponent<TMP_Text>().text = "+ " + UIManager.instance.moneyToGet + " $";
    }
    public void LoadScene()
    {
        //yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }
}
