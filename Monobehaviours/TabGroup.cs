using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;

    public TabButton selectedTab;

    public List<GameObject> objectsToSwap;

    public SoundEffect TabHoverSound;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);

    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();

        if (selectedTab == null || button != selectedTab)
        {
            button.IconImage.color = tabHover;
            SoundManager.Instance.PlaySoundEffect(TabHoverSound);
        }
    }


    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
      //  Debug.Log("OnTabSelected");
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        button.IconImage.color = tabActive;

        int index = button.transform.GetSiblingIndex();

      //  Debug.Log("index = " + index);

        for (int i=0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
              //  Debug.Log("set active " + i);
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
              //  Debug.Log("set not active " + i);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.IconImage.color = tabIdle;
        }
    }

}
