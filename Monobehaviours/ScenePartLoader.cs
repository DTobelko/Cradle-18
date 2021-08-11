using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum CheckMethod
{
    Distance,   // больше подходит для открытых локаций
    Trigger     //больше подходит для домов и лабиринтов
}

public class ScenePartLoader : MonoBehaviour
{
    public Transform player;
    public CheckMethod checkMethod;
    public float loadRange;

    private bool isLoaded;
    private bool shouldLoad;

    // Start is called before the first frame update
    void Start()
    {
        // проверяем, что сцена уже открыта, чтобы не открыть повторно
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkMethod == CheckMethod.Distance)
        {
            DistanceCheck();
        }
        else if (checkMethod == CheckMethod.Trigger)
        {
            TriggerCheck();
        }
    }

    void DistanceCheck()
    {
        if (Vector3.Distance(player.position, transform.position) < loadRange)
        {
            LoadScene();
        }
        else
        {
            UnloadScene();
        }
    }


    // загружаем сцену станции (добавочным методом)
    void LoadScene()
    {
        if (!isLoaded)
        {
           // SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive); // подгрузка сцен в начале игры не охватывается статус баром....
            
            // добавляем сюда проверку окончания асинхронной загрузки   
            StartCoroutine(LoadAsynchronously(gameObject.name));

            isLoaded = true;
        }
    }

    IEnumerator LoadAsynchronously(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        while (!ao.isDone)
        {
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            UIManager.Instance.SetValueInLoadingSlider(0.5f + progress / 2);  // в начале игры загружается сразу несколько сцен: Gameplay, HibernatorsModule
                                                                   // потому.... надо разделить слайдер на две части
            yield return null;
        }

/// пока костыль - убираем экран загрузки после загрузки 1-го уровня
            if (gameObject.name == "HibernatorsModule")
                UIManager.Instance.ShowLoadingScreen(false);
        
    }





    void UnloadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
    }

    void TriggerCheck()
    {
        if (shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnloadScene();
        }
    }
}
