using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorlockUI : MonoBehaviour
{

    string PIN;
    string enteringPIN = "";
    GameObject Door;   // та дверь, которую мы открываем
    string DoorName;

    [SerializeField] PressedBtn[] DoorLockButtons;

    public Events.EventItemUnlocked OnDoorUnlocked; // событие разблокировки двери

    private void Awake()
    {
        OnDoorUnlocked.AddListener(StatsManager.OnDoorOpened); // на событие разблокировки терминала подпишем обработчик - GameManager
    }


    public void SetPin(string pin, GameObject door, string doorName)
    {
        PIN = pin;
        Door = door;
        DoorName = doorName;
    }


    private void Update() // слушать нажатия клавиш - строку из 4-х цифр, затем сравнить с пином
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //    WrongPin();

        // слушаем и запоминаем последовательность из 4-х нажатий цифровых клавиш
        // при этом клавиши могут нажиматься как на клавиатуре, так и на экране мышкой
        // получив 4 кнопки - сравниваем с пин

        if (Input.inputString != "")
        {
            int number;
            bool is_a_number = Int32.TryParse(Input.inputString, out number);
            if (is_a_number && number >= 0 && number < 10)
            {
                CompilePin(number.ToString());
                SoundManager.Instance.PlaySoundEffect(SoundEffect.DoorlockButton);

                // послать сигнал визуальной кнопке чтобы она нажалась
                DoorLockButtons[number].OnPressed();
             }
            
        }

    }

    public void MouseButtonClick(string button)
    {
        CompilePin(button);
        SoundManager.Instance.PlaySoundEffect(SoundEffect.DoorlockButton);

    }

    void CompilePin(string s)
    {
        enteringPIN = enteringPIN + s;

        if (enteringPIN.Length == 4)
            TryToOpen();
    }
    
    public void TryToOpen()
    {
        
        if (enteringPIN == PIN)
        {
            Open();
        }
        else
        {
            WrongPin();
        }
        enteringPIN = "";

        foreach(PressedBtn DoorLockButton in DoorLockButtons)
            DoorLockButton.OnClick();

    }


    void Open()
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffect.AccessGranted);  // заменить звук


        // включить зелёную лампу
        // включить бокс коллайдр на двери
        // выключить UI объект
        // сохранить значение открытой двери
        
        DoorlockManager doorlockManager = Door.GetComponent<DoorlockManager>();
        doorlockManager.UnlockDoor();
        UIManager.Instance.TurnOnDoorlockUI(false);

        OnDoorUnlocked.Invoke(DoorName);

    }

    void WrongPin()
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffect.AccessDenied);
    }
}
