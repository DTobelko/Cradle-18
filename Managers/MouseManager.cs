using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;

// показать курсор в режиме главного меню и в режиме паузы
// режиме игры - показать курсор в режиме показа инвентаря

public class MouseManager : Manager<MouseManager> //onoBehaviour
{
    public Texture2D pointer;
    private GameObject FPS;
    private bool shouldRotateView = true;   // вращать ли камерой
    public LayerMask clickableLayer;
    public bool isHovering = false; // курсор указывает на что-то кликабельное
    public float interractDistance;
    Camera cam;
    private GameObject interactible, currentInteractible;    // объект, с которым можно взаимодействовать
    ItemAction itemAction;


    //  public EventVector3 OnClickEnvironment;
    //  public EventVector3 OnRightClickEnvironment;
    public EventGameObject OnHoverInteractible;    // событие наведения мыши на объект с которым можно взаимодействовать
    public EventGameObject OnExitHoverInteractible;
    
    private bool showCursor = true;

    Transform forwardCam, itemForward;


    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

       // FPS = GameObject.Find("FPSController");
        Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        // проверить также, что объект повёрнут лицом к ГГ, а то сейчас терминалы можно прочитать с задней стороны

        if (UIManager.Instance.CurrentGameUIType != UIManager.GameUIType.NONE)   // если мы в режиме просмотра инвентаря или планшета, выходим
            return;

        RaycastHit hit;
        if (!cam)
            return; 



        // этот код не учитывает моментальный переход с одного кликабельного объекта на другой

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out hit, interractDistance, clickableLayer.value))  // если провели мышью над кликэбл лейер
        {

            bool isInteractible = hit.collider.GetComponent(typeof(IInteractible)) != null; // и объект при этом интерактивный
            forwardCam = cam.transform;
            itemForward = hit.collider.gameObject.transform;


            // проверить не только isHovering, но и замену объекта

            if (isInteractible)  // если взгляд наткнулся на интерактивный объект с лицевой стороны ( в случае терминала)
            {

                interactible = hit.collider.gameObject; // Hero controller очень хочет знать, на что мы смотрим

                if (!isHovering || (interactible != currentInteractible))    // это новый объект
                {
                    // если перевели взгляд с одного объекта на другой - сначала пульнуть событие unhover в старый объект 
                    if (isHovering && currentInteractible) OnExitHoverInteractible.Invoke(currentInteractible);

                    currentInteractible = interactible;
                    itemAction = currentInteractible.GetComponent<ItemAction>();

                    //// костыль на костыле - ПЕРЕДЕЛАТЬ В НОРМ АРХИТЕКТУРУ
                    if (!forwardCam.IsFacingTarget(itemForward) && itemAction.itemDefinition.itemType == ItemTypeDefenitions.TERMINAL)
                        return; // если это терминал, но смотрим на него сзади, то ничего не делаем

                        // если дверной замок уже открыт, игнорируем
                    if (itemAction.itemDefinition.itemType == ItemTypeDefenitions.DOORLOCK && itemAction.itemDefinition.isOpened)
                        return;

                    isHovering = true;

#if UNITY_EDITOR
                 //   Debug.Log("смотрим на объект " + currentInteractible);
#endif

                    OnHoverInteractible.Invoke(currentInteractible);   // пульнём в него событие 

                    // эти вызовы вынестти из Update, потому что сильно вешают систему, а может и не в них дело
                    
                    UIManager.Instance.EnableAndFillItemUI(itemAction);
                }
            }
        }
        else // перенесли мышь на не кликабельный лейер 
        {
            if (isHovering)
            {
                if (currentInteractible)
                {
                    OnExitHoverInteractible.Invoke(currentInteractible);   // пульнём в него событие
                    isHovering = false;
                    UIManager.Instance.TurnOnItemUI(false);

#if UNITY_EDITOR
                //    Debug.Log("перестали смотреть на объект " + currentInteractible);
#endif

                }
            }
        }
    }

    public void SwitchFPS(bool turnOn)
    {
        if (FPS)
        {
            FPS.GetComponent<FirstPersonController>().enabled = turnOn;
          //  Debug.Log("фпс " + turnOn);
        }
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
       // Debug.Log("procedure MouseManager.HandleGameStateChanged");

        // пока что скрываем курсор во время игры, показывваем только в меню
        showCursor = (currentState != GameManager.GameState.RUNNING);
        shouldRotateView = (currentState == GameManager.GameState.RUNNING);

        // Set cursor
        Cursor.visible = showCursor;

        //крутим камерой только в режиме игры
        FPS = GameObject.Find("FPSController");
        if (FPS)
        {
            FPS.GetComponent<FirstPersonController>().enabled = shouldRotateView;
            cam = FPS.GetComponentInChildren<Camera>();
        }

       // Debug.Log("пытаемся подписаться на событие ...1");
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)    // если переходим от главного меню в игру - 
        {
            // на момент подписывания characterInventory ещё не существует
            // и срабатывает только после TogglePause
            // т.е. подписываться надо только тогда, когда CharacterInventory.instance загружен. 

            StartCoroutine(AddingListenerToCharacterInventory());

            //  CharacterInventory.instance.OnToggleInventory.AddListener(HandleToggleGameInventory);   // подписываемся на событие вызова игрового меню

        }
    }


    IEnumerator AddingListenerToCharacterInventory()
    {
        // дожидаемся пока загрузится CharacterInventory
        yield return new WaitUntil(() => CharacterInventory.Instance);

        // а затем подписываемся на его событие
        CharacterInventory.Instance.OnToggleInventory.AddListener(HandleToggleGameInventory);
      //  Debug.Log("...2 CharacterInventory.instance.OnToggleInventory.AddListener");

    }

    void HandleToggleGameInventory()
    {
        shouldRotateView = !CharacterInventory.Instance.InventoryDisplayHolder.activeSelf;  // если инвентарь неактивен, то вращаем мышкой и наоборот
        FPS.GetComponent<FirstPersonController>().enabled = shouldRotateView;
    }

}

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }

[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> {}


/*
    if (isInteractible && !isHovering )  // если взгляд наткнулся на интерактивный объект с лицевой стороны ( в случае терминала)
            {



                interactible = hit.collider.gameObject; // Hero controller очень хочет знать, на что мы смотрим

                if (!forwardCam.IsFacingTarget(itemForward) && interactible.GetComponent<ItemAction>().itemDefinition.itemType == ItemTypeDefenitions.TERMINAL)
                    return; // если это терминал, но смотрим на него сзади, то ничего не делаем

                isHovering = true;

                OnHoverInteractible.Invoke(interactible);   // пульнём в него событие 

                // эти вызовы вынестти из Update, потому что сильно вешают систему, а может и не в них дело
                
                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! перенести как раз в ивент
                UIManager.Instance.TurnOnItemUI(true);
                // загрузить в ItemUI название итема и действия
                UIManager.Instance.SetItemUI(interactible.GetComponent<ItemAction>().itemDefinition.itemType.ToString(),        // пропишем в табличку itemUI что за элемент
                                                interactible.GetComponent<ItemAction>().itemDefinition.actionType.ToString(),   // что с ним можно  делать
                                                  InputManager.instance.KeyBinding.interact.ToString()  );  // и какую для этого нажать кнопку

            }
     */
