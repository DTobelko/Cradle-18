using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorlockManager : MonoBehaviour
{
    [SerializeField] GameObject GreenLight;
    [SerializeField] GameObject RedLight;

    [SerializeField] GameObject Door;

  //  ItemAction itemAction;

    public void UnlockDoor()
    {
        Door.gameObject.GetComponent<BoxCollider>().enabled = true;
        GreenLight.SetActive(true);
        RedLight.SetActive(false);

      //  itemAction = Door.transform.parent.GetComponentInChildren<ItemAction>();    // получаем ItemAction из сиблинга


        
       /// itemAction.SetOpened(true);
      // сохраняем в сейвах открытую дверь



    }

}
