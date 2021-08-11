using System.Collections.Generic;

[System.Serializable]
public class SessionStats   // статистика сессии
    
{
    public  string SessionDateTime;  // дата  сессии
    //public EndGameState WinOrLoss;

    public List<string> ReadFiles = new List<string>();  // лист содержит названия прочитанных файлов

    public List<string> OpenedTerminals = new List<string>();  // лист содержит названия разблокированных терминалов

    public List<string> OpenedDoors = new List<string>(); // разблокированные двери

    public List<string> ReadLetters = new List<string>();  // лист содержит названия прочитанных писем

   

    /*public int MobsKilled;
    public int ExperienceGained;
    public int WavesCompleted;
    */


    /*
    public SessionStats()
    {
        HighestLevel = 1;
    }*/

}

/*
[System.Serializable]
public enum EndGameState
{
    Win,
    Loss
}*/