using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogMission : MonoBehaviour
{
  
    [SerializeField] private Text MissionName;

    private string MissionKey;

    public void SetMissionData(string missionName, string missionKey)
    {
      
        MissionName.text = missionName;
        MissionKey = missionKey;
    }


    public void OnPointerClick()
    {
        
        CharacterInventory.Instance.LoadMissionContent(MissionKey);   // читаем локализованный контент по ключу
        CharacterInventory.Instance.TurnOffHighLighting();
        CharacterInventory.Instance.HighlightRow(this.gameObject, true);  //подсветить выбранную строку

        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);
        

    }
}



