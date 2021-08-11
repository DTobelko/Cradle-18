using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CradleTime : MonoBehaviour
{
    [SerializeField] private Text DateTime;

    private void Update()
    {
        DateTime.text = "2152/06/30 " + System.DateTime.Now.ToString("HH:mm:ss");
    }
}
