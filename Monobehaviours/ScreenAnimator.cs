using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAnimator : MonoBehaviour
{

    public List<Material> materials;
    public int framesPerSecond = 5;

 

    // Update is called once per frame
    void Update()
    {
        int index =  (int) (Time.time * framesPerSecond) % materials.Count;

        GetComponent<Renderer>().material = materials[index];
     
             

    }
}
