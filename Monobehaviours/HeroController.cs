using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{

    private GameObject interactibleGO;
    [SerializeField] private GameObject Flashlight;
    [SerializeField] private GameObject WeaponCamera;

    private void Start()
    {
        // подписываемся на событие наведения мышью на что-либо кликабельное
        if (MouseManager.Instance != null)
        {
            MouseManager.Instance.OnHoverInteractible.AddListener(HoverOverTarget);
            MouseManager.Instance.OnExitHoverInteractible.AddListener(UnHoverOverTarget);

        }
    }

    public void HoverOverTarget(GameObject target)  // мышь указывает на объект
    {
        interactibleGO = target; // запоминаем объект, с которым будем возможно взаимоействовать

        var interactibles = target.GetComponentsInChildren(typeof(IInteractible));

        foreach (IInteractible interactible in interactibles)
        {
            interactible.OnHover(); // подсветить объект
        }

    }

    public void UnHoverOverTarget(GameObject target) // мышь ушла с объекта
    {
        interactibleGO = target; // запоминаем объект, с которым будем возможно взаимоействовать

        var interactibles = target.GetComponentsInChildren(typeof(IInteractible));

        foreach (IInteractible interactible in interactibles)
        {
            interactible.OnUnhover(); // подсветить объект
        }


    }

    void ToggleFlashlight()
    {
        if (Flashlight.activeSelf)
        {
            Flashlight.GetComponent<Animator>().SetTrigger("ToDisable");
            Invoke("DisableFlashlight", 1);
        }
        else
        {
            Flashlight.SetActive(true);
            WeaponCamera.SetActive(true);
            Flashlight.GetComponent<Animator>().SetTrigger("ToEnable");
        }
    }

    private void DisableFlashlight()
    {
         Flashlight.SetActive(false);
        WeaponCamera.SetActive(false);
    }


    private void Update()
    {

        // отследить нажатие кнопки действия
        // если при этом игрок смотрит на какой-то предмет, то послать ивент для показа соотв. интерфейса
        // if (Input.GetKeyDown(KeyCode.E))

        if (InputManager.instance.KeyDown("Light")) // вызов фонарика
        {
            if (CharacterInventory.Instance.IsItemInInventory("Flashlight")) // проверим, найден ли фонарик
            {
                ToggleFlashlight();
            }
        }


        if (!MouseManager.Instance.isHovering) // если мышь не над объектом
            return;

        if (UIManager.Instance.CurrentGameUIType != UIManager.GameUIType.NONE)
            return;

        if (InputManager.instance.KeyDown("Interact"))
        {
            // послать в объект ивент "использовать"

            var interactibles = interactibleGO.GetComponentsInChildren(typeof(IInteractible));

            foreach (IInteractible interactible in interactibles)
            {
                interactible.OnInteract(); // провзаимодейстовать с объектом 
            }
            
            

        }


    }
}
