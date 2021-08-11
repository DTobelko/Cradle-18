using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemTypeDefenitions { TERMINAL, DOORLOCK, DIARY, NOTE, DOCUMENT, TABLET, BIGBOX, SMALLBOX, LOCKER, WATERTAP, TOILETBOWL, CHAIR, HIBERNATOR, FUELTANK, MICROCHIP, FLASHLIGHT, SWITCH, ENERGYCELL, PASSCARD }
public enum ItemActioinsType { NONE, USE, UNLOCK, READ, OPEN, CLOSE, PICKUP, SWITCH, TAKE, PUSH }
public enum MissionEventStartCondition {ON_INTERACT, AFTER_INTERACT }

[CreateAssetMenu(fileName = "NewItem", menuName = "New Interactible Item", order = 1)]

public class Item_SO : ScriptableObject
{
    public string itemName = "New Item";
    public string itemDescriptionKey = null;
    public int itemID;
    public ItemTypeDefenitions itemType = ItemTypeDefenitions.TABLET;
    public ItemActioinsType actionType = ItemActioinsType.USE;
    public ItemActioinsType alternativeActionType = ItemActioinsType.NONE;
    public string owner = null; // владелец (например, дневника)

    public Material itemMaterial = null; // для записок - будет материал бумага

    public bool isInteractible = false; // с этим можно взаимодействовать (то есть взламывать, читать, открывать)
    public bool isStorable = false;     // это можно хранить в инвентаре - микросхема, топливный модуль, модуль памяти
    public bool isUnique = false;       // уникальный итем
    public Sprite itemIcon = null;    	// иконка для тех итемов, которые можно носить
    public GameObject model;            // меш

    public SoundEffect ActionSound;     // звук, издаваемый при взаимодействии с объектом

    public bool isStorage = false;  // это что-то, что можно отрыкать и закрывать - шкаф, коробка, гибернатор
	
    public bool destroyOnUse = false;   // при нахождении исчазает - прочитанный дневник, например, модуль памяти и т.п.
    public string contentNameKey = null;    // ключ для заглавия контента (например, статьи)
    public string contentKey = null;    // ключ для контента - для выбора из соответствующего языкового файла
    public bool isUnlockable = false;   // это можно открыть кодом или паролем - терминал, дверной замок
    public string lockPIN;                 // если это дверной замок, то пин-код к нему
    public string login = null;         // если это терминал, то логин доступа
    public string password = null;       // если это терминал, то пароль
    public bool isOpened = false;       // открыто ли уже - это надо будет сохранять в сейвы
    public bool isAnimated = false;     // анимирован ля объект

    public bool isPickable = false;  // можно поднять

    public bool isTriggerForNewMission = false; // запускает ли этот итем новую миссию
    public MissionEventStartCondition missionEventStartCondition = MissionEventStartCondition.ON_INTERACT;  // по умолчанию - запууск миссии при взаимодействии с предметом
    public string StartsNewMission = null;      // если да, то какую


    public Letter_SO[] letters;
   

}
