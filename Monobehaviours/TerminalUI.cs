using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalUI : MonoBehaviour
{
    [SerializeField] private Text terminalLogin;
    [SerializeField] private InputField terminalPassword;

    [SerializeField] private GameObject LogonScreen;
    [SerializeField] private GameObject TerminalScreen;

    [SerializeField] private Transform LettersContainer;
    [SerializeField] private GameObject LetterPrefab;

    [SerializeField] private Text LetterText;

    public Events.EventItemUnlocked OnTerminalUnlocked; // событие разблокировки терминала

    string TerminalName;
    string password;
    Letter_SO[] Letters;
    bool loggedIn = false;

    private List<GameObject> LetterHeaderRows = new List<GameObject>(); // масссив строк логов


    private void Awake()
    {
        OnTerminalUnlocked.AddListener(StatsManager.OnTerminalLoggedIn); // на событие разблокировки терминала подпишем обработчик - GameManager
    }


    public void SetLogin(string login, string pwd, Letter_SO[] letters, string terminalName)
    {
        TerminalName = terminalName;
        terminalLogin.text = login;
        terminalPassword.text = "";
        password = pwd;
        Letters = letters; // запомним, чей терминал вскрываем

        loggedIn = StatsManager.TerminalIsUnlocked(TerminalName);   // если в этот терминал уже заходили, то поставим этот признак
    }

    public void SetTerminalContent()
    {
        // подгрузить письма и элементы управления:
        // 1. найти все письма, прикреплённые к этому терминалу
        // 2. загрузить каждое письмо в элемент MailStrings
        // 3. курсор поставить на первое письмо, сразу добавить его в лог
        // 4. по нажатию на письмо:
        //  4.1. подсветить его
        //  4.2. загрузить текст письма    
        //  4.3. добавить письмо в лог

        //1. получить из объекта список всех писем

        // 0. Сначала очистим всё от старых писем
        LetterHeaderRows.Clear();
        foreach (Transform child in LettersContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
        LetterText.text = "";



        foreach (Letter_SO letter in Letters)
        {
            AddLetter(letter);
        }
    
    }


    public void ManageWindows() // проверить - если терминал уже был вскрыт, показываем не интерфейс логина, а уже внутренности
    {
        if (StatsManager.TerminalIsUnlocked(TerminalName))
        {
            LogonScreen.SetActive(false);
            TerminalScreen.SetActive(true);
            SetTerminalContent();
        }
        else
        {
            LogonScreen.SetActive(true);
            TerminalScreen.SetActive(false);
        }
    }


    private void AddLetter(Letter_SO letter)
    {
            GameObject LetterInstance = Instantiate(LetterPrefab);  // создаём UI строку с заголовком письма
            LetterInstance.transform.SetParent(LettersContainer, false);   // прикрепляем строку к списку vertical Layout group;

            MailHeader mailHeader = LetterInstance.GetComponent<MailHeader>();
            mailHeader.SetHeaderData(letter.itemName, letter.senderKey, letter.recieverKey, letter.subjectKey, letter.contentKey);   // загружаем данные письма

        
        LetterHeaderRows.Add(LetterInstance); // запомним сохранённую строку. 

        /*
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
        }*/


    }


    public void LoadLetterContent(string FileKey)
    {
        LetterText.text = LocalizationManager.instance.GetLocalizedValue(FileKey).Replace("\\t", "\t").Replace("\\n", "\n");
    }

    public void TurnOffHeaderLighting()
    {
        //for (int i = MobileFileRows.Count - 1; i >= 0; i--)
        foreach(GameObject headerRow in LetterHeaderRows)
        {

            HighlightHeaderRow(headerRow, false);

        }
    }

    public void HighlightHeaderRow(GameObject mailHeader, bool onOrOff)
    {
        mailHeader.transform.Find("ActiveTableRow").gameObject.SetActive(onOrOff);  // подсвечиваем строку
    }

    private void Update()
    {
        if (loggedIn)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
            TryLogin();
    }

    public void TryLogin()
    {
        // сравнить введённый логи с тем, что пришло из itemAction

        if (terminalPassword.text == password)
        {
            loggedIn = true;
            Login();
        }
        else
        {
            WrongPassword();
        }

    }


    void Login()
    {
        OnTerminalUnlocked.Invoke(TerminalName); // пуляем событие открытия терминала

        LogonScreen.SetActive(false);
        TerminalScreen.SetActive(true);
        SoundManager.Instance.PlaySoundEffect(SoundEffect.OSLogon);

        // подгрузить контент терминала
        SetTerminalContent();

    }

    void WrongPassword()
    {
        terminalPassword.text = "";
        SoundManager.Instance.PlaySoundEffect(SoundEffect.WrongPassword);
    }
}
