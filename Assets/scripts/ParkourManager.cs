using FastBuf;
using System;
using UnityEngine;

public class ParkourManager : MonoBehaviour
{
    public static ParkourManager _instance;
    private GameObject cardP;
    [HideInInspector]
    public CharacterCtrl cCtrl;
    private CharacterFSMManager chaFSMManager;
    public GameObject[] effectObjs;
    private Vector3 gameStartPoint = Vector3.zero;
    public static Vector3 GameStartPos;
    public bool isPause;
    private bool m_GameStart;
    public AnimationCurve monsterDeadCurve;
    [HideInInspector]
    public CameraCtrl parkourCamera;
    public int[] parkourPropIndex = new int[] { 0x21, 0x2c, 0x62, 0x63 };
    public AnimationCurve propFlyCurve;
    public float runDistance;

    public static string AnimNmae(CharacterType type)
    {
        return CharacterFSMManager.Instance().GetStateInfo(type).animName;
    }

    private void Awake()
    {
        _instance = this;
    }

    public void DestoryParkourAsset()
    {
        UnityEngine.Object.Destroy(this.cardP.gameObject);
        ParkourEvent._instance.DestoryParkourEvent();
        ParkourInit._instance.DesteryParkourInitInfo();
    }

    private void FixedUpdate()
    {
        if (this.GameStart)
        {
            ParkourInit._instance.UpdateProp();
            ParkourInit._instance.DestoryMap();
        }
    }

    private void Init()
    {
        this.Pause(false);
        AnimFSMInfoManager.Instance();
        this.chaFSMManager = CharacterFSMManager.Instance();
        this.cCtrl = UnityEngine.Object.FindObjectOfType<CharacterCtrl>();
        GameStartPos = this.cCtrl.transform.position;
        this.SelectCharacter(0);
        this.parkourCamera = UnityEngine.Object.FindObjectOfType<CameraCtrl>();
        this.parkourCamera.Init();
        ParkourEvent._instance.Init();
    }

    public void Pause(bool _isPause)
    {
        this.isPause = _isPause;
        if (this.isPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Resurrection()
    {
        this.cCtrl.animFsm.ResetAnim();
        this.cCtrl.animFsm.PlayAnim(AnimNmae(CharacterType.RESURRECTION), 1f, 0.1f, false);
        this.cCtrl.animFsm.ResetAnim();
        base.StartCoroutine(this.cCtrl.GameStart("PK_Resurrection"));
    }

    public void SelectCharacter(int entry)
    {
        if (this.cardP != null)
        {
            UnityEngine.Object.Destroy(this.cardP);
        }
        CombatEquip equip = new CombatEquip();
        if (ConfigMgr.getInstance().getByEntry<guildrun_character_config>(entry).for_card_entry == 0)
        {
            equip.entry = 0xfc;
        }
        else if (ConfigMgr.getInstance().getByEntry<guildrun_character_config>(entry).for_card_entry == 13)
        {
            equip.entry = 0x28b;
        }
        else if (ConfigMgr.getInstance().getByEntry<guildrun_character_config>(entry).for_card_entry == 0x13)
        {
            equip.entry = 0x149;
        }
        this.cardP = CardPlayer.CreateCardPlayer(ConfigMgr.getInstance().getByEntry<guildrun_character_config>(entry).for_card_entry, equip, CardPlayerStateType.Battle, 2);
        this.cardP.transform.parent = this.cCtrl.characterTr.transform;
        this.cardP.transform.localPosition = Vector3.zero;
        this.cardP.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
        this.cardP.animation.Play("daiji");
        ParkourInit._instance.characterEntry = entry;
        Debug.Log("Anim prefab name:" + this.cardP.name);
    }

    private void Start()
    {
        this.Init();
        ParkourInit._instance.LoadMapInfo();
        if (SoundManager.mInstance != null)
        {
            SoundManager.mInstance.PlayMusic("Battle_Music02");
        }
    }

    private void Update()
    {
        if (this.GameStart)
        {
            Vector3 vector = _instance.cCtrl.thisT.position - GameStartPos;
            this.runDistance = vector.z;
            if (PaokuInPanel._instance != null)
            {
                PaokuInPanel._instance.UpdataProgress(this.cCtrl.parkouerInfo.GetRunPercent);
                ParkourEvent event1 = ParkourEvent._instance;
                event1.gameTime += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    this.Pause(true);
                    GUIMgr.Instance.DoModelGUI("PaokuPausePanel", null, null);
                }
            }
        }
    }

    public bool GameStart
    {
        get
        {
            return this.m_GameStart;
        }
        set
        {
            this.m_GameStart = value;
            if (this.m_GameStart)
            {
                this.Pause(false);
                this.cCtrl.isAction = true;
            }
            else if (!this.m_GameStart)
            {
                this.cCtrl.isAction = false;
            }
        }
    }
}

