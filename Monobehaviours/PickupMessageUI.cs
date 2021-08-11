using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMessageUI : MonoBehaviour
{
    [SerializeField] private Text Message;
    [SerializeField] private string PickupStringKey = "PlayerGotSomething";

    public void SetItemMessage(string iName)
    {
        Message.text = LocalizationManager.instance.GetLocalizedValue(PickupStringKey)  + " " + iName;
    }
}
