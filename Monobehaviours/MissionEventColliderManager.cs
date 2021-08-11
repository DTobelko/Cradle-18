using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEventColliderManager : MonoBehaviour
{

    private MissionEventColliders missionEventColliders; // переменная для оперирования с листом коллайдеров
    public string missionEvent;  // миссия, к которой относится этот коллайдр
    public Events.EventMission OnMissionStop;

    void Awake()
    {
       OnMissionStop.AddListener(GameplayManager.OnMissionClose);  // подписываемм GamePlayManager на событие входа ГГ в коллайдр завершения миссии
 
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            OnMissionStop.Invoke(missionEvent);


        }


        // завершить миссию, которая связана с этим коллайдером

    }


}
