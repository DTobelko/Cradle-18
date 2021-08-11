using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public KeyBinding_SO KeyBinding;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public bool KeyDown(string key)
    {
        if (Input.GetKeyDown(KeyBinding.CheckKey(key)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
