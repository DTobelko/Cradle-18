using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonClick : MonoBehaviour
{
    public AudioSource audioS;

    public void OnClick()
    {
        audioS.Play();
    }
}
