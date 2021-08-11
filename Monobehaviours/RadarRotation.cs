using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float frequency = 1f;
    public float addedAngle = 1f;

    void Start()
    {
        InvokeRepeating("Rotate", 0f, frequency);
    }


    private void Rotate()
    {
        //float newY = transform.rotation.y + 1f;
        transform.Rotate(0f, addedAngle, 0f);
    }

}
