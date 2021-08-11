using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterHeader : MonoBehaviour       // это заголовок письма в журнале ГГ
{
    [SerializeField] private Text Sender;
    [SerializeField] private Text Reciever;
    [SerializeField] private Text Subject;


    private string FileKey; // ключ для контента

    public void SetHeaderData(string sender, string reciever, string subject, string  fileKey)
    {
        Sender.text = sender;
        Reciever.text = reciever;
        Subject.text = subject;
        FileKey = fileKey;
    }


    public void OnPointerClick()
    {
        CharacterInventory.Instance.OnLetterHeaderClick(this.gameObject, FileKey);
/*
        CharacterInventory.Instance.LoadLetterContent(FileKey);
        CharacterInventory.Instance.TurnOffHighLighting();
        CharacterInventory.Instance.HighlightRow(this.gameObject, true);  //подсветить выбранную строку*/

        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);


    }
}
