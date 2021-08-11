using System;
using UnityEngine;


[System.Serializable]
public struct TipDefinition
{
    public TipKind Kind;
    public string LocalizationTextKey;  // это  ключ локализации подсказки
}

[System.Serializable]
public enum TipKind
{
    Interaction,
    New_Mission
}