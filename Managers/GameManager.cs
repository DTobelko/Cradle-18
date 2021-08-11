using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System;




public class GameManager : Manager<GameManager>
{
    // на каком уровне сейчас игра
    // загрузка и выгрузка уровней
    // keep track of the game state : PREGAME, RUNNING, PAUSED
    // generate other persistent system


    public GameObject[] SystemPrefabs;  // потенциальные префабы, которые мы можем создать

    public Events.EventGameState OnGameStateChanged;

    private List<GameObject> _instancedSystemPrefabs; // это лист префабов, которые уже созданы, и за которыми надо следить

    private string _currentLevelName = string.Empty;
    List<AsyncOperation> _loadOperations;

   // private static SessionStats CurrentSession;

    // Game states
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        POSTGAME
    }


    GameState currentGameState; // = GameState.PREGAME;

  
        
    public GameState CurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }

     private void Start()
     {

      //CheckDate();

        DontDestroyOnLoad(gameObject); // предохранитель - никогда не выгружать 

        _loadOperations = new List<AsyncOperation>();

        _instancedSystemPrefabs = new List<GameObject>();

        InstantiateSystemPrefabs(); // создаём инстансы префабов системных

        currentGameState = GameState.PREGAME;

        
        // загрузить сохранённый язык
        if (LocalizationManager.instance)
        {
            LocalizationManager.instance.LoadLocalizedText("saved");
        }

        //UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

        

    }

   /* void CheckDate()
    {
        
        if (System.DateTime.Now.ToString("MM/dd/yyyy") != "12/18/2020")
            Application.Quit();
    }*/

    private void Update()
    {
     

        if (currentGameState == GameState.PREGAME)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            // проверяем, если включен интерфейс инвентаря, или планшета, или терминала - то с первым нажатием esc сначала выключаем его
            if (UIManager.Instance.CurrentGameUIType != UIManager.GameUIType.NONE)   // если включен инвентарь или планшет, то выключить и выйти
            {
                UIManager.Instance.ToggleGameUIOff();
                return;
            }

            TogglePause();

        }
    }

    void UpdateState(GameState state)
    {
       GameState _previousGameState = currentGameState;

        currentGameState = state;
      
        switch (currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            default:

                break;

        }

        // dispatch messages
        OnGameStateChanged.Invoke(currentGameState, _previousGameState); // вызываем ивент смены состояния игры
       

        // transition between scenes
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;

        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);

            _instancedSystemPrefabs.Add(prefabInstance);
        }

    }

  /*   void OnLoadOperationComplete(AsyncOperation ao)
      {

          if (_loadOperations.Contains(ao))    // если в списке операций есть операция по загрузке сцены
          {
              _loadOperations.Remove(ao); // то удалим её

              if (_loadOperations.Count == 0)
                  UpdateState(GameState.RUNNING);
          }

        
      }*/

    void OnLoadOperationComplete(AsyncOperation ao)
    {

        
        if (_currentLevelName == "Gameplay")
        {
      
            UpdateState(GameState.RUNNING);
          

            // Instance.InitSessions();
        }
    }

   /* void OnUnloadOperationComplete(AsyncOperation ao)   // выгрузка сцены завершена
    {

    

    }*/

    /*  public void LoadLeveL(string levelName)
      {
          AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive); // при загрузке новой сцены не выгружаем имеющуюся
          if (ao == null)
          {
              Debug.LogError("[GameManager] Unable to load level " + levelName);
              return;
          }
          ao.completed += OnLoadOperationComplete; // подписываемся на событие окончания загрузки сцены
          _loadOperations.Add(ao);
          _currentLevelName = levelName;
      }*/

   public void LoadLevel(string levelName)
    {
        // AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        UIManager.Instance.ShowLoadingScreen(true);
        StartCoroutine(LoadAsynchronously(levelName));

      /*  if (ao == null)
        {

#if UNITY_EDITOR
            Debug.LogError("[GameManager] Unable to load level " + levelName);
#endif

            return;
        }

        ao.completed += OnLoadOperationComplete;*/

        _currentLevelName = levelName;
    }

    // добавляем прогрес-бар к загрузке уровня 25042021
    IEnumerator LoadAsynchronously(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

        while (!ao.isDone)
        {
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            UIManager.Instance.SetValueInLoadingSlider(progress / 2);  // в начале игры загружается сразу несколько сцен: Gameplay, HibernatorsModule
                                                                    // потому.... надо разделить слайдер на две части
            yield return null;
        }

        ao.completed += OnLoadOperationComplete;
    }




    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {

#if UNITY_EDITOR
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
#endif

            return;
        }

      //  ao.completed += OnUnloadOperationComplete;

    }
    
    
    protected void OnDestroy()
    {
        if (_instancedSystemPrefabs == null)
            return;

        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)        // удаляем все системные префабы 
        {
            Destroy(_instancedSystemPrefabs[i]);
        }

        _instancedSystemPrefabs.Clear();
    }
    

    public void StartGame()
    {
        //InitSession();
        StatsManager.InitSession();

        LoadLevel("Gameplay"); //  пока по нажатию на new game загружаем сцену. Потом поменять на показ видеоролика 
    }
    
    public void TogglePause()
    {
        if (currentGameState == GameState.RUNNING)
        {
            UpdateState(GameState.PAUSED);
        }
        else
        {
            UpdateState(GameState.RUNNING);
        }
    }


  /*  private void InitSession()
    {
       // StatsManager.SaveFilePath = Path.Combine(Application.persistentDataPath, "saveGame.json");
        //StatsManager.LoadSessions();
        CurrentSession = new SessionStats();
       // Debug.Log("CurrentSession = " + CurrentSession);
    }*/

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        // implement features for quitting - autosave
        // вывести сообщение - точно ли хотите выйти

        Application.Quit();
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }

    }


    #region Stats



    public void SaveGame()  //   сохранение игры
    {
        // узнаем текущую дату
      //  CurrentSession.SessionDateTime = DateTime.Now.ToLongDateString();

        // ещё нам нужно узнать:
        // до фига всего, а именно:
        // 1. где ГГ
        // 2. что он уже сделал, а именно:
        // 3. какие плашеты прочитаны
        // 4. какие терминалы открыты
        // 5. какие дверные замки открыты
        // 6. запущен ли рекатор
        // 7. что у игрока в инвентаре уже собрано
        // 8. какие миссии пройдены
        // 9. какие миссии активны


      /*CurrentSession.HighestLevel = hero.GetCurrentLevel();
        CurrentSession.WinOrLoss = endGameState;
        CurrentSession.ExperienceGained = hero.GetCurrentXP();
        */


       // StatsManager.sessionStats.Sessions.Add(CurrentSession); // это от Swords and showels  - добавление сессии к списку сессий
        StatsManager.SaveSession();
    }


    // ДОПИЛИТЬ ЭТУ ФУНКЦИЮ ПОД ПРОЧТЕНИЕ ПИСЕМ 

   




    #endregion




}
