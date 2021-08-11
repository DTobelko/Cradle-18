using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShield : MonoBehaviour
{
    [SerializeField] private string ForceShieldColorCard; // цвет карты, открывающий это силовое поле
    

    private void OnTriggerEnter(Collider other)
    {
        

        // игрок пришёл к двери - проверим, есть ли у него карта нужного цвета и если есть - нужно его пропустить

        if (other.gameObject.tag == "Player")
        {
            

            if (CharacterInventory.Instance.IsItemInInventory(ForceShieldColorCard)) 
            {
                OpenForceShield();
            }

        }


    }


    void OpenForceShield()
    {
        this.gameObject.transform.Find("Shield").gameObject.SetActive(false);
    }

}
