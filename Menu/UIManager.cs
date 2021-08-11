using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private ItemUI itemUI;
    [SerializeField] private TabletUI tabletUI;
    [SerializeField] private TerminalUI terminalUI;
    [SerializeField] private DoorlockUI doorlockUI;
    [SerializeField] private AlertUI alertUI;
    [SerializeField] private string[] LocalizedAlerts = new string[2];
    [SerializeField] private GameObject Aim;    // прицел
    [SerializeField] private GameObject VideoPlaying;   // видеоподложка для меню
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject LanguageField;
    [SerializeField] private GameObject GameName;
    [SerializeField] private GameObject SettingsMenuPanel;
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ExitGameButton;
    [SerializeField] private PickupMessageUI pickupMessageUI;
    [SerializeField] private TipMessageUI tipMessageUI;
    [SerializeField] private Slider LoadingSlider;
    [SerializeField] private GameObject LoadingScreen;

    [SerializeField] private Text FPStext;

    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventMission OnMissionStart;


    public enum Alert
    {
        EXIT,
        CHANGES
    }

    public enum GameUIType  // тип интерфейса, который может быть включен
    {
        NONE,
        INVENTORY,
        TABLET,
        TERMINAL,
        DOORLOCK,
        SETTINGSinPAUSE,
        INVENTORYDETAILS
    }

    GameUIType currentGameUIType;

    public GameUIType CurrentGameUIType     // тип текущего интерфейса
    {
        get { return currentGameUIType; }
        private set { currentGameUIType = value; }
    }

   // public object FrameCount { get => frameCount; set => frameCount = value; }

    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        if (GameManager.Instance)
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

        NewGameButton.onClick.AddListener(() => ButtonClicked(1)); // нажали "нью гейм" - обрабатываем
        OnMissionStart.AddListener(GameplayManager.OnMissionStart);   // подписываем менеджера миссий на событие получения новой миссии от закрытия интерфейса
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }


    #region MainMenu Buttions
    void ButtonClicked(int buttonNo)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuButtonClick);
        switch (buttonNo)
        {
            case 1: // новая игра
                Cursor.visible = false;
                GameManager.Instance.StartGame();

                break;

            case 2:

                break;

            default:
                break;
        }
    }

    #endregion

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);

        _mainMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);

        Aim.SetActive(currentState == GameManager.GameState.RUNNING);   // прицел показываем в режиме игры

        VideoPlaying.SetActive(currentState == GameManager.GameState.PREGAME);  // видеоподложку меню показываем только в главном меню
        GameName.SetActive(currentState == GameManager.GameState.PREGAME);  // и название игры тоже

        SettingsMenuPanel.SetActive(currentState == GameManager.GameState.PAUSED);


        SetDummyCameraActive(currentState == GameManager.GameState.PREGAME);



        //   bool showUnitFrame = currentState == GameManager.GameState.RUNNING ||
        //                                                 currentState == GameManager.GameState.PAUSED;
        //  unitFrame.SetActive(showUnitFrame);
    }


    #region ItemUI, tablets, lockpads, terminals
    public void SetItemUI(string itemName, string itemAction, string itemButton)
    {
        // подгрузить из нужного файла слова
        string localizedItemName, localizedItemAction;


        /// по хорошему, это надо сделать один раз - при загрузке локализованных тектов, а не несколько раз - ПЕРЕДЕЛАТЬ
        localizedItemName = LocalizationManager.instance.GetLocalizedValue(itemName);
        localizedItemAction = LocalizationManager.instance.GetLocalizedValue(itemAction);

        itemUI.SetItemName(localizedItemName);
        itemUI.SetItemAction(localizedItemAction);
        itemUI.SetButtonAction(itemButton);
    }


    public void EnableAndFillItemUI(ItemAction itemAction)
    {
        TurnOnItemUI(true);
        // загрузить в ItemUI название итема и действия

        // если ящик или шкаф уже открыт, то сообщение OPEN должно отображаться как CLOSE

        SetItemUI(itemAction.itemDefinition.itemType.ToString(),        // пропишем в табличку itemUI что за элемент
                                        itemAction.itemDefinition.actionType.ToString(),   // что с ним можно  делать
                                          InputManager.instance.KeyBinding.interact.ToString());  // и какую для этого нажать кнопку
    }


    public void TurnOnItemUI(bool turnOn)
    {
        itemUI.gameObject.SetActive(turnOn);
    }

    public void TurnOnTabletUI(bool turnOn) // здесь замутить ивенты: попереключать интерфейсы, подгрузить контент
    {
        if (turnOn)
        {
            SetCurrentGameUIType(GameUIType.TABLET);
        }
        else
        {
            SetCurrentGameUIType(GameUIType.NONE);

            // здесь добавляем событие выключения интерфейса планшета для запуска миссии, если требуется
            if (MissionManager.Instance.isWaitingToActionForStartMission)
                StartMissionEvent();
        }

        tabletUI.gameObject.SetActive(turnOn);
        SwitchFPSMode(turnOn);
    }


    // запуск миссии при закрытии какого-либо интерфейса
    void StartMissionEvent()
    {
        OnMissionStart.Invoke(MissionManager.Instance.MissionWaitingToStart);

        MissionManager.Instance.SetisWaitingToActionForStartMission(false); // обозначаем, что больше не ждём запуск миссии

        // обнуляем ожидаемую миссию
        MissionManager.Instance.SetMissionToStart(null);
    }


    public void SetTabletContent(string name, string content)
    {

        tabletUI.SetTabletName(name);
        tabletUI.SetTabletContent(content);

    }

    public void TurnOnTerminalUI(bool turnOn) //!!!!!!!!!!!!!!!!!!!!!!!!! здесь замутить ивенты: попереключать интерфейсы, подгрузить контент
    {
        if (turnOn)
        {
            SetCurrentGameUIType(GameUIType.TERMINAL);
        }
        else
        {
            SetCurrentGameUIType(GameUIType.NONE);
        }

        // включаем интерфейс терминала
        terminalUI.gameObject.SetActive(turnOn);

        // выключаем ФПС
        SwitchFPSMode(turnOn);

        // теперь надо проверить - если терминал уже был вскрыт, показываем не интерфейс логина, а уже внутренности
        if (turnOn)
            terminalUI.ManageWindows();
    }

    public void SetTerminalLogin(string login, string password, Letter_SO[] letters, string terminalName)
    {
        terminalUI.SetLogin(login, password, letters, terminalName);
    }

    public void TurnOnDoorlockUI(bool turnOn)   /// объединить с методом TurnOnTerminalUI
    {
        if (turnOn)
        {
            SetCurrentGameUIType(GameUIType.DOORLOCK);
        }
        else
        {
            SetCurrentGameUIType(GameUIType.NONE);
        }

        doorlockUI.gameObject.SetActive(turnOn);
        SwitchFPSMode(turnOn);
    }

    public void SetTerminalPin(string pin, GameObject door, string doorName)
    {
        doorlockUI.SetPin(pin, door, doorName);
    }



    private void SwitchFPSMode(bool turnOn)
    {
        Aim.SetActive(!turnOn); // включаем/выключаем прицел
        MouseManager.Instance.SwitchFPS(!turnOn);
        TurnOnItemUI(!turnOn);  // отключаем/включаем подсказку предмета
        Cursor.visible = turnOn;
    }

    public void SetCurrentGameUIType(GameUIType guit)
    {
        CurrentGameUIType = guit;
    }

    public void SetCurrentGameUITypeSettings()
    {
        CurrentGameUIType = GameUIType.SETTINGSinPAUSE; // костылёк. Переключаемся в этот режим при нажатии кнопки Settings в меню паузы
    }

    public void ToggleGameUIOff()   // выклчить все интерфейсы
    {
        switch (currentGameUIType)
        {
            case GameUIType.TABLET:
                TurnOnTabletUI(false);
                break;

            case GameUIType.TERMINAL:
                TurnOnTerminalUI(false);
                break;

            case GameUIType.INVENTORY:
                CharacterInventory.Instance.ToggleInventory();

                break;

            case GameUIType.INVENTORYDETAILS:
                CharacterInventory.Instance.ShowDetailWindow(false);
                break;

            case GameUIType.SETTINGSinPAUSE:    // раздел настроек в меню паузы. По  нажатию Esc - выйти в основное меню паузы
                _pauseMenu.gameObject.SetActive(true);
                SettingsMenu.SetActive(false);
                currentGameUIType = GameUIType.NONE;

                break;

        }

        Aim.SetActive(true); // и показать прицел
    }

    public void LetterHeaderOnClick(string FileKey, GameObject LetterHeader)
    {

        terminalUI.LoadLetterContent(FileKey); // загрузить текст письма
        terminalUI.TurnOffHeaderLighting(); // погасить подсветку на всех письмах
        terminalUI.HighlightHeaderRow(LetterHeader, true);  //подсветить выбранную строку
    }

    #endregion



    #region Alerts
    public void ShowAlert(Alert alert, int currentSelectedLanguage)
    {
        switch (alert)
        {
            case Alert.EXIT:
                // показать окно предупреждения на текущем (сохранённом) языке - currentSelectedLanguage 

                break;

            case Alert.CHANGES:

                // Debug.Log("Show alert --" + currentSelectedLanguage);

                // показать окно предупреждения на выбранном только что языке 
                alertUI.SetLocalizedAlert(LocalizedAlerts[currentSelectedLanguage]);
                alertUI.gameObject.SetActive(true);
                break;
        }
        // вывести окно с нужным текстом
    }

    #endregion


    public void UnloadLevel()
    {
        GameManager.Instance.LoadLevel("MainMenu");
        GameManager.Instance.RestartGame(); // переходим к PREGAME
    }


    public void BackButtonClicked() // нажали кнопку Back в меню настроек. Если это режим главного меню - включить главное меню, если режим игры - включить меню паузы
    {


        switch (GameManager.Instance.CurrentGameState)
        {
            case GameManager.GameState.PREGAME:
                DisableSettingsMenu();
                DisableLanguageField();
                EnableMainMenu();

                break;

            case GameManager.GameState.PAUSED:
                DisableSettingsMenu();
                DisableLanguageField();
                EnablePauseMenu();
                currentGameUIType = GameUIType.NONE;

                break;
        }

    }

    public void DisableSettingsMenu()
    {
        SettingsMenu.SetActive(false);
    }

    public void DisableLanguageField()
    {
        LanguageField.SetActive(false);
    }

    public void EnablePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);

    }

    public void EnableMainMenu()
    {
        _mainMenu.gameObject.SetActive(true);
    }


    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);

