using UnityEngine;
using UnityEngine.UI;

public class TipMessageUI : MonoBehaviour
{
    [SerializeField] private Text Message;
   // [SerializeField] private string PickupStringKey = "PlayerGotSomething";

    public void SetTipMessage(string iName)
    {
        Message.text = iName;
    }
}
