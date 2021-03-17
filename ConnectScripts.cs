using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Music;
using System;
using UnityEngine.EventSystems;

public static class MyEventSystemObject
{
    public static EventSystem myEventSystem;
}
public class ConnectScripts : MonoBehaviour  , Mediator
{
    #region master scripts
    // Start is called before the first frame update
    [SerializeField]
    private SoundManager _soundManager;
    [SerializeField]
    private List<MusicPlayer> _musicPlayer;
    [SerializeField]
    private List<SoundPlayer> _soundPlayer;
    
    [SerializeField]
    private MasterGUI masterGUI;
    [SerializeField]
    private MasterSave masterSave;
    [SerializeField]
    private MasterMenu masterMenu;
    [SerializeField]
    private MasterUnits masterUnits;
    [SerializeField]
    private MasterScore masterScore;
    [SerializeField]
    private MasterObject masterObject;
    [SerializeField]
    private MasterLanguage masterLanguage;
    [SerializeField]
    private MasterDialogue masterDialogue;

    #endregion
    [SerializeField]
    private Player player;
    [SerializeField]
    EventSystem eventSystem;



    void Awake()
    {
     
    }
    void Start()
    {
        MyEventSystemObject.myEventSystem = eventSystem;
        if (_soundManager != null)
        {
            _soundManager.mediator = this;
        }
        if (masterGUI != null)
        {
            masterGUI.mediator = this;
            masterGUI.market.GetScore = masterScore.GetScore;
            masterGUI.market.UpdateScore = masterScore.UpdateScore;


        }
        if (masterSave != null)
        {
            masterSave.mediator = this;
        }
        if (masterMenu != null)
        {
            masterMenu.mediator = this;
        }
        if (masterUnits != null)
        {
            masterUnits.mediator = this;
        }
        if (masterScore != null)
        {
            masterScore.mediator = this;
            masterScore.updateScore += masterGUI.market.textScore.SetScore;
            masterScore.updateScore?.Invoke(masterScore.GetScore());
        }
        if (masterObject != null)
        {
            masterObject.mediator = this;
        }
        if (masterLanguage != null)
        {
            masterLanguage.mediator = this;
        }

        int lenght = _musicPlayer.Count;
        for (int i = 0; i < lenght; i++)
        {
            _soundManager.Attach(_musicPlayer[i]);

        }

        lenght = _soundPlayer.Count;
        for (int i = 0; i < lenght; i++)
        {
            _soundManager.Attach(_soundPlayer[i]);
        }

        if (_soundManager)
        {
            _soundManager.Play("Master", true);
        }

        if (player != null)
        {
            player.SwitchGUI = masterGUI.SwitchUnit;
            player.ObjectIsAvalible = masterUnits.Send;
            player.ObjectIsUnavalible = masterUnits.Send;
            player.SendMessageInMaster = masterGUI.Send;
            if (masterGUI != null)
            {
                masterGUI.instantHackPanel.SendToMaster = masterGUI.SendAndEdit;
                masterGUI.spetialSkillsPanel.sendSpetialSkillsType = player.SpecialSkill.SetSkill;
                masterGUI.spetialSkillsPanel.sendSpetialSkillsTypeUse = player.SpecialSkill.Use;
                masterGUI.my_map.AddCenterPoint(player.gameObject);

                player.UpdateSkills.UpdateInstantHackMaxCount = masterGUI.instantHackPanel.UpdateMaxCount;
                player.UpdateSkills.UpdateInstantHackCount = masterGUI.instantHackPanel.UpdateCount;
                player.UpdateSkills.UpdateInstantHackSpeed = masterGUI.instantHackPanel.UpdateSpeed;

                for (int i = 0; i < masterGUI.market.tempElements.Count; i++)
                {
                    masterGUI.market.tempElements[i].Element.upgrateDateOnUnit = player.UpgrateSkillDate;
                }
            }
        }
        if (masterUnits != null)
        {
            masterUnits.NewUnit();
        }
        if (masterSave != null)
        {
            masterSave.LoadGame();
        }

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Score"), LayerMask.NameToLayer("Unit"), true);
        MyEventSystemObject.myEventSystem = eventSystem;
    }
    public void TEMP_CREATE_UNIT()
    {
        masterUnits.Notify(TypeMessage.UpdateUnits);
    }

    #region save/load
    /// <summary>
    /// подумать об возможности создание интепфейса или через медиатр отправлять оповещения
    /// </summary>
    void Save()
    {
        if (masterUnits != null)
        {
            masterSave._save.zoneEnemyCreateData = masterUnits.Save();
        }
        //    masterObject.Save();
        masterSave._save.gameSetingsSaveData = new GameSetingsSaveData((masterScore==null?0:masterScore.Save()), masterLanguage.Save());
        if (player != null)
        {
            masterSave._save.playerSaveData = player.Save();
        }
        if(masterGUI != null)
        {
            masterSave._save.guiSaveData = masterGUI.Save();
        }

        masterSave.Notify(TypeMessage.SaveGame);
    }

