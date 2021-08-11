using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
 
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemAction;
    [SerializeField] private Text itemButton;

    public void SetItemName(string iName)
    {
        itemName.text = iName;
    }

    public void SetItemAction(string iAction)
    {
        itemAction.text = iAction;
    }

    public void SetButtonAction(string iButton)
    {
        itemButton.text = iButton;
    }
   

}
