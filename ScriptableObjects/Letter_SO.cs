using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum LetterTypeDefenitions { SENDER, SUBJECT, CONTENT }


[CreateAssetMenu(fileName = "NewItem", menuName = "New Letter", order = 1)]

public class Letter_SO : ScriptableObject
{
    public string itemName = "New Letter";
    public int itemID;
   // public LetterTypeDefenitions letterType = LetterTypeDefenitions.SENDER;
    
    public string terminalOwner = null; // терминал

    public string senderKey = null;    // ключ отправителя
    public string recieverKey = null;  // ключ получателя
    public string subjectKey = null;    // ключ для темы
    public string contentKey = null;    // ключ для тела письма



}
