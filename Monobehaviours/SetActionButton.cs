using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetActionButton : MonoBehaviour
{
    
   // Start is called before the first frame update

    void Start()
    {
        SetTextForActionButton();
    }

    void SetTextForActionButton()
    {
        Text text = GetComponent<Text>();
        text.text = InputManager.instance.KeyBinding.interact.ToString();
    }



}
