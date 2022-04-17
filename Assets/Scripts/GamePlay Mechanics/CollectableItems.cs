using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    // NOTE: ITEM TYPE, DETERMINE WHAT ITEM IS FOR
    // 1. GUN UPGRADES (UPGRADE THE PROJECTILE USED BY THE PLAYER)
    // 2. COLLECTABLE ITEM BONUS (INCREMENT TO THE PLAYER SCORE)
    public string itemType; //KEYWORD TO FIND   1. gun    2. collect_item

    private IEnumerator coroutine;



    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name.Contains("Bot-Player Controller"))
        {
            //DETERMIN WHAT THE TYPE OF THIS COLLECTABLE ITEM (GUN OR COLLECT ITEM)
            if(itemType.Contains("gun"))
            {
                EventManager.instance.gunType = 1;
                PlayerController.instance.character_sprite.sprite = PlayerController.instance.character_display[1];
                //RESET THE GUN TYPE AFTER 10 SECONDS OF USE
                EventManager.instance.resetGun_coroutine = EventManager.instance.WaitToResetGun(10.0F); // SETUP TO  10 SECONDS BEFORE TURNING BACK TO DEFAULT GUN
                EventManager.instance.StartCoroutine(EventManager.instance.resetGun_coroutine); // EXECUTE RESETTING
                EventManager.instance.pickup_sounds.Play(); // PLAY SOUND
                Destroy(this.gameObject); // DESTROY THIS OBJECT, AFTER COLLISION

                // NOTE THE GUN TYPE WILL BE EXECUTED ON PLAYERCONTROLLER SCRIPT
                // PLAYERCONTROLLER WILL REFER TO THE EVENTMANAGER TO KNOW THE STATUS OF GUN UPGRADE AND RESET GUN TYPE
            }

            if(itemType.Contains("collect_item"))
            {
                EventManager.instance.pickup_sounds.Play(); // PLAY SOUND

                EventManager.instance.score += 50; // UPDATE SCORE
                EventManager.instance.score_gui.text = "Score: " + EventManager.instance.score; // UPDATE SCORE GUI 

            }
        }
    }


    

}
