using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum Language { ENGLISH, RUSSIAN }

public class SwitchLanguage : MonoBehaviour
{
    [SerializeField] private Text SelectedLanguage;
  //  public Language language = Language.ENGLISH;

    [SerializeField] private string[] Languages = new string[2];
    private int savedLanguage, currentSelectedLanguage;

    // получить сохранённый в playerprefs язык
    // если его нет - по умолчанию вывестти 0 - English
    // по кнопке apply - загрузить нужный файл
    // и сохранить выбранный язык в player prefs

    private void Start()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            savedLanguage = PlayerPrefs.GetInt("Language"); //получаем код сохранного языка 
         
        }
        else
        {
            savedLanguage = 0;
        }

        currentSelectedLanguage = savedLanguage;
        SelectedLanguage.text = Languages[savedLanguage]; // отображаем его название

    }


    public void ApplyLanguage()
    {
      //  Debug.Log("попытка поменять язык " + savedLanguage + " на  " + currentSelectedLanguage);

        if (currentSelectedLanguage != savedLanguage)
        {
            PlayerPrefs.SetInt("Language", currentSelectedLanguage);
            savedLanguage = currentSelectedLanguage;

            // UIManager.Instance.ShowAlert(UIManager.Alert.CHANGES, currentSelectedLanguage);    // покажем сообщение, что изменения вступят в силу после перезагрузки

            // попробуем перезагрузить всё онлайн
            var allLocalizedElements = Resources.FindObjectsOfTypeAll(typeof(LocalizedText)); // находим все локализованные объекты
            LocalizationManager.instance.LoadLocalizedText("saved"); // сначала загрузим файл

          //  Debug.Log("применяем язык " + currentSelectedLanguage);

            foreach (LocalizedText lt in allLocalizedElements)
            {
                lt.LoadLocalization();
            }

        }
    }


    

    // процедура меняет выбранный язык в зависимости от нажатой кнопки
    public void ChangeLanguage(bool  pressedLeft) // true - нажали налево, false - направо
    {
        // переключить код языка
       

        if (pressedLeft) // переключиться на предыдущий язык в списке
        {
            currentSelectedLanguage--;
            if (currentSelectedLanguage < 0)
                currentSelectedLanguage = Languages.Length - 1;
            
        }
        else // переключиться на следующий язык в списке
        {
            currentSelectedLanguage++;
            if (currentSelectedLanguage > Languages.Length -1)
                currentSelectedLanguage = 0;

        }

        // отобразить в интерфейсе, запомнить выбранный код
        SelectedLanguage.text = Languages[currentSelectedLanguage];
        

    }
}
