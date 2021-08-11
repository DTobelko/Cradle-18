using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="KeyBindings", menuName ="KeyBindings")]
public class KeyBinding_SO : ScriptableObject
{
    public KeyCode jump, interact, inventory, forward, backward, left, right, light;

    public KeyCode CheckKey(string key)
    {
        switch (key)
        {
            case "Jump":
                return jump;

            case "Interact":
                return interact;

            case "Inventory":
                return inventory;

            case "Forward":
                return forward;

            case "Backward":
                return backward;

            case "Left":
                return left;

            case "Right":
                return right;

            case "Light":
                return light;

            default:
                return KeyCode.None;
        }
    }

}
