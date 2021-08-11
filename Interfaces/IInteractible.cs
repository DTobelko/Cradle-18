using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible // интерфейс для тех итемов, с которыми можно взаимодействовать
{
    void OnInteract(); // взаимодействуем

    void OnHover(); // наводим курсор

    void OnUnhover(); //  убираем курсор
}
