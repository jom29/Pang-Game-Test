using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BalloonBehaviour : MonoBehaviour
{
    //NOTE FOR BALLOON TYPE
    //1st_instance - Used for the BIG SIZE BALLOON, 1 PIECE GAMEOBJECT
    //2nd_instance - Used for the MEDIUM SIZE BALLOON, 2 PIECES GAMEOBJECTS
    //3rd_instance - Used for the SMALL SIZE BALLOON, 2 PIECES GAMEOBJECTS
    //BALLOON PREFAB WILL HAVE ATTACHED  SCRIPT TO PUT DISTINCT IDENTITY



    public string balloonType = "1st_instance";
    public GameObject balloon_prefab;
    public float ball_velocity = 25;
    public float balloom_size;
    public bool balloon_trigger = false;

    
    private IEnumerator coroutine;
    public bool ground = false;
    private float ball_magnitude;
    private int hitCount;

    void OnCollisionEnter2D(Collision2D col)
    {
        EventManager.instance.bounce_sound.Play();

        if(col.gameObject.name.Contains("ground"))
        {
            hitCount++;
            if(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 10)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * 2, ForceMode2D.Impulse);
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 5, ForceMode2D.Impulse);
            }
        }


        //WHEN BALLOON HAS SLOW BOUNCE, LET'S APPLY FORCE FROM LEFT AND RIGHT WALL TRIGGER
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 10 && gameObject.GetComponent<Transform>().position.y > -3)
        {
            if (col.gameObject.name.Contains("LeftWall"))
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * 3, ForceMode2D.Impulse);
            }



            if (col.gameObject.name.Contains("RightWall"))
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * -3, ForceMode2D.Impulse);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D projectiles)
    {
        //WHEN HIT BY THE BOW ARROW
        if(projectiles.name.Contains("bow-arrow")) 
        {

            //STRICTLY FOLLOWS 5-15 MINUTES TIME LAPSE BEFORE REPEATING THIS RESPAWN ITEMS.
            //SECONDLY MAKE SURE THAT THE PLAYER IS NOT YET USING THE UPGRADED BOW ARROW. NEED TO BE CLEAR THAT PLAYER IS ON BASIC BOW ARROW ONLY - BEFORE ALLOWING TO RESPAWN THE NEW GUN ITEM
            // REASON: IT DOESN'T MAKE SENSE TO RESPAWN IT WHILE USER ALREADY HAVE THIS.
            //ONLY APPLIED THIS RESPAWN IN THE SMALLEST BALLOON, THE HARDEST TO SHOOT
            if (EventManager.instance.instantiating == false && EventManager.instance.gunType == 0 && balloonType.Contains("last"))
            {
                EventManager.instance.instantiate_items_coroutine = EventManager.instance.WaitToInstantiate_Items(Random.Range(5,15)); // SETUP RANDOMIZE 5 TO 15 SECONDS BEFORE SPAWNING NEW ITEMS
                EventManager.instance.StartCoroutine(EventManager.instance.instantiate_items_coroutine); //EXECUTE NEW RESPAWN TIME OF ITEMS
                EventManager.instance.instantiating = true; // INSTANTIATING MARKER IS ACTIVE NOW.


                if(EventManager.instance.item_prefab[0] != null) //ITEM_PREFAB USED IS AN ARRAY VARIABLE, WHERE CAN STORE MANY ITEMS, WE NEED TO REFER FOR IT'S INDEX VALUE, AND DON'T FORGET TO SETUP IN EVENTMANAGER SCRIPT (PUBLIC VARIABLE SETUP)
                {
                    
                    GameObject gunGO = GameObject.FindGameObjectWithTag("items"); // FIND ITEMS ON THE SCENE


                    if(gunGO == null) // IF NOT FOUND, AND VALIDATED THAT WE DON'T HAVE ANYMORE ITEMS ON THE SCENE, THEN WE CAN NOW INSTANTIATE NEW ITEM
                    {
                        GameObject clone_item;
                        clone_item = Instantiate(EventManager.instance.item_prefab[0], transform.position, transform.rotation);
                        clone_item.name = "BowArrow-UpgradePickup";
                    }
                }
            }











            //STRICTLY FOLLOWS 5-15 MINUTES TIME LAPSE BEFORE REPEATING THIS RESPAWN ITEMS.
            //SECONDLY MAKE SURE THAT THE PLAYER IS NOT YET USING THE UPGRADED BOW ARROW. NEED TO BE CLEAR THAT PLAYER IS ON BASIC BOW ARROW ONLY - BEFORE ALLOWING TO RESPAWN THE NEW GUN ITEM
            // REASON: IT DOESN'T MAKE SENSE TO RESPAWN IT WHILE USER ALREADY HAVE THIS.
            //ONLY APPLIED THIS RESPAWN IN THE SMALLEST BALLOON, THE MEDIUM TO SHOOT
            if (EventManager.instance.instantiating == false && EventManager.instance.gunType == 0 && balloonType.Contains("3rd"))
            {
                EventManager.instance.instantiate_items_coroutine = EventManager.instance.WaitToInstantiate_Items(Random.Range(5, 15)); // SETUP RANDOMIZE 5 TO 15 SECONDS BEFORE SPAWNING NEW ITEMS
                EventManager.instance.StartCoroutine(EventManager.instance.instantiate_items_coroutine); //EXECUTE NEW RESPAWN TIME OF ITEMS
                EventManager.instance.instantiating = true; // INSTANTIATING MARKER IS ACTIVE NOW.

                GameObject gunGO = GameObject.FindGameObjectWithTag("items"); // FIND ITEMS ON THE SCENE


                    if (gunGO == null) // IF NOT FOUND, AND VALIDATED THAT WE DON'T HAVE ANYMORE ITEMS ON THE SCENE, THEN WE CAN NOW INSTANTIATE NEW ITEM
                    {
                       try
                       {
                        GameObject clone_item;
                        clone_item = Instantiate(EventManager.instance.item_prefab[Random.Range(0, EventManager.instance.item_prefab.Length)], transform.position, transform.rotation);
                        clone_item.name = "ScorePackage_prefab";
                       }
                       

                       catch
                       {
                         
                       }
                       
                    }
                
            }


















            //MAKE SURE THAT WE HIT THE FIRST INSTANCE OF BALLOON, BEFORE THE SECOND INSTANCE
            if (balloonType.Contains("1st") && balloon_trigger == false)
            {
             

                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;


                //PLAY POP BALLOON SOUND
                EventManager.instance.balloon_soundPlayer.Play();

                //UPDATE SCORE, PLUS 50
                EventManager.instance.score += 50;
                EventManager.instance.score_gui.text = "Score: " + EventManager.instance.score.ToString();


                this.gameObject.GetComponent<Transform>().localScale = new Vector3(0.2F, 0.2F, 0.2F);
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * ball_velocity, ForceMode2D.Impulse);
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * ball_velocity, ForceMode2D.Impulse);
                this.gameObject.name = "secondInstance_balloon";
                this.gameObject.GetComponent<BalloonBehaviour>().balloonType = "2nd_instance";


                //INSTANTIATE NEW BALLOON AFTER HITTING
                GameObject clone;
                clone = Instantiate(this.gameObject, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody2D>().AddForce(clone.GetComponent<Transform>().right * ball_velocity * -1, ForceMode2D.Impulse);
                clone.GetComponent<Rigidbody2D>().AddForce(clone.GetComponent<Transform>().up * ball_velocity, ForceMode2D.Impulse);

                //RENAME CLONED BALLOON, BIG TO MEDIUM
                clone.name = "secondInstance_balloon";
                clone.GetComponent<BalloonBehaviour>().balloonType = "2nd_instance";

                //DESTROY PROJECTILE AFTER HITTING
                Destroy(projectiles.gameObject);

                //ENABLE THIS TO AVOID OVERLAPPING TRIGGER EVENTS.
                balloon_trigger = true;

                //1 SECOND DELAY TRIGGER DETECTION
                coroutine = WaitAndPrint(1.0f);
                StartCoroutine(coroutine);

            }



            //WHEN HIT BY MEDIUM BALLOON
            if (balloonType.Contains("2nd") && balloon_trigger == false)
            {
                //CHANGE COLOUR
                gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;


                //PLAY POP BALLOON SOUND
                EventManager.instance.balloon_soundPlayer.Play();

                //UPDATE SCORE, PLUS 100
                EventManager.instance.score += 100;
                EventManager.instance.score_gui.text = "Score: " + EventManager.instance.score.ToString();



                this.gameObject.GetComponent<Transform>().localScale = new Vector3(0.1F, 0.1F, 0.1F);
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * ball_velocity, ForceMode2D.Impulse);
                this.gameObject.name = "thirdInstance_balloon";
                this.gameObject.GetComponent<BalloonBehaviour>().balloonType = "3rd_instance";


                //INSTANTIATE NEW BALLOON AFTER HITTING
                GameObject clone;
                clone = Instantiate(this.gameObject, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody2D>().AddForce(clone.GetComponent<Transform>().right * ball_velocity * -1, ForceMode2D.Impulse);
                clone.name = "thirdInstance_balloon";
                clone.GetComponent<BalloonBehaviour>().balloonType = "3rd_instance";

                //DESTROY PROJECTILE AFTER HITTING
                Destroy(projectiles.gameObject);


                //1 SECOND DELAY TRIGGER DETECTION
                balloon_trigger = true;
                coroutine = WaitAndPrint(1.0f);
                StartCoroutine(coroutine);

            }









            //WHEN HIT BY MEDIUM BALLOON
            if (balloonType.Contains("3rd") && balloon_trigger == false)
            {
                //CHANGE COLOUR
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;


                //PLAY POP BALLOON SOUND
                EventManager.instance.balloon_soundPlayer.Play();

                //UPDATE SCORE, PLUS 100
                EventManager.instance.score += 100;
                EventManager.instance.score_gui.text = "Score: " + EventManager.instance.score.ToString();



                this.gameObject.GetComponent<Transform>().localScale = new Vector3(0.05F, 0.05F, 0.05F);
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * ball_velocity, ForceMode2D.Impulse);
                this.gameObject.name = "last_balloon";
                this.gameObject.GetComponent<BalloonBehaviour>().balloonType = "last_instance";


                //INSTANTIATE NEW BALLOON AFTER HITTING
                GameObject clone;
                clone = Instantiate(this.gameObject, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody2D>().AddForce(clone.GetComponent<Transform>().right * ball_velocity * -1, ForceMode2D.Impulse);
                clone.name = "last_balloon";
                clone.GetComponent<BalloonBehaviour>().balloonType = "last_instance";

                //DESTROY PROJECTILE AFTER HITTING
                Destroy(projectiles.gameObject);


                //1 SECOND DELAY TRIGGER DETECTION
                balloon_trigger = true;
                coroutine = WaitAndPrint(1.0f);
                StartCoroutine(coroutine);

            }








            
            //WHEN HIT BY LAST BALLOON
            if (balloonType.Contains("last_instance") && balloon_trigger == false)
            {
            
                //PLAY POP BALLOON SOUND
                EventManager.instance.balloon_soundPlayer.Play();


                //UPDATE SCORE, PLUS 300
                EventManager.instance.score += 300;
                EventManager.instance.score_gui.text = "Score: " + EventManager.instance.score.ToString();
                EventManager.instance.bonus300_popup.SetActive(true);
                EventManager.instance.bonus300_popup.GetComponent<Transform>().position = transform.position;




                //  StartCoroutine(Fade());
                EventManager.instance.StartCoroutine((EventManager.instance.ResetPopup()));


                //DESTROY PROJECTILE AFTER HITTING
                Destroy(projectiles.gameObject);
                Destroy(this.gameObject);


            }
            

        }

      

        if(projectiles.name.Contains("Bot") && PlayerController.instance.health >= 1)
        {
            PlayerController.instance.health--;
            EventManager.instance.heart_beat.Play();
        }


        

        
        //HIT GAME OVER POPUP
        if (PlayerController.instance.health == 0 && EventManager.instance.ignore_health.isOn == false)
        {
            EventManager.instance.GUI_Hint_GameOver.SetActive(true);
            EventManager.instance.heart_beat.Play();
            EventManager.instance.popup_scorev2.text = "SCORE: " + EventManager.instance.score;
            Destroy(gameObject);
        }

     
           
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        balloon_trigger = false;
    }
}
