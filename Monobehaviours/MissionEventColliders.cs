using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MissionEventColliders : MonoBehaviour
{
    [SerializeField] public List<MECollidersStruct> MEColliders;
}

[System.Serializable]
public struct MECollidersStruct
{
    public string ColliderID;
    public GameObject MissionEventCollider;
}