    void Load()
    {
        if (masterUnits != null)
        {
            masterUnits.Load(masterSave._save.zoneEnemyCreateData);
        }
        //    masterObject.Load();
        if (masterScore != null)
        {
            masterScore.Load(masterSave._save.gameSetingsSaveData.score);
        }
        if (masterLanguage != null)
        {
            masterLanguage.Load(masterSave._save.gameSetingsSaveData.language);
        }
        if (masterGUI != null)
        {
            masterGUI.Load(masterSave._save.guiSaveData);
        }
        if (player != null)
        {

            player.Load(masterSave._save.playerSaveData);
        }
       

    }
    #endregion

    public void Send(TypeMessage msg, Colleague colleague)
    {

        if (masterMenu.GetType() == colleague.GetType())
        {
            if(TypeMessage.EnterInMenu == msg)
            {
                _soundManager.Notify(TypeMessage.PauseSound);
            }
            else if (TypeMessage.ExitOutMenu == msg)
            {
                _soundManager.Notify(TypeMessage.UnPauseSound);
            }
        }  
        else if (masterSave.GetType() == colleague.GetType())
        {
          
            switch(msg)
            {
                case TypeMessage.SaveGame:
                    Save();
                    break;
                case TypeMessage.LoadGame:
                    Load();
                    break;
            }
        }
        else if(masterObject.GetType() == colleague.GetType())
        {
            switch (msg)
            {
                case TypeMessage.BreakingActivete:
                case TypeMessage.BreakingUse:
                case TypeMessage.BreakingDeactivete:
                case TypeMessage.UseInstantHack:
                    masterGUI.Notify(msg);
                    break;
            }
        }
        else if (masterGUI.GetType() == colleague.GetType())
        {
            switch (msg)
            {
                case TypeMessage.BreakingComplit:                 
                case TypeMessage.BreakingFaill:
                case TypeMessage.UseInstantHack:
                    masterObject.Notify(msg);
                    break;
                case TypeMessage.UpdateLevelCallDrone:
                    player.SpecialSkill.UpdateLevelCallDrone((uint)player.updateStateFromSkill.levelCallDrone);
                    masterGUI.Notify(msg);
                    break;
                case TypeMessage.UpdateLevelCallRobot:
                    player.SpecialSkill.UpdateLevelCallRobot((uint)player.updateStateFromSkill.levelCallRobot);
                    masterGUI.Notify(msg);
                    break;
                case TypeMessage.UpdateLevelCallSoldat:
                    player.SpecialSkill.UpdateLevelCallSoldat((uint)player.updateStateFromSkill.levelCallSoldat);
                    masterGUI.Notify(msg);
                    break;
                case TypeMessage.ActivateInstantHack:
                case TypeMessage.DisableInstantHack:
                case TypeMessage.ActivateAcceleration:
                    player.SpecialSkill.ActivateAcceleration();
                    masterGUI.Notify(msg);
                    break;
                case TypeMessage.DisableAcceleration:
                case TypeMessage.UpdateCountInstantHack:
                case TypeMessage.UpdateSpesdUpdateInstantHack:
                    masterGUI.Notify(msg);
                    break;
                case TypeMessage.BreakingUpdateTime:
                    masterGUI.breaking.speedDeactivateBreaking = player.updateStateFromSkill.maxTimeForВreaking;
                    break;
               
            }

        }
        else if(masterUnits.GetType() == colleague.GetType())
        {
            switch (msg)
            {
                case TypeMessage.NewUnit:
                    for (int i = 0; i < masterUnits.zones.Count; i++)
                    {
                        for (int y = 0; y < masterUnits.zones[i].CreateUnits.Count; y++)
                        {
                            _soundManager.Attach(masterUnits.zones[i].CreateUnits[y].GetComponent<IObserverSound>());
                            if (masterUnits.zones[i].CreateUnits[y].GetComponent<IUnit>().TextElement != null)
                            {
                                masterLanguage.Attach(masterUnits.zones[i].CreateUnits[y].GetComponent<IUnit>().TextElement); //сделать при создании или загрузки добавление класса для изменение языка, расмотреть метод через подписки
                            }
                        }
                    }
                    break;
                case TypeMessage.DeadUnit:
                    for (int i = 0; i < masterUnits.zones.Count; i++)
                    {
                        for (int y = 0; y < masterUnits.zones[i].CreateUnits.Count; y++)
                        {
                            _soundManager.Detach(masterUnits.zones[i].CreateUnits[y].GetComponent<IObserverSound>());
                            masterLanguage.Delete(masterUnits.zones[i].CreateUnits[y].GetComponent<IUnit>().TextElement);
                        }
                    }
                    break;
                case TypeMessage.IsUseAvalible:
                    masterGUI.Notify(TypeMessage.IsUseAvalible);
                    break;
                case TypeMessage.IsUseUnavalible:
                    masterGUI.Notify(TypeMessage.IsUseUnavalible);
                    break;
                case TypeMessage.PlaerIsDead:
                    masterMenu.Notify(TypeMessage.PlaerIsDead);
                    break;
            }

        }
        

    }

