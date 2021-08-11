using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogFile : MonoBehaviour
{
    [SerializeField] private Text FileType;
    [SerializeField] private Text FileName;

    private string FileKey;

    public void SetFileData(string fileName, string fileType, string fileKey)
    {
        FileType.text = fileType;
        FileName.text = fileName;
        FileKey = fileKey;
    }


    public void OnPointerClick()
    {
        CharacterInventory.Instance.LoadFileContent(FileKey);
        CharacterInventory.Instance.TurnOffHighLighting();
        CharacterInventory.Instance.HighlightRow(this.gameObject, true);  //подсветить выбранную строку

        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);
        

    }
}



