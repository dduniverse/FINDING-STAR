using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int TotalPoint;
    public int StagePoint;
    public int StageIndex;
    public int health;
    public PlayerMove player;

    public GameObject[] Stages;
    public GameObject UIGameOver;
    public GameObject UIAgainBtn;
    public GameObject UIExitBtn;
    public Image[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    

    void Update()
    {       
        UIPoint.text = (TotalPoint + StagePoint).ToString();
    }
    // Start is called before the first frame update
    public void NextStage()
    {
        //Change Stage
        if (StageIndex < Stages.Length - 1)
        {
            Stages[StageIndex].SetActive(false);
            StageIndex++;
            Stages[StageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (StageIndex + 1);

            if (StageIndex==5)
            {
                UIStage.text = "BOSS";
            }
        }
        else
        {
            // Game clear
            Time.timeScale = 0;
        }
        TotalPoint += StagePoint;
        StagePoint = 0;
    }
    public void HealthDown()
    {
        if (health > 1)
        {         
            health--;
            UIHealth[health].color = new Color(1, 0, 0, 0.3f);
        }
        else
        {   
            //UI에 결과 출력 Debug.Log("죽었습니다.");
            UIHealth[0].color = new Color(1,0,0,0.3f);
            //플레이어가 죽는 모션
            player.OnDie();

            GameObject.Find("Canvas").transform.Find("GameOver").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Again").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Exit").gameObject.SetActive(true);
  

            UIHealth[0].gameObject.SetActive(false);
            UIHealth[1].gameObject.SetActive(false);
            UIHealth[2].gameObject.SetActive(false);
            UIPoint.gameObject.SetActive(false);
            UIStage.gameObject.SetActive(false);
        }        
    }
    
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                PlayerReposition();                
            }            
            HealthDown();
        }
    }
    void PlayerReposition()
    {
        player.transform.position = new Vector3(-12, -5, 0);
        player.VelocityZero();
    }
    
    
    public void Again() {     
        SceneManager.LoadScene(1);    
        Time.timeScale = 1; 
    } 

    public void Exit() {     
        #if UNITY_EDITOR
       UnityEditor.EditorApplication.isPlaying = false;
       #else
       Application.Quit();
       #endif
    }




}
