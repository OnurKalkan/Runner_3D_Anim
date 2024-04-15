using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel, inGamePanel, endPanel, winPanel, failPanel;
    public Image shiledIcon, starIcon, magnetIcon;
    public TextMeshProUGUI starCountText, scoreText, winScoreText, shieldCountText, coinCountText;
    GameObject playerParent, gameSounds;
    ScoreManager scoreManager;
    GameManager gameManager;

    private void Awake()
    {
        playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        scoreManager = GetComponent<ScoreManager>();
        gameManager = GetComponent<GameManager>();
        gameSounds = GameObject.Find("Game Sounds");
    }

    void OpenPanel(GameObject panelObject, GameObject secondPanel)
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(false);
        endPanel.SetActive(false);
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        panelObject.SetActive(true);
        if (secondPanel != null)
            secondPanel.SetActive(true);
    }

    private void Start()
    {
        OpenPanel(startPanel, null);
    }

    public void FailPanel()
    {
        OpenPanel(failPanel, endPanel);
        gameSounds.transform.Find("BG Music").GetComponent<AudioSource>().Stop();
        //gameSounds.transform.Find("Fail Music").GetComponent<AudioSource>().Play();
    }

    public void WinPanel()
    {
        OpenPanel(winPanel, endPanel);
        //gameSounds.transform.Find("Win Music").GetComponent<AudioSource>().Play();
    }

    #region Button Functions
    public void TapToStart()
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        playerParent.GetComponent<Move>().speed = 10;
        playerParent.GetComponent<Move>().AnimPlay("Run");
        StartCoroutine(scoreManager.ScoreUpdate());     
        gameManager.levelFinished = false;
        gameSounds.transform.Find("BG Music").GetComponent<AudioSource>().Play();
    }

    public void NextLevel()
    {
        RestartLevel();
        //int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);//levelim 7ydi integera 7 atadim        
        ///////
        //if(currentLevel < 200)
        //{
        //    PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);//local datada CurrentLevel degerini 1 arttırdım 8 oldu
        //    //SceneManager.LoadScene(currentLevel);//private currentLevel indexine sahip sahneyi cagirdim yani index no 7 levelini cagirdim
        //    ////////////////////
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//sonraki sahnenin index numarasina gore leveli cagiriyorum
        //}
        //else if(currentLevel >= 200)
        //{
        //    PlayerPrefs.SetInt("CurrentLevel", 0);//200uncu levele gelince datayi sifirla
        //    SceneManager.LoadScene(0);//index no 0'i cagir
        //}
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//index numarasi bu olan, oldugum/kaybettigim sahneyi tekrar baslat
    }
    #endregion

}
