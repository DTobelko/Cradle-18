using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public TabGroup tabGroup;

    public Image IconImage;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    public SoundEffect TabClickSound;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }



    // Start is called before the first frame update
   // void Start()
   // {
        //background = GetComponentInChildren<Image>();
        //tabGroup.Subscribe(this);
   // }

    public void Select()
    {
        if (onTabSelected != null)
            onTabSelected.Invoke();

       SoundManager.Instance.PlaySoundEffect(TabClickSound);
    }
    
    
    public void Deselect()
    {
        if (onTabDeselected != null)
            onTabDeselected.Invoke();

    }
}
