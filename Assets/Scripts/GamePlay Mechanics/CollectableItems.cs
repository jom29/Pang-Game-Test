using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    // NOTE: ITEM TYPE, DETERMINE WHAT ITEM IS FOR
    // 1. GUN UPGRADES (UPGRADE THE PROJECTILE USED BY THE PLAYER)
    // 2. COLLECTABLE ITEM BONUS (INCREMENT TO THE PLAYER SCORE)
    public string itemType; //KEYWORD TO FIND   1. gun    2. collect_item
    public int score_value = 50;
    public int health_package = 0;
    private IEnumerator coroutine;



    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name.Contains("Bot-Player Controller")) //IF THE ITEM GUN OR OTHER PICKUPS TRIGGER IN THIS COLLISION. WE KNOW PLAYER WAS COLLIDED WITH OUR OBJECT, NEED TO TRIGGER ACTION
        {
            //DETERMIN WHAT THE TYPE OF THIS COLLECTABLE ITEM (GUN OR COLLECT ITEM)
            if(itemType.Contains("gun"))
            {
                EventManager.instance.gunType = 1; //GUN MARKER
                PlayerController.instance.character_sprite.sprite = PlayerController.instance.character_display[1]; // WHEN TRIGGER, CHANGE THE SPRITE OF THE PLAYER. (THE LOOKS OF THE CHARACTER)
                EventManager.instance.resetGun_coroutine = EventManager.instance.WaitToResetGun(10.0F); // SETUP TO  10 SECONDS BEFORE TURNING BACK TO DEFAULT GUN
                EventManager.instance.StartCoroutine(EventManager.instance.resetGun_coroutine); // EXECUTE RESETTING
                EventManager.instance.pickup_sounds.Play(); // PLAY SOUND
                Destroy(this.gameObject); // DESTROY THIS OBJECT, AFTER COLLISION

                // NOTE THE GUN TYPE WILL BE EXECUTED ON PLAYERCONTROLLER SCRIPT AT RUNTIME
                // PLAYERCONTROLLER WILL REFER TO THE EVENTMANAGER TO KNOW THE STATUS OF GUN UPGRADE AND RESET GUN TYPE
            }



            // FROM THIS METHOD - NOTE THAT WE NEED A PREFAB CALLED ITEM (METHOD APPLIED. INCREASE THE SCORE OF THE PLAYER)
            //ATTACH THIS SCRIPT TO THE PREFAB CALLED ITEM OR WHATEVER YOU MIGHT CALL.
            if(itemType.Contains("collect_item")) // IF THIS PREFAB IS CLASSIFIED AS A PICKUP ITEM ONLY (NOT GUN)
            {
                EventManager.instance.pickup_sounds.Play(); // PLAY SOUND
                

                //INCREMENT HEALTHPACKAGE FROM THIS PREFAB
                if(PlayerController.instance.health <= 4)
                {
                    PlayerController.instance.health++;
                }


                EventManager.instance.score += score_value; // UPDATE SCORE
                EventManager.instance.score_gui.text = "Score: " + EventManager.instance.score; // UPDATE SCORE GUI 
                Destroy(this.gameObject);

            }
        }
    }

}
