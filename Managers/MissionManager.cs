using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MissionEventStruct
{
    public string MissionEventID;
    public MissionEvent_SO MissionEventContainer;
}

public class MissionManager : Manager<MissionManager>
{

    // Этот класс призван управлять задачами - выдавать задачу по сигналу от GameplayManger, завершать задачу при входящем сигнале о наступлении собвтия завершения миссии
            

    public List<MissionEventStruct> MissionEvents;

    private MissionEvent_SO MissionEventDefinition; // описание миссии или события 

    
    public bool isWaitingToActionForStartMission = false; // глобальный признак, что ожидается запуск миссии
    public string MissionWaitingToStart = null; // название миссии, которая ожидает запуска
    public string ActiveMissionName = null; // имя активной миссии (чтобы не выдавать две одинаковые)

    public GameObject missionEventCollidersGO; // ссылка на объект, содержащий все нужные для миссий коллайдеры
    private MissionEventColliders missionEventColliders; // переменная для оперирования с листом коллайдеров


    private void Start()
    {
        missionEventColliders = missionEventCollidersGO.gameObject.GetComponent<MissionEventColliders>();
    }

    public void SetisWaitingToActionForStartMission(bool onOff)
    {
        isWaitingToActionForStartMission = onOff;
    }

    public void SetMissionToStart(string missionName)
    {
        MissionWaitingToStart = missionName;
    }

    public void SetActiveMissionName(string name)
    {
        ActiveMissionName = name;
        
    }

    // выдаём задачу
    public void CreateNewMission(string missionName) // по названию миссии предстоит найти её содержимое в SO и загрузить все данные
    {
        if (ActiveMissionName == missionName) return;  // если попытка запустить уже запущенную миссию, то выходим

        SetActiveMissionName(missionName);
        // прочитать из SO параметры миссии

        MissionEventDefinition = MissionEvents.Find(me => me.MissionEventID == missionName).MissionEventContainer; // читаем описание миссии из листа всех миссий/событий

        CharacterInventory.Instance.OnMissionGet(MissionEventDefinition.MissionNameKey, MissionEventDefinition.MissionDescriptionKey); //  для отображения в UI





        // показать в интерфейсе текст новой задачи
        // ТУТ СНАЧАЛА НАДО РЕШИТЬ, БУДЕТ ЛИ СПИСОК МИССИЙ, МОЖЕТ ЛИ БЫТЬ БОЛЕЕ ОДНОЙ АКТИВНОЙ МИССИИ


        // установить элемент окончания миссии (коллайдр, кнопка и т.п.)
        // активируем коллайдр, который относится к этой миссии
        switch (MissionEventDefinition.MissionEventEnd)    // зацениваем, что должно завершить миссию
        {
            case MissionEventEndType.COLLIDER: // если миссию заканчивает коллайдр, то начнём его слушать
                // А ТАКЖЕ ПРЕДУСМОТРЕТЬ ШАНС, ЧТО МИССИЯ ВЫПОЛНЕНА РАНЕЕ, т.е. например, реактор уже был найден к моменту прочтения сообщения и т.п.

                // для начала коллайдр можно активировать
                // ищем нужный коллайдр
                GameObject MissionEventCollider = missionEventColliders.MEColliders.Find(me => me.ColliderID == MissionEventDefinition.EndMECollider).MissionEventCollider;
                // и включаем
                MissionEventCollider.SetActive(true);


                break;

        }

    }

    /*
    public List<SoundFXDefinition> SoundFX;
    public AudioSource SoundFXSource;

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        AudioClip effect = SoundFX.Find(sfx => sfx.Effect == soundEffect).Clip;
        SoundFXSource.PlayOneShot(effect);
    }*/


    // выполняем задачу
    public void CloseMission(string missionName)    // надо ещё запомнить, что миссия пройдена, записать её в список выполненных миссий, сохранить состояние для сейва
    {
        if (ActiveMissionName != missionName) return;  // если попытка закрыть уже закрытую миссию, то выходим


        // обработать завершение миссии в интерфейсе - перенести миссию в завершённые
        CharacterInventory.Instance.OnMissionClose(MissionEventDefinition.MissionNameKey, MissionEventDefinition.MissionDescriptionKey);

        Debug.Log("Завершена миссия  " + missionName);

        SetActiveMissionName(null);

        // показать в интерфейсе сообщение о выполнении миссии


        // проиграть звук выполнения миссии


        // отключить коллайдр, который завершил миссию, если это коллайдр
        switch (MissionEventDefinition.MissionEventEnd)    // зацениваем, что должно завершить миссию
        {
            case MissionEventEndType.COLLIDER: 
                // ищем нужный коллайдр
                GameObject MissionEventCollider = missionEventColliders.MEColliders.Find(me => me.ColliderID == MissionEventDefinition.EndMECollider).MissionEventCollider;
                // и выключаем
                MissionEventCollider.SetActive(false);


                break;

        }

    }










}


