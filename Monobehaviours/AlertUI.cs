using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    [SerializeField] private Text alertText;

    public void SetLocalizedAlert(string aText)
    {
        alertText.text = aText;
    }

}
