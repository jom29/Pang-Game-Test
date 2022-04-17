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
    public GameObject projectile_prefab;
    public float projectile_velocity = 10;
    public Transform projectile_respawner;
    public AudioSource bow_arrow_sound;

    public int health = 4;
    public Image health_gui;
    public Sprite[] health_status;


    public static PlayerController instance;
    public RectTransform joystick;
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
        //----------------------------------------------------- SHOOT METHOD ---------------------------------------------------------------------

        bow_arrow_sound.Play();

        // Instantiate the projectile at the position and rotation of projectile_respawner (gun mouth)
        Rigidbody2D clone;
        clone = Instantiate(projectile_prefab.GetComponent<Rigidbody2D>(), projectile_respawner.position, projectile_respawner.rotation);

        // Give the cloned object an initial velocity along the current
        if (clone != null)
        {
            clone.AddForce(transform.up * projectile_velocity, ForceMode2D.Impulse);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------

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
