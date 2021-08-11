using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsManager : Manager<TipsManager>    // Менеджер подсказок игроку
{
    public List<TipDefinition> Tip;  // лист подсказок
    

    public void ShowTip(TipKind tipKind)
    {

        string CurrentTip = Tip.Find(x => x.Kind == tipKind).LocalizationTextKey;  // получаем текст подсказки и показываем её на экране


        /// звук появления подсказки
        SoundManager.Instance.PlaySoundEffect(SoundEffect.ShowTip);

        UIManager.Instance.ShowTip(CurrentTip);
    }
}