#if UNITY_EDITOR
      //  Debug.Log("set dummy camera to " + active);
#endif

    }



    public void ShowItemPickupMessage(string itemName) // покажем, что взял ГГ
    {
        string localizedItemName;
        localizedItemName = LocalizationManager.instance.GetLocalizedValue(itemName);

        pickupMessageUI.gameObject.SetActive(true);

        pickupMessageUI.SetItemMessage(localizedItemName);

        Invoke("HideItemPickupMessage", 2);
    }

    void HideItemPickupMessage()
    {
        pickupMessageUI.gameObject.SetActive(false);
    } 





    public void InitUnitFrame()
    {
       // levelText.text = "1";
      //  healthBar.fillAmount = 1;
    }


    /* обновляем фрейм главного героя - там у него здоровье и текущий уроввень развития
    public void UpdateUnitFrame(HeroController hero)
    {
        int curHealth = hero.GetCurrentHealth();
        int maxHealth = hero.GetMaxHealth();

        healthBar.fillAmount = (float)curHealth / maxHealth;
        levelText.text = hero.GetCurrentLevel().ToString();
    } */

    #region Loading Screen
    public void ShowLoadingScreen(bool isActivate)
    {
        LoadingScreen.SetActive(isActivate);

        if (isActivate) // если активируем экран загрузки, то статус бар должен быть пустой
            LoadingSlider.value = 0f;

    }

    public void SetValueInLoadingSlider(float progress)
    {
        LoadingSlider.value = progress;

    }


    #endregion

    #region ShowTip

    public void ShowTip(string tip) // показываем подсказку на некоторое время
    {
         string localizedTip;
         localizedTip = LocalizationManager.instance.GetLocalizedValue(tip);

         tipMessageUI.gameObject.SetActive(true);

         tipMessageUI.SetTipMessage(localizedTip);

         Invoke("HideTipMessage", 5);
    }


    void HideTipMessage()
    {
        tipMessageUI.gameObject.SetActive(false);
    }

    #endregion

    public void ShowFPS(float fps)
    {
        int i = (int)fps;
        FPStext.text = i.ToString();

    }

}
