using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int score;
    public Text score_gui;
    public Text popup_score;
    public Text popup_scorev2;
    public GameObject bonus300_popup;
    public AudioSource balloon_soundPlayer;
    public AudioSource heart_beat;
    public AudioSource bounce_sound;
    public GameObject[] balloons;
    public GameObject GUI_Hint;
    public GameObject GUI_Hint_GameOver;
    public GameObject GamePlayCanvas;
    public bool gameIsDone;
    public GameObject StartPopup;
    public Transform BalloonRandomPos;
    public GameObject balloon_prefab;
    public Transform balloon_respawn;
    public Image joystick_gui;
    public Image shoot_gui;
    public Toggle ignore_health;

    public int stage = 1;
    public Text stage_gui;


    private void Start()
    {
        stage_gui.text = "Stage: " + stage;
        instance = this;
        Time.timeScale = 0;


        if(BalloonRandomPos != null)
        BalloonRandomPos.gameObject.SetActive(true);
        BalloonRandomPos.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        BalloonRandomPos.position = new Vector3(Random.Range(6.83F, -6.69F), BalloonRandomPos.position.y, BalloonRandomPos.position.z);
        BalloonRandomPos.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * (Random.Range(1,3)), ForceMode2D.Impulse);
       
    }


    public void StartTheGame()
    {
        StartPopup.SetActive(false);
        Time.timeScale = 1;
        BalloonRandomPos.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        GamePlayCanvas.SetActive(true);
        joystick_gui.enabled = true;
        shoot_gui.enabled = true;
    }



    private void Update()
    {





        //KEEP TRACKING ALWAYS IF THE BALLOONS ARE EMPTY, TO DETERMINE IF THE GAMEPLAY IS OVER
        //FIND ALL BALLOONS, BY HAVING GAMEOBJECT WITH TAG, STORE IN ARRAY, DESTROY THE INDEX  IF ALREADY NOT EXISTED,
        //THE INDEX LENGTH, WILL BE REPRESENT AS THE COUNTED NUMBER OF ALL BALLOONS THAT EXISTED IN THE SCENE
        balloons = GameObject.FindGameObjectsWithTag("balloon");

        foreach(GameObject balloon in balloons)
        {
            if(balloon == null)
            {
                Destroy(balloon);
            }
        }

        //READING BALLOONS LENGTH VALUE, WILL BE THE GO SIGNAL TO TELL THE PLAYER WIN ON THE CERTAIN LEVEL 
        if(balloons.Length == 0 && gameIsDone == false && PlayerController.instance.health > 0  && ignore_health.isOn == false ||
            balloons.Length == 0 && ignore_health.isOn == true )
        {
           
            stage_gui.text = "Stage: " + stage;

            gameIsDone = true;
            GUI_Hint.SetActive(true);
            popup_score.text = "SCORE: " + score;
            Time.timeScale = 0;
        }

        Debug.Log("Toogle Value: " + ignore_health.isOn);



        // SAVE THE SCORE OF THE PLAYER



    }




    // PROJECTILE CLEAN UP AFTER HITTING THE TOP COLLIDER
    // NEEDED TO OPTIMIZE THE GAMEPLAY
    void OnTriggerEnter2D(Collider2D projectiles)
    {
        if(projectiles.gameObject.name.Contains("bow-arrow"))
        {
            Destroy(projectiles.gameObject);
        }
    }



    //CALLED WHEN SMALL BALLOON ARE POPUP, NEED TO DEACTIVE IT AFTER IT DISPLAYED FOR HALF SECONDS
    public  IEnumerator ResetPopup()
    {
         yield return new WaitForSeconds(.5f);
        bonus300_popup.SetActive(false);
    }


    public void Resume()
    {
        stage++;
        stage_gui.text = "Stage: " + stage;
       
        gameIsDone = false;

        //SceneManager.LoadScene("TestAB", LoadSceneMode.Single);
        
        
        PlayerController.instance.health = 4;
        ignore_health.isOn = false;
        GameObject clone;
        clone = Instantiate(balloon_prefab, balloon_respawn.position, balloon_respawn.rotation);
        clone.name = "balloon_large-size";
        Time.timeScale = 1;
        GUI_Hint.SetActive(false);
    }


    public void RestartGame()
    {
        gameIsDone = false;
        SceneManager.LoadScene("TestAB", LoadSceneMode.Single);
        Time.timeScale = 1;
        GUI_Hint.SetActive(false);
    }

}
