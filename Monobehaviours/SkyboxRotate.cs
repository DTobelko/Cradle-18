using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    public GameObject DirectionalLight;
    public float rotateSpeed = 0.4f;
    public GameObject target;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
        DirectionalLight.transform.RotateAround(target.transform.position, Vector3.up, -rotateSpeed * Time.deltaTime);
    }
}
