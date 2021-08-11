using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonHover : MonoBehaviour
{
  
  public AudioSource audioS;
  //private Animator anim;
    
   /* void Start()
    {
        anim = GetComponent<Animator>();
    }*/


    public void onHover()
    {
        if (audioS) audioS.Play(); // играем звук при наведении на кнопку
       // anim.SetBool("hover", true);

   
    }

  /*  public void offHover()
    {
        anim.SetBool("hover", false);
    }*/




}
