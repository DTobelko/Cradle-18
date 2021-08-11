using UnityEngine;



public class GameplayManager : Manager<GameplayManager>
{

    // Этот класс призван управлять геймплеем - показывать подсказки, выдавать задачи, переключать музыку и свет, показывать кат-сцены и т.п.
    // а также провести игрока по сюжету


    // Первое при загрузке Gameplay - показать кат-сцену





    // сюжет, т.е. все события, квесты, музыка и звуки должны быть описаны в Scriptable Object, чтобы если что, можно было легко изменить
    // каждая миссия - это отдельный scriptable object
    // а также в Scriptable object надо занести подсказки, всплыващие по каким-либо событиям

    // менедрер получает сигнал (событие, event) и отвечает запуском чего-либо - показывает подсказку, выдаёт задачу или заканчивает её, включает свет
    // входные и выходные сигналы и должны быть описаны во входном файле
    // это реализация паттерна Observer

    // Данные миссий загружает класс MissionManager


    // в этот цикл надо попасть после загрузки первого уровня, затем запускаем кат-сцену, затем запускаем первую миссию
    // пока что просто запускаем первую миссию

    // первая миссия - найти реактор появляется после прочтения первого дневника, то есть по событию


    // здесь что--то должно слушать события, которые приводят к началу миссии
    public static void OnMissionStart(string MissionName)    // обрабатываем событие начала миссии
    {
        MissionManager.Instance.CreateNewMission(MissionName);


        // показать сообщение о том, что есть новая задача
        TipsManager.Instance.ShowTip(TipKind.New_Mission);

    }


    public static void OnMissionClose(string MissionName)
    {
        MissionManager.Instance.CloseMission(MissionName);

        SoundManager.Instance.PlaySoundEffect(SoundEffect.MissionComplete);

    }
    


}

