using UnityEngine;

[System.Serializable]
public struct SoundFXDefinition 
{
    public SoundEffect Effect;
    public AudioClip Clip;
}

[System.Serializable]
public enum SoundEffect
{
    ShowGameInterface,
    PushMenuButton,
    MenuButtonClick,
    MenuSelectionClick,
    LockerOpen,
    LockerClose,
    BigBoxOpen,
    BigBoxClose,
    SmallBoxOpen,
    SmallBoxClose,
    HibernatorOpen,
    HibernatorClose,
    WrongPassword,
    DoorlockButton,
    AccessDenied,
    AccessGranted,
    PutInBag,
    Flashlight,
    Toilet,
    WaterTap,
    UIHover,
    UIClickElectronic,
    OSLoad,
    TerminalButtonHover,
    OSLogon,
    TabClickSound,
    WindowSwitch,
    WindowWoosh,
    ShowTip,
    MissionComplete
}
