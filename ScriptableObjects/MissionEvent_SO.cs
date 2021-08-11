using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MissionEventType { MISSION, EVENT, CUTSCENE }
public enum MissionEventBeginType { CUTSCENEENDED, EVENTENDED, MISSIONENDED, ITEMUSED  }  // возможные триггеры начала новой миссии или события
public enum MissionEventEndType { TERMINALBUTTON, ITEMINSERTED, COLLIDER } // возможные триггеры окончания миссии

[CreateAssetMenu(fileName = "NewMissionEvent", menuName = "New Mission or Event", order = 1)]

public class MissionEvent_SO : ScriptableObject
{
    public string MissionEventName = "New MissionEvent";
    public string MissionEventDescriptionKey = null; // описание миссии для ежедневника
    public int MEID;    // идентификатор, если нужен
    public MissionEventType itemType = MissionEventType.MISSION;    // п умолчанию тип - миссия


    // миссия или событие должны иметь:
    // входные события (по которым они наступают)
    // выходные события (по которым они завершаются)
    // и кучу всего остального: коллайдр или кнопка завершения, ...

    public MissionEventBeginType MissionEventBegin = MissionEventBeginType.CUTSCENEENDED; // по умолчанию начало новой миссии - завершение кат сцены
    public MissionEventEndType MissionEventEnd = MissionEventEndType.ITEMINSERTED;  // по умолчанию конец миссии - вставка итема в слот


    public string EndMECollider = null; // коллайдр завершения миссии (ссылка в виде строки на элемент листа объекта MissionEventColliders сцены Gameplay

}