using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TabletUI : MonoBehaviour
{
    [SerializeField] private Text tabletName;
    [SerializeField] private Text tabletContent;

    public void SetTabletName(string iName)
    {
        tabletName.text = iName;
    }

    public void SetTabletContent(string tContent)
    {
        tabletContent.text = tContent.Replace("\\t", "\t").Replace("\\n", "\n");
    }

}
