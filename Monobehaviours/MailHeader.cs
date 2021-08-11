using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailHeader : MonoBehaviour // это письмо внутри терминала
{


    [SerializeField] private Text Sender;
    [SerializeField] private Text Subject;

    private string SenderKey;
    private string RecieverKey;
    private string SubjectKey;
    private string LetterName;
    
    private string FileKey;

    public void SetHeaderData(string letterName, string sender, string reciever, string subject, string fileKey)
    {
        Sender.text = LocalizationManager.instance.GetLocalizedValue(sender); 
        Subject.text = LocalizationManager.instance.GetLocalizedValue(subject); 
        FileKey = fileKey;

        LetterName = letterName;
        SenderKey = sender;
        RecieverKey = reciever;
        SubjectKey = subject;

    }


    public void OnPointerClick() // нажали на заголовок письма
    {
        UIManager.Instance.LetterHeaderOnClick(FileKey, this.gameObject);

        // запомним прочитанное письмо в лог
        StatsManager.OnLetterRead(LetterName, SenderKey, RecieverKey, SubjectKey, FileKey);

       // terminalUI.LoadLetterContent(FileKey); // загрузить текст письма
       //  terminalUI.TurnOffHeaderLighting(); // погасить подсветку на всех письмах
       //  terminalUI.HighlightHeaderRow(this.gameObject, true);  //подсветить выбранную строку

        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);
    }

}
