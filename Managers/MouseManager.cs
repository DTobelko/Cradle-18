using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;

// �������� ������ � ������ �������� ���� � � ������ �����
// ������ ���� - �������� ������ � ������ ������ ���������

public class MouseManager : Manager<MouseManager> //onoBehaviour
{
    public Texture2D pointer;
    private GameObject FPS;
    private bool shouldRotateView = true;   // ������� �� �������
    public LayerMask clickableLayer;
    public bool isHovering = false; // ������ ��������� �� ���-�� ������������
    public float interractDistance;
    Camera cam;
    private GameObject interactible, currentInteractible;    // ������, � ������� ����� �����������������
    ItemAction itemAction;


    //  public EventVector3 OnClickEnvironment;
    //  public EventVector3 OnRightClickEnvironment;
    public EventGameObject OnHoverInteractible;    // ������� ��������� ���� �� ������ � ������� ����� �����������������
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
        // ��������� �����, ��� ������ ������� ����� � ��, � �� ������ ��������� ����� ��������� � ������ �������

        if (UIManager.Instance.CurrentGameUIType != UIManager.GameUIType.NONE)   // ���� �� � ������ ��������� ��������� ��� ��������, �������
            return;

        RaycastHit hit;
        if (!cam)
            return; 



        // ���� ��� �� ��������� ������������ ������� � ������ ������������� ������� �� ������

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out hit, interractDistance, clickableLayer.value))  // ���� ������� ����� ��� ������� �����
        {

            bool isInteractible = hit.collider.GetComponent(typeof(IInteractible)) != null; // � ������ ��� ���� �������������
            forwardCam = cam.transform;
            itemForward = hit.collider.gameObject.transform;


            // ��������� �� ������ isHovering, �� � ������ �������

            if (isInteractible)  // ���� ������ ��������� �� ������������� ������ � ������� ������� ( � ������ ���������)
            {

                interactible = hit.collider.gameObject; // Hero controller ����� ����� �����, �� ��� �� �������

                if (!isHovering || (interactible != currentInteractible))    // ��� ����� ������
                {
                    // ���� �������� ������ � ������ ������� �� ������ - ������� �������� ������� unhover � ������ ������ 
                    if (isHovering && currentInteractible) OnExitHoverInteractible.Invoke(currentInteractible);

                    currentInteractible = interactible;
                    itemAction = currentInteractible.GetComponent<ItemAction>();

                    //// ������� �� ������� - ���������� � ���� �����������
                    if (!forwardCam.IsFacingTarget(itemForward) && itemAction.itemDefinition.itemType == ItemTypeDefenitions.TERMINAL)
                        return; // ���� ��� ��������, �� ������� �� ���� �����, �� ������ �� ������

                        // ���� ������� ����� ��� ������, ����������
                    if (itemAction.itemDefinition.itemType == ItemTypeDefenitions.DOORLOCK && itemAction.itemDefinition.isOpened)
                        return;

                    isHovering = true;

#if UNITY_EDITOR
                 //   Debug.Log("������� �� ������ " + currentInteractible);
#endif

                    OnHoverInteractible.Invoke(currentInteractible);   // ������ � ���� ������� 

                    // ��� ������ �������� �� Update, ������ ��� ������ ������ �������, � ����� � �� � ��� ����
                    
                    UIManager.Instance.EnableAndFillItemUI(itemAction);
                }
            }
        }
        else // ��������� ���� �� �� ������������ ����� 
        {
            if (isHovering)
            {
                if (currentInteractible)
                {
                    OnExitHoverInteractible.Invoke(currentInteractible);   // ������ � ���� �������
                    isHovering = false;
                    UIManager.Instance.TurnOnItemUI(false);

#if UNITY_EDITOR
                //    Debug.Log("��������� �������� �� ������ " + currentInteractible);
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
          //  Debug.Log("��� " + turnOn);
        }
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
       // Debug.Log("procedure MouseManager.HandleGameStateChanged");

        // ���� ��� �������� ������ �� ����� ����, ����������� ������ � ����
        showCursor = (currentState != GameManager.GameState.RUNNING);
        shouldRotateView = (currentState == GameManager.GameState.RUNNING);

        // Set cursor
        Cursor.visible = showCursor;

        //������ ������� ������ � ������ ����
        FPS = GameObject.Find("FPSController");
        if (FPS)
        {
            FPS.GetComponent<FirstPersonController>().enabled = shouldRotateView;
            cam = FPS.GetComponentInChildren<Camera>();
        }

       // Debug.Log("�������� ����������� �� ������� ...1");
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)    // ���� ��������� �� �������� ���� � ���� - 
        {
            // �� ������ ������������ characterInventory ��� �� ����������
            // � ����������� ������ ����� TogglePause
            // �.�. ������������� ���� ������ �����, ����� CharacterInventory.instance ��������. 

            StartCoroutine(AddingListenerToCharacterInventory());

            //  CharacterInventory.instance.OnToggleInventory.AddListener(HandleToggleGameInventory);   // ������������� �� ������� ������ �������� ����

        }
    }


    IEnumerator AddingListenerToCharacterInventory()
    {
        // ���������� ���� ���������� CharacterInventory
        yield return new WaitUntil(() => CharacterInventory.Instance);

        // � ����� ������������� �� ��� �������
        CharacterInventory.Instance.OnToggleInventory.AddListener(HandleToggleGameInventory);
      //  Debug.Log("...2 CharacterInventory.instance.OnToggleInventory.AddListener");

    }

    void HandleToggleGameInventory()
    {
        shouldRotateView = !CharacterInventory.Instance.InventoryDisplayHolder.activeSelf;  // ���� ��������� ���������, �� ������� ������ � ��������
        FPS.GetComponent<FirstPersonController>().enabled = shouldRotateView;
    }

}

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }

[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> {}


/*
    if (isInteractible && !isHovering )  // ���� ������ ��������� �� ������������� ������ � ������� ������� ( � ������ ���������)
            {



                interactible = hit.collider.gameObject; // Hero controller ����� ����� �����, �� ��� �� �������

                if (!forwardCam.IsFacingTarget(itemForward) && interactible.GetComponent<ItemAction>().itemDefinition.itemType == ItemTypeDefenitions.TERMINAL)
                    return; // ���� ��� ��������, �� ������� �� ���� �����, �� ������ �� ������

                isHovering = true;

                OnHoverInteractible.Invoke(interactible);   // ������ � ���� ������� 

                // ��� ������ �������� �� Update, ������ ��� ������ ������ �������, � ����� � �� � ��� ����
                
                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ��������� ��� ��� � �����
                UIManager.Instance.TurnOnItemUI(true);
                // ��������� � ItemUI �������� ����� � ��������
                UIManager.Instance.SetItemUI(interactible.GetComponent<ItemAction>().itemDefinition.itemType.ToString(),        // �������� � �������� itemUI ��� �� �������
                                                interactible.GetComponent<ItemAction>().itemDefinition.actionType.ToString(),   // ��� � ��� �����  ������
                                                  InputManager.instance.KeyBinding.interact.ToString()  );  // � ����� ��� ����� ������ ������

            }
     */