    public void Send(TypeMessage msg, Vector3 _transphorm, Colleague colleague)
    {
        if (masterUnits.GetType() == colleague.GetType())
        {
            if (TypeMessage.PlayerKillUnits == msg)
            {
                // сделать обновления звука когда умерает бот
                masterScore.Notify(TypeMessage.GetScore, _transphorm);
                for (int i = 0; i < masterUnits.zones.Count; i++)
                {
                    for (int y = 0; y < masterUnits.zones[i].CreateUnits.Count; y++)
                    {
                        if (masterUnits.zones[i].CreateUnits[y].GetComponent<IUnit>().stateStruct.isDead)
                        {
                            _soundManager.Detach(masterUnits.zones[i].CreateUnits[y].GetComponent<IObserverSound>());
                            masterLanguage.Delete(masterUnits.zones[i].CreateUnits[y].GetComponent<IUnit>().TextElement);
                            Destroy(masterUnits.zones[i].CreateUnits[y]);
                            masterUnits.zones[i].CreateUnits.RemoveAt(y);
                        }
                    }
                }
            }
            if (TypeMessage.DeadUnit == msg)
            {            
                for (int i = 0; i < masterUnits.zones.Count; i++)
                {
                    for (;masterUnits.zones[i].CreateUnits.Count>0 && masterUnits.zones[i].CreateUnits[0].GetComponent<Player>() == null;)
                    {                      
                            _soundManager.Detach(masterUnits.zones[i].CreateUnits[0].GetComponent<IObserverSound>());
                            masterLanguage.Delete(masterUnits.zones[i].CreateUnits[0].GetComponent<IUnit>().TextElement);
                            Destroy(masterUnits.zones[i].CreateUnits[0]);
                            masterUnits.zones[i].CreateUnits.RemoveAt(0);                       
                    }
                }
            }
        }
        else if (masterObject.GetType() == colleague.GetType())
        {
            //сделать обновления звука когда взаимодействие с объектом
            masterScore.Notify(TypeMessage.GetScore, _transphorm);
        }
    }

    //public void Send(TypeMessage msg, Vector3 _transphorm, Colleague colleague)
    //{
    //    throw new NotImplementedException();
    //}
}

public enum TypeMessage
{
    //костыли изменить на отдельные интерфейсы
    EnterInMenu,
    ExitOutMenu,
    PauseMusic,
    UnPauseMusic,
    PauseSound,
    UnPauseSound,
    LoadGame,
    SaveGame,
    DeleteAllUnits,
    UpdateUnits,
    PlayerKillUnits,
    GetScore,
    TakeScore,
    ObjectTerminalScore,
    ObjectDoorOpen,
    ObjectCameraTarget,
    BreakingActivete,
    BreakingDeactivete,
    BreakingComplit,
    BreakingFaill,
    BreakingUse,
    NewUnit,
    DeadUnit,
    IsUseAvalible,
    IsUseUnavalible,
    PlaerIsDead,
    
    BreakingUpdateTime,
    UpdateLevelCallDrone,
    UpdateLevelCallRobot,
    UpdateLevelCallSoldat,

    ActivateAcceleration,
    DisableAcceleration,

    ActivateInstantHack,
    DisableInstantHack,
    UpdateCountInstantHack,
    UpdateSpesdUpdateInstantHack,
    UseInstantHack
}

public interface Mediator
{
      void Send(TypeMessage msg, Colleague colleague);
      void Send(TypeMessage msg, Vector3 _transphorm, Colleague colleague);
 
}
public interface Colleague
{
    Mediator mediator { get; set; } 

    void Send(TypeMessage message);
    void Send(TypeMessage message, Vector3 _transphorm);
    //  void Send(TypeMessage message, out GameSaveStruct save);
    void Notify(TypeMessage message);
    void Notify(TypeMessage message, Vector3 _transphorm);
    //   void Notify(TypeMessage message, out GameSaveStruct save);
}