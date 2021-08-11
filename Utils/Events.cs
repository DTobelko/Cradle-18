using UnityEngine;
using UnityEngine.Events;



public class Events 
{
   [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    [System.Serializable] public class EventFileRead : UnityEvent<string, string> { }

    [System.Serializable] public class EventItemUnlocked : UnityEvent<string> { }
    
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }

    [System.Serializable] public class EventItemPickup : UnityEvent<GameObject> { }

    [System.Serializable] public class EventMission : UnityEvent<string> { }
}
