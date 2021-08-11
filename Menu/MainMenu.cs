using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{    
  //  [SerializeField] private Animation _mainMenuAnimator;
  //  [SerializeField] private AnimationClip _fadeOutAnimation;
  //  [SerializeField] private AnimationClip _fadeInAnimation;
   

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
     //   Debug.Log("add listener in main menu");
               
    }




    public void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
    }



    public void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {


        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)    // если переходим от главного меню в игру - 
        {
           // FadeOut();  // затеняем меню 
            Debug.Log("Fade out");
        }   

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)    // если переходим к главному меню
        {
          //  FadeIn();   // показываем его
            Debug.Log("Fade in");
        }
    }

  /*  public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }

    public void FadeOut()
    {

      //  Debug.Log("Fade out running - dummy canera off");

        UIManager.Instance.SetDummyCameraActive(false);

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }*/
}
