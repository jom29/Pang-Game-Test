using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement Properties")]
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;



    [Header("Projectile Properties")]
    public GameObject projectile_prefab;                         //PROJECTILE PREFAB
    public float projectile_velocity = 10;                       //PROJECTILE VELOCITY
    public Transform projectile_respawner;                       // USED FOR SINGLE PROJECTILE, PLACED AT THE CENTER OF THE GUN
    public Transform projectile_respawner_left_Direction;        // USED TO RESPAWN GUNTYPE 2 PROJECTILE, PLACED AT THE LEFT SIDE OF THE GUN
    public Transform projectile_respawner_right_Direciton;       // USED TO RESPAWN GUN TYPE 2 PROJECTILE, PLACED AT THE RIGHT SIDE OF THE GUN
    public Vector2 leftDirection;
    public Vector2 rightDirection;
    
    public AudioSource bow_arrow_sound;                          // USED SOUND WHEN FIRING THE BOW-ARROW
    public Sprite[] character_display;                           // USED TO SWITCHING BACK FORTH THE VISUAL LOOK OF THE PLAYER - TO INDICATE WHAT'S UPGRADE IT HAS. ONLY ESTHETIC
    public SpriteRenderer character_sprite;                      // THE ACTUAL SPRITE THAT NEED TO CHANGE FROM THE SCENE


    [Header("Player Health Properties")]                       
    public int health = 4;                                       // PLAYER HEALTH STATUS
    public Image health_gui;                                     //PLAYER HEALTH GRAPHIC DISPLAY CONTAINER
    public Sprite[] health_status;                               //HEALTH STATUS IN INDIVIDUAL SPRITES


    public static PlayerController instance;                    //USED FOR ACCESSING OTHER SCRIPT Methods and Variables
    public RectTransform joystick;                              // USED FOR MOBILE STICK CONTROLLER
    public float horizontal_value;                              
    public float vertical_value;

    public bool joystick_switch;

    void Start()
    {
        //PLAYER 2D RIGIDBODY
        rb = GetComponent<Rigidbody2D>();
        instance = this;
    }

    
    public void Shoot()
    {
        //----------------------------------------------------- SHOOT METHOD SINGLE BOW ARROW ---------------------------------------------------------------------
        if(EventManager.instance.gunType == 0) // MAKE SURE THAT WE CHECK THE EVENTMANAGER SCRIPT AND CHECK GUNTYPE VARIABLE. THIS METHOD IS USED FOR SINGLE BOW-ARROW ONLY
        {
            bow_arrow_sound.Play();

            // Instantiate the projectile at the position and rotation of projectile_respawner (gun mouth)
            Rigidbody2D clone;
            clone = Instantiate(projectile_prefab.GetComponent<Rigidbody2D>(), projectile_respawner.position, projectile_respawner.rotation);

            // Give the cloned object an initial velocity along the current
            if (clone != null)
            {
                clone.AddForce(transform.up * projectile_velocity, ForceMode2D.Impulse);
            }
        }
       
        //--------------------------------------------------------------------------------------------------------------------------------------------








        //------------------------------------------------- SHOOT METHOD TRIPLE BOW ARROW -----------------------------------------------------------------------
        if(EventManager.instance.gunType == 1)
        {
            bow_arrow_sound.Play(); //PLAY GUN SOUND EFFECTS




            // CENTER PROJECTILE
            Rigidbody2D clone;
            Rigidbody2D clone_left;
            Rigidbody2D clone_right;



            clone = Instantiate(projectile_prefab.GetComponent<Rigidbody2D>(), projectile_respawner.position, projectile_respawner.rotation);
            clone_left = Instantiate(projectile_prefab.GetComponent<Rigidbody2D>(), projectile_respawner_left_Direction.position, projectile_respawner_left_Direction.rotation);
            clone_right = Instantiate(projectile_prefab.GetComponent<Rigidbody2D>(), projectile_respawner_right_Direciton.position, projectile_respawner_right_Direciton.rotation);


            // PROJECTILE CENTER - APPLY FORCE
            if (clone != null)
            {
                clone.AddForce(transform.up * projectile_velocity, ForceMode2D.Impulse);
            }

            // PROJECTILE LEFT - APPLY FORCE
            if(clone_left != null)
            {
                clone_left.AddForce(leftDirection * 4, ForceMode2D.Impulse);
                clone_left.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 30);
            }


            // PROJECTILE RIGHT - APPLY FORCE
            if(clone_right != null)
            {
                clone_right.AddForce(rightDirection * 4, ForceMode2D.Impulse);
                clone_right.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, -30);
            }




        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------

    }



    


    void Update()
    {
        if (health == 0)
        {
            health_gui.sprite = health_status[0];
        }


        if (health == 1)
        {
            health_gui.sprite = health_status[1];
        }


        if (health == 2)
        {
            health_gui.sprite = health_status[2];
        }

        if (health == 3)
        {
            health_gui.sprite = health_status[3];
        }




        //WHEN JOYSTICK SWITCH IS ACTIVE
        if (joystick_switch == true)
        {
            //IDLE
            if (joystick.anchoredPosition.x == 86)
            {
                horizontal_value = 0;
            }

            //LEFT MOVE
            if (joystick.anchoredPosition.x < 86)
            {
                horizontal_value = -1;
            }



            //RIGHT MOVE
            if (joystick.anchoredPosition.x > 86)
            {
                horizontal_value = 1;
            }
        }
       


        if(joystick_switch == false)
        {
            if(Input.GetKey(KeyCode.A))
            {
                horizontal_value = -1;
            }

            else if(Input.GetKey(KeyCode.D))
            {
                horizontal_value = 1;
            }

            else
            {
                horizontal_value = 0;
            }
        }

        //----------------------------------------------------INPUT MOVEMENTS---------------------------------------------------------------------
        Vector2 moveInput = new Vector2(horizontal_value, 0);
        moveVelocity = moveInput.normalized * speed;
      
        //---------------------------------------------------------------------------------------------------------------------------------------


     

    }

  



        private void FixedUpdate()
    {
        //MOVEMENT EXECUTION
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        
    }
}
