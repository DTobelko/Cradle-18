using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class StatsManager
{
    public static SessionStats sessionStats = new SessionStats();
    public static string SaveFilePath { get; set; }

    private static SessionStats CurrentSession;

    public static void SaveSession()
    {

        CurrentSession.SessionDateTime = DateTime.Now.ToLongDateString();

        string json = JsonUtility.ToJson(sessionStats);
        File.WriteAllText(SaveFilePath, json);
    }

    public static void LoadSession()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            sessionStats = JsonUtility.FromJson<SessionStats>(json);
        }
    }


    public static void InitSession()
    {

        CurrentSession = new SessionStats();
   
    }


    public static void OnFileRead(string fileNameKey, string fileKey)    // обрабатываем событие прочтения файла
    {
        /// проверить, не был ли данный файл уже прочитан ранее
        if (CurrentSession.ReadFiles.Contains(fileNameKey))
            return;

        // если нет - добавляем
        CurrentSession.ReadFiles.Add(fileNameKey);   // запоминаем, что в данной сессии прочитан этот файл
        CharacterInventory.Instance.OnFileRead(fileNameKey, fileKey); // вызываем ивент для отображения в UI
    }

    public static void OnLetterRead(string LetterName, string senderKey, string recieverKey, string subjectKey, string fileKey)    // обрабатываем событие прочтения файла
    {
        /// проверить, не был ли данный файл уже прочитан ранее
        if (CurrentSession.ReadLetters.Contains(LetterName))
            return;

        // если нет - добавляем
        CurrentSession.ReadLetters.Add(LetterName);   // запоминаем, что в данной сессии прочитано письмо
        CharacterInventory.Instance.OnLetterRead(senderKey, recieverKey, subjectKey, fileKey); // вызываем ивент для отображения в UI
    }


    public static int GetReadFilesCount()
    {
        return CurrentSession.ReadFiles.Count;
    }

    public static bool TerminalIsUnlocked(string terminalName)
    {
        return (CurrentSession.OpenedTerminals.Contains(terminalName));
    }


    public static void OnTerminalLoggedIn(string TerminalName)    /// обрабатываем событтие открытия терминала
    {
        if (CurrentSession.OpenedTerminals.Contains(TerminalName))
            return;
        CurrentSession.OpenedTerminals.Add(TerminalName);   // запомним, что в данной сессии открыт данный терминал -- его не надо больше разблокировать
    }

    public static void OnDoorOpened(string DoorName)
    {
        if (CurrentSession.OpenedDoors.Contains(DoorName))
            return;
        CurrentSession.OpenedDoors.Add(DoorName);   // запомним, что в данной сессии открыт данный терминал -- его не надо больше разблокировать
    }

    public static bool DoorIsOpened(string DoorName)
    {
        return (CurrentSession.OpenedDoors.Contains(DoorName));
    }




}