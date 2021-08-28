using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class CharacterInventory : Manager<CharacterInventory> //MonoBehaviour
{
    public GameObject InventoryDisplayHolder;

    public GameObject ActiveMissionsHolder; // окно для активных миссий
    public GameObject FinishedMissionsHolder; // окно для завершённых миссий

    public UnityEvent OnToggleInventory;
    //   public static CharacterInventory instance;
    [SerializeField] private Transform FileNamesContainer;  // трансформ для отображения имён файлов
    [SerializeField] private Transform MissionsContainer;  // трансформ для отображения активных миссий
    [SerializeField] private Transform FinishedMissionsContainer;  // трансформ для отображения активных миссий


    [SerializeField] private GameObject FileNameAndTypePrefab;
    [SerializeField] private GameObject MissionPrefab;  // Строка "миссия" для лога
    [SerializeField] private Text MissionText;  // контент миссии

    [SerializeField] private Text ArticleText;  // контент планшета

    [SerializeField] private Transform LettersContainer;  // трансформ для отображения имён писем
    [SerializeField] private GameObject LetterPrefab;
    [SerializeField] private Text LetterText;  // контент письма

    [SerializeField] private int Spaciousness = 10; // вместимость в инвентаре
    [SerializeField] private GameObject[] inventoryDisplaySlots; 


    private List<GameObject> MobileFileRows = new List<GameObject>(); // масссив строк логов

    private List<GameObject> LettersRows = new List<GameObject>(); // масссив строк писем

    private List<GameObject> ActiveMissionRows = new List<GameObject>(); // масссив строк активных миссий
    private List<GameObject> FinishedMissionRows = new List<GameObject>(); // масссив строк завершённых миссий

    private Dictionary<int, ItemAction> itemsInInventory = new Dictionary<int, ItemAction>(); // массив объектов в инвентаре

    int idCount = 0;


    [SerializeField] private GameObject detailWindow;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDefinition;
    [SerializeField] private Image itemImage;

    [SerializeField] private Button ActiveMissionsButton;
    [SerializeField] private Button FinishedMissionsButton;

    [SerializeField] private Sprite YellowButtonSprite;
    [SerializeField] private Sprite BlueButtonSprite;


    private void Start()
    {
        OnToggleInventory.AddListener(ToggleInventory);
        itemsInInventory.Clear();
       // inventoryDisplaySlots = new GameObject[10]; // масссив картинок элементов в инвентаре
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;

        if (UIManager.Instance.CurrentGameUIType != UIManager.GameUIType.NONE && UIManager.Instance.CurrentGameUIType != UIManager.GameUIType.INVENTORY)
            return;

        if (InputManager.instance.KeyDown("Inventory"))
        {
            // отрубить/врубить FPS и мышь
            OnToggleInventory.Invoke();

            

        }

    }



    public void ToggleInventory()
    {
        if (InventoryDisplayHolder.activeSelf == true)
        {
            UIManager.Instance.SetCurrentGameUIType(UIManager.GameUIType.NONE);
            InventoryDisplayHolder.SetActive(false);
            Cursor.visible = false;
            MouseManager.Instance.SwitchFPS(true);
        }
        else
        {
            UIManager.Instance.SetCurrentGameUIType(UIManager.GameUIType.INVENTORY);

            // подгрузить прочитанный  контент в журнал
            // GameManager.Instance.CurrentSession.ReadFiles содержит лист id прочитанных файлов
            // при этом все локализованные тексты в память уже загружены
            //LoadLog();

            InventoryDisplayHolder.SetActive(true);
            Cursor.visible = true;
            MouseManager.Instance.SwitchFPS(false);

        }

        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);

      
    }

    public void ToggleActiveMissions(bool ActiveOrFinished)  // переключаемся между активными и завершёнными миссиями
    {   
        // 1 - Active, 0 - finished
           ActiveMissionsHolder.SetActive(ActiveOrFinished);
           FinishedMissionsHolder.SetActive(!ActiveOrFinished);

        // подстветить жёлтым цветом кнопку выбранного раздела
        MissionButtonsSpriteSwap(ActiveOrFinished);


        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);


    }

    private void MissionButtonsSpriteSwap(bool ActiveOrFinished)   // подстветить жёлтым цветом кнопку выбранного раздела
    {
        
        if (ActiveOrFinished) // если смотрим на активные миссии, то подсветим жёлтым кнопку активных, и синим - кнопку завершнных
        {
            ActiveMissionsButton.GetComponent<Image>().sprite = YellowButtonSprite;
            FinishedMissionsButton.GetComponent<Image>().sprite = BlueButtonSprite;

        }
        else
        {
            ActiveMissionsButton.GetComponent<Image>().sprite = BlueButtonSprite;
            FinishedMissionsButton.GetComponent<Image>().sprite = YellowButtonSprite;
        }
    }


    #region Reading Tablets

    public void OnFileRead(string fileNameKey, string fileKey) // подгружаем прочитанный контент в лог
    {
        // подгружаем прочитанные планшеты в раздел Files - File Panel -Mobile
        // получить лист прочитанных id 
        // отобразить список названий прочитанных статей в левой части - Grid Layout Group
        // первую строку выделить
        // отобразить контент в правом окне

        // не перезагружать список каждый раз, а добавлять новые записи

        AddRowToLog(fileNameKey, fileKey);

        // по-хорошему последний файл надо выводить в самом начале
        // последний выбранный файл - подсветить и вывести детализацию
        // у предудущего выбранного файла подсветку убрать
    }


    private void AddRowToLog(string fileNameKey, string fileKey)
    {
        // добавляемм запись в журнале логов
        GameObject fileNameAndTypeInstance = Instantiate(FileNameAndTypePrefab);  // создаём UI строку с названием гаджета и именем файла (статьи)
        fileNameAndTypeInstance.transform.SetParent(FileNamesContainer, false);   // прикрепляем строку к списку vertical Layout group; здесь false противодействует тому, чтобы scale нового объекта был 0

        fileNameAndTypeInstance.transform.SetSiblingIndex(0);   // новая строка - в самое начало списка


        MobileFileRows.Add(fileNameAndTypeInstance); // запомним сохранённую строку. В массиве строка вставляется в конец


        LogFile logfile = fileNameAndTypeInstance.GetComponent<LogFile>();
        logfile.SetFileData(LocalizationManager.instance.GetLocalizedValue(fileNameKey), ItemTypeDefenitions.TABLET.ToString(), fileKey);  //записываем значения - тип гаджета и название статьи; пока заглушка - всегда планшет


        // теперь надо уубрать подстветку со всех остальных файлов и раскрасить их
        TurnOffHighLighting();

        HighlightRow(fileNameAndTypeInstance, true);  //  подсветим добавленную строку

        // и наконец нужно подгрузить текст в соседнее окно
        LoadFileContent(fileKey);
    }

    public void TurnOffHighLighting()
    {
       for (int i = MobileFileRows.Count - 1; i >= 0; i--)
            {
                MobileFileRows[i].GetComponent<Image>().enabled = MobileFileRows[i].transform.GetSiblingIndex() % 2 == 0 ? true : false; // у чётных строк делаем фон
                HighlightRow(MobileFileRows[i], false);

            }
    }

    public void HighlightRow(GameObject row, bool onOrOff) // подсветить строку
    {
           row.transform.Find("ActiveTableRow").gameObject.SetActive(onOrOff);  // подсвечиваем строку
    }


    public void LoadFileContent(string fileKey)
    {
        ArticleText.text = LocalizationManager.instance.GetLocalizedValue(fileKey);
    }

    public void LoadMissionContent(string fileKey)
    {
        MissionText.text = LocalizationManager.instance.GetLocalizedValue(fileKey);
    }

    private void ClearMissionContent()
    {
        MissionText.text = "";
    }



    #endregion



    #region Reading Letters


    public void OnLetterHeaderClick(GameObject row, string FileKey)
    {
        CharacterInventory.Instance.LoadLetterContent(FileKey);
        CharacterInventory.Instance.TurnOffHighLightingLetters();
        CharacterInventory.Instance.HighlightRow(row, true);  //подсветить выбранную строку
    }


    public void OnLetterRead(string senderKey, string recieverKey, string subjectKey, string fileKey) // подгружаем прочитанный контент в лог
    {
        // подгружаем прочитанные письма в раздел Files - File Panel - Terminal
        // получить лист прочитанных id 
        // отобразить список названий прочитанных писем в левой части - Grid Layout Group
        // первую строку выделить
        // отобразить контент в правом окне

        // не перезагружать список каждый раз, а добавлять новые записи

        AddLetterRowToLog(senderKey, recieverKey, subjectKey, fileKey);

        // по-хорошему последний файл надо выводить в самом начале
        // последний выбранный файл - подсветить и вывести детализацию
        // у предудущего выбранного файла подсветку убрать
    }

    private void AddLetterRowToLog(string senderKey, string recieverKey, string subjectKey, string fileKey)
    {
        
        // добавляемм запись в журнале логов
        GameObject letterInstance = Instantiate(LetterPrefab);  // создаём UI строку 
        letterInstance.transform.SetParent(LettersContainer, false);   // прикрепляем строку к списку vertical Layout group; здесь false противодействует тому, чтобы scale нового объекта был 0

        letterInstance.transform.SetSiblingIndex(0);   // новая строка - в самое начало списка


        LettersRows.Add(letterInstance); // запомним сохранённую строку. В массиве строка вставляется в конец


        LetterHeader header = letterInstance.GetComponent<LetterHeader>();
        header.SetHeaderData(LocalizationManager.instance.GetLocalizedValue(senderKey), LocalizationManager.instance.GetLocalizedValue(recieverKey), LocalizationManager.instance.GetLocalizedValue(subjectKey), fileKey);  


        // теперь надо уубрать подстветку со всех остальных файлов и раскрасить их
        TurnOffHighLightingLetters();

        HighlightRow(letterInstance, true);  //  подсветим добавленную строку

        // и наконец нужно подгрузить текст в соседнее окно
        LoadLetterContent(fileKey);
    }

    public void LoadLetterContent(string fileKey)
    {
        LetterText.text = LocalizationManager.instance.GetLocalizedValue(fileKey);
    }


    public void TurnOffHighLightingLetters()
    {
        //for (int i = LettersRows.Count - 1; i >= 0; i--)
        foreach(GameObject letter in LettersRows)
        {
           HighlightRow(letter, false);

        }
    }

    #endregion


    #region  Missions

    public void OnMissionGet(string fileNameKey, string fileKey) // подгружаем полученную миссию в лог
    {
        AddMissionToLog(fileNameKey, fileKey);
    }

    public void OnMissionClose(string fileNameKey, string fileKey) // перемещаем миссию в завершённые
    {
        
        // для начала очистим окно детального описания
            ClearMissionContent();

        // перемещаем в список завершённых


        // найти строку в массиве ActiveMissionRows и переместить её в FinishedMissionRows

        // ВнИМАНИЕ!!! ДАННАЯ РЕАЛИЗАЦИЯ ТОЛЬКО ДЛЯ СЛУЧАЯ ОДНОЙ АКТИВНОЙ МИССИИ
         GameObject missionInstance = ActiveMissionRows.ElementAt(0);



             // поменять родителя строки на FinishedMissionContainer
             missionInstance.transform.SetParent(FinishedMissionsContainer, false);
             missionInstance.transform.SetSiblingIndex(0); // новая строка - в самое начало списка
             FinishedMissionRows.Add(missionInstance);
        
        // удалить миссию из листа активных
        ActiveMissionRows.Remove(missionInstance);

    }

    private void AddMissionToLog(string fileNameKey, string fileKey)
    {
        // добавляемм запись в журнале миссий
        GameObject missionInstance = Instantiate(MissionPrefab);  // создаём UI строку с названием миссии
        missionInstance.transform.SetParent(MissionsContainer, false);   // прикрепляем строку к списку vertical Layout group; здесь false противодействует тому, чтобы scale нового объекта был 0

        missionInstance.transform.SetSiblingIndex(0);   // новая строка - в самое начало списка



        ActiveMissionRows.Add(missionInstance); // запомним сохранённую строку. В массиве строка вставляется в конец


        LogMission logfile = missionInstance.GetComponent<LogMission>();
        logfile.SetMissionData(LocalizationManager.instance.GetLocalizedValue(fileNameKey),  fileKey);  //записываем значения


        // теперь надо уубрать подстветку со всех остальных файлов и раскрасить их
        TurnOffHighLighting();

        HighlightRow(missionInstance, true);  //  подсветим добавленную строку

        // и наконец нужно подгрузить текст в соседнее окно
        LoadMissionContent(fileKey);
    }

    #endregion

    #region Pick up items

    public void OnItemPickup(ItemAction item)
    {
        if (TryPickUp(item))
        {
            DestroyObject(item.gameObject);

            UIManager.Instance.TurnOnItemUI(false);

            // показать подсказку: что взяли. 
            // "ПОЛУЧЕНО: бак с топливом / микросхема / фонарь"
            UIManager.Instance.ShowItemPickupMessage(item.itemDefinition.itemName);


        }
    }

    void AddItemToInv(ItemAction item)
    {
        itemsInInventory.Add(idCount, item);
        idCount = IncreaseID(idCount);
 
        SoundManager.Instance.PlaySoundEffect(SoundEffect.PutInBag);
        
        FillInventoryDisplay();
    }

    public bool IsItemInInventory(string itemName)  // есть ли такой предмет в инвентаре
    {
        foreach (KeyValuePair<int, ItemAction> ia in itemsInInventory)
        {
            if  (ia.Value.itemDefinition.itemName == itemName)
                return true;

        }
        return false;
    }

    int IncreaseID(int currentID)   // получить следующий id элемента, свобоного в массиве слотов
    {
        int newID = 0;

        for (int itemCount = 0; itemCount <= itemsInInventory.Count-1; itemCount++)  // от одного до количества элементов в массиве итемов
        {
            if (itemsInInventory.ContainsKey(newID)) // если номер уже занят, попробуем следующий
            {
                newID += 1;
            }
            else return newID; // номер свободен, берём его
        }

        return newID;
    }

    void RemoveItem()
    {
        //currentItemStored--;
        //itemsInInventory.Remove

    }

    bool TryPickUp(ItemAction item)
    {
        if (itemsInInventory.Count < Spaciousness) // пока просто проверим, не переполнен ли инвентарь
        {
            AddItemToInv(item);
        
            return true;
        }
        else
        {
            Debug.Log("недостаточно места");
            return false;
        }
        
    }

    void FillInventoryDisplay()
    {
        // поместить картинку объекта в свободный слот с наименьшим номером
        // при использовании объекта - удалить его из слота, при этом НЕ смещая позиции
        // при нажати на элемент в слоте должна повляться инфа по этому элементу, поэтому слот должен знать, что в нём лежит
        // 
        int slotCounter = 0;

        foreach (KeyValuePair<int, ItemAction> ia in itemsInInventory)
        {
            inventoryDisplaySlots[slotCounter].GetComponent<Image>().enabled = true;  
            inventoryDisplaySlots[slotCounter].GetComponent<Image>().sprite = ia.Value.itemDefinition.itemIcon; // тут надо вытащить картинку для хранимого геймобджекта

            slotCounter += 1;
        }
    }



    #endregion


    #region DetailWindow


    public void FillDetailWindow(int id)  // сюда загрузить: название, описание, картинку
    {
        if (!itemsInInventory.ContainsKey(id))
            return;


        string localizedItemName;
        string localizedItemDefinition;

        ShowDetailWindow(true);
        


        localizedItemName = LocalizationManager.instance.GetLocalizedValue(itemsInInventory[id].itemDefinition.itemName);
        localizedItemDefinition = LocalizationManager.instance.GetLocalizedValue(itemsInInventory[id].itemDefinition.itemDescriptionKey);

        itemName.text = localizedItemName;
        itemDefinition.text = localizedItemDefinition;
        itemImage.sprite = itemsInInventory[id].itemDefinition.itemIcon;

    }

    public void ShowDetailWindow(bool enable)
    {
        SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);
        detailWindow.SetActive(enable);

        if(enable)
            UIManager.Instance.SetCurrentGameUIType(UIManager.GameUIType.INVENTORYDETAILS);
        else
            UIManager.Instance.SetCurrentGameUIType(UIManager.GameUIType.INVENTORY);

    }

    #endregion



}
