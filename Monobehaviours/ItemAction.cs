using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAction : MonoBehaviour, IInteractible
{

    public Item_SO itemDefinition;
    public Events.EventFileRead OnFileRead;
    public Events.EventMission OnMissionStart; // ивент запуска новой миссии

    [SerializeField] GameObject dependentObject;

   // public Events.EventItemPickup OnItemPickup;


    private Animator itemAnimator;
    private bool isOpened;
    private Outline outline;
    // private ItemPickup item; // создаём вспомогательный класс для передачи item_SO и связанного GameObject для манипуляций в инвентаре




    public void OnHover()
    {
        // подсветить объект
        outline.enabled = true;
    }

    public void OnUnhover()
    {
        outline.enabled = false;
    }


    void Awake()
    {
        OnFileRead.AddListener(StatsManager.OnFileRead);    // подписываем менеджера на событие прочтения планшета или КПК
        OnMissionStart.AddListener(GameplayManager.OnMissionStart);   // подписываем менеджера миссий на событие получения новой миссии от использования предмета


        // OnItemPickup.AddListener(CharacterInventory.Instance.OnItemPickup);

        if (itemDefinition.isAnimated)
        {
            itemAnimator = gameObject.GetComponent<Animator>();
        }

        outline = GetComponent <Outline>();
        if (!outline)
            outline = GetComponentInParent<Outline>();

        isOpened = itemDefinition.isOpened;


        if (isOpened && itemDefinition.isStorage)
            itemDefinition.actionType = ItemActioinsType.CLOSE;

      //  Debug.Log("item " + itemDefinition.itemName + " isOpened = " + itemDefinition.isOpened + " actionType = " + itemDefinition.actionType);

        /* это не срабатывает вероятно из-за того, что item создан раньше characterInventory
        if (CharacterInventory.Instance)
        {
           OnFileRead.AddListener(CharacterInventory.Instance.OnFileRead);
        }*/
    }



public void OnInteract()
    {
        switch (itemDefinition.itemType)
        {
            case ItemTypeDefenitions.TABLET:
                // показать интерфейс книги
                    OnFileRead.Invoke(itemDefinition.contentNameKey, itemDefinition.contentKey);   // запоминаем, что данный файл прочитан (чтобы потом сохранить)

                /// вероятно, эти вызовы тоже стоит переделать на подписчиков события
                    UIManager.Instance.TurnOnTabletUI(true);

                    UIManager.Instance.SetTabletContent(LocalizationManager.instance.GetLocalizedValue(itemDefinition.contentNameKey), 
                                                            LocalizationManager.instance.GetLocalizedValue(itemDefinition.contentKey)); // подставляем в планшет нужный локализованный текст

                /// StatsManager должен запомнить, что гг этот планшет прочитал

                    SoundManager.Instance.PlaySoundEffect(itemDefinition.ActionSound);

                // для целей запуска миссии надо запомнить, что планшет (или терминал) открыт, чтобы запустить миссию при закрытии
                

                break;

            case ItemTypeDefenitions.TERMINAL:

                // показать интерфейс терминала
                

                UIManager.Instance.SetTerminalLogin(itemDefinition.login, itemDefinition.password, itemDefinition.letters, itemDefinition.itemName); // подставляем в терминал логин, пароль, набор писем и его имя
                UIManager.Instance.TurnOnTerminalUI(true);

                SoundManager.Instance.PlaySoundEffect(itemDefinition.ActionSound);

                break;

            case ItemTypeDefenitions.DOORLOCK:
                // показать интерфейс панели доступа, если она закрыта

                if (!StatsManager.DoorIsOpened(itemDefinition.itemName))
                {
                    UIManager.Instance.TurnOnDoorlockUI(true);
                    UIManager.Instance.SetTerminalPin(itemDefinition.lockPIN, this.gameObject, itemDefinition.itemName); // подставляем пин код и саму дверь для управления ею
                    SoundManager.Instance.PlaySoundEffect(SoundEffect.MenuSelectionClick);
                }
                break;

            case ItemTypeDefenitions.LOCKER:    // надо подумать над сохранением состояний всех шкафов и коробок - чтобы после перезагрузки игры юзер не открывал их заново

                if (isOpened)
                {
                    itemAnimator.SetTrigger("ToClose");
                    isOpened = false;

                   // itemDefinition.isOpened = false;
                   // itemDefinition.actionType = ItemActioinsType.OPEN;

                }
                else
                {
                    itemAnimator.SetTrigger("ToOpen");
                    isOpened = true;
                    SoundManager.Instance.PlaySoundEffect(SoundEffect.LockerOpen);

                   // itemDefinition.isOpened = true;
                   // itemDefinition.actionType = ItemActioinsType.CLOSE;
                }
                break;

            case ItemTypeDefenitions.BIGBOX:    // надо подумать над сохранением состояний всех шкафов и коробок - чтобы после перезагрузки игры юзер не открывал их заново

                if (isOpened)
                {
                    itemAnimator.SetTrigger("ToClose");
                    isOpened = false;

                }
                else
                {
                    itemAnimator.SetTrigger("ToOpen");
                    isOpened = true;
                    SoundManager.Instance.PlaySoundEffect(SoundEffect.BigBoxOpen);

                }
                break;

            case ItemTypeDefenitions.SMALLBOX:    // надо подумать над сохранением состояний всех шкафов и коробок - чтобы после перезагрузки игры юзер не открывал их заново

                if (isOpened)
                {
                    itemAnimator.SetTrigger("ToClose");
                    isOpened = false;

                }
                else
                {
                    itemAnimator.SetTrigger("ToOpen");
                    isOpened = true;
                    SoundManager.Instance.PlaySoundEffect(SoundEffect.SmallBoxOpen);
                }
                break;

            case ItemTypeDefenitions.HIBERNATOR:    // надо подумать над сохранением состояний всех шкафов и коробок - чтобы после перезагрузки игры юзер не открывал их заново

                if (isOpened)
                {
                    itemAnimator.SetTrigger("ToClose");
                    isOpened = false;
                    SoundManager.Instance.PlaySoundEffect(SoundEffect.HibernatorClose);

                }
                else
                {
                    itemAnimator.SetTrigger("ToOpen");
                    isOpened = true;
                    SoundManager.Instance.PlaySoundEffect(SoundEffect.HibernatorOpen);
                }
                break;



                //// Это фаршмак, надо сделать более универсальным!!!!

            case ItemTypeDefenitions.TOILETBOWL:

                this.gameObject.GetComponent<AudioSource>().Play();

                break;

            case ItemTypeDefenitions.WATERTAP:

                this.gameObject.transform.Find("WaterLeak").gameObject.SetActive(true);
                this.gameObject.GetComponent<AudioSource>().Play();

                Invoke("TurnOffWaterLeak", 7);
                break;


            case ItemTypeDefenitions.SWITCH:

                GameObject Window = this.gameObject.transform.parent.gameObject.transform.Find("Wall_Window").gameObject;
              //  Window.GetComponent<AudioSource>().Play(); // играем woosh

                this.gameObject.GetComponent<AudioSource>().Play(); // играем switch
                itemAnimator.SetTrigger("Switch");
                

                
                // пытаемся подменить стекло в окне
                        Material GoldGlass = (Material)Resources.Load("Glass_Gold", typeof(Material));
                        Material Glass = (Material)Resources.Load("Glass", typeof(Material));
                        Material[] MatArray = new Material[4];

                        MatArray = Window.GetComponent<Renderer>().materials;

                        if (MatArray[2].name == "Glass (Instance)")
                        {
                             MatArray[2] = GoldGlass;
                           // Window.GetComponent<Animator>().SetTrigger("SwitchToGold");
                            // включаем коллайдр, чтобы не пропускало блик от солнца
                            Window.gameObject.transform.parent.gameObject.transform.Find("ShineCollision").gameObject.SetActive(true);
                            Window.gameObject.transform.parent.gameObject.transform.Find("Curtain").gameObject.SetActive(true);
                        }
                        else
                        {
                             MatArray[2] = Glass;
                           // Window.GetComponent<Animator>().SetTrigger("SwitchToGlass");
                            // ВЫКЛЮЧАЕМ КОЛЛАЙДР
                            Window.gameObject.transform.parent.gameObject.transform.Find("ShineCollision").gameObject.SetActive(false);
                            Window.gameObject.transform.parent.gameObject.transform.Find("Curtain").gameObject.SetActive(false);

                }
                        Window.GetComponent<Renderer>().materials = MatArray;
                        
                

                break;


                /*  case ItemTypeDefinitions.MANA:
                      charStats.ApplyMana(itemDefinition.itemAmount);
                      break;
                  case ItemTypeDefinitions.WEALTH:
                      charStats.GiveWealth(itemDefinition.itemAmount);
                      break;
                  case ItemTypeDefinitions.WEAPON:
                      charStats.ChangeWeapon(this);
                      break;
                  case ItemTypeDefinitions.ARMOR:
                      charStats.ChangeArmor(this);
                      break;*/
        }


        /// для любого типа, если item Is Strorable, то положим его в инвентарь
        if (itemDefinition.isStorable)
        {
          
            // 1.+ удалить объект со сцены
            // 2.+ поместить объект в массив поднятых предметов
            // 3.+ отобразить объект в инвентаре
            // 4. показать подсказку к поднятому объекту
            // 5.+ издать звук 
            // 6.+ погасить ItemUI
            CharacterInventory.Instance.OnItemPickup(this);

        }


        // для любого типа, если он запускает миссию - запустим миссию
        // но есть нюанс -  если миссию запускает прочение планшета, то запускать миссию надо не при открытии, а при закрытии планшета...

        if(itemDefinition.isTriggerForNewMission)
        {
           
                // анализ ещё одного поля - старт миссии по факту использования предмета, либо по факту завершения взаимодействия
                if (itemDefinition.missionEventStartCondition == MissionEventStartCondition.ON_INTERACT)
                    OnMissionStart.Invoke(itemDefinition.StartsNewMission);
                else
                {
                    // устанавливаем глобальный признак ожидания начала миссии
                    MissionManager.Instance.SetisWaitingToActionForStartMission(true);

                    // и обозначаем, что это за мисиия
                    MissionManager.Instance.SetMissionToStart(itemDefinition.StartsNewMission);
                }


        }


    }

    private void TurnOffWaterLeak()
    {
        this.gameObject.transform.Find("WaterLeak").gameObject.SetActive(false);
    }



    public void PlaySound(string sound)     // звук закрытия шкафа играем в конце анимации
    {
        SoundEffect Sound = (SoundEffect)System.Enum.Parse(typeof(SoundEffect), sound); // переводим строку в enum
        SoundManager.Instance.PlaySoundEffect(Sound);
    }


    // это пока выключаем

    /*public void SetOpened(bool status)
    {
        itemDefinition.isOpened = status;
    }*/

          /*public void UseItem()   // зайти сюда по кнопке Е
          {
              switch (itemDefinition.itemType)
              {
                  case ItemTypeDefenitions.TABLET:
                      // показать интерфейс книги


                      //charStats.ApplyHealth(itemDefinition.itemAmount);
                      Debug.Log("прочли книгу");
                      break;
                /*  case ItemTypeDefinitions.MANA:
                      charStats.ApplyMana(itemDefinition.itemAmount);
                      break;
                  case ItemTypeDefinitions.WEALTH:
                      charStats.GiveWealth(itemDefinition.itemAmount);
                      break;
                  case ItemTypeDefinitions.WEAPON:
                      charStats.ChangeWeapon(this);
                      break;
                  case ItemTypeDefinitions.ARMOR:
                      charStats.ChangeArmor(this);
                      break;
              }
          }*/
}
