using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    private Dictionary<CharacterType, System.Action> actions = new Dictionary<CharacterType, System.Action>();
    [HideInInspector]
    public AnimFSM animFsm;
    private System.Action changeTrackAction;
    private float changeTrackTime;
    public Transform characterTr;
    private RaycastHit distanceHit;
    private Ray distanceRay;
    private RaycastHit downHit;
    private Ray downRay;
    private Vector3 downRayStartPos;
    private bool endFlyAnim;
    private float endFlyAnimTime;
    public float fastSpeed = 1f;
    public bool fly;
    public Vector3 flyHight;
    public float forwordSpeed;
    public bool isAction;
    public bool isAttack;
    public bool isCanAttack;
    public bool isChangeTrack;
    private bool isFly;
    public bool isHit;
    private bool isJump;
    private bool isTouch;
    private System.Action jumpAction;
    public float jumpAnimTime;
    public Vector3 jumpHight;
    public float jumpTime;
    private Vector3 leftDicPos = ((Vector3) ((Vector3.right * 2f) + (Vector3.forward * 1.5f)));
    private RaycastHit leftHit;
    private Ray leftRay;
    public bool m_action;
    private System.Action moveAction;
    public CharacterType nextActionKey;
    public int nextTrackDic;
    private int nowTrack = 1;
    private bool OpenCheckStairs;
    public ParkourerInfo parkouerInfo;
    private Vector3 rightDicPos = ((Vector3) ((-Vector3.right * 2f) + (Vector3.forward * 1.5f)));
    private RaycastHit rightHit;
    private Ray rightRay;
    private float startFlyAnimTime;
    private float startFlyTime = 0.5f;
    private Vector3 targetFlyPos;
    private Vector3 targetJumpPos;
    public int targetTrack = 1;
    [HideInInspector]
    public Transform thisT;
    private Collider tiggerCollider;
    private float tiggerRarDistance = 1.5f;
    private Vector3 tiggerRayStartPos;
    private Vector2 touchStartPos;
    public float trackAnimTime;
    private int trackDic;
    private float trackPosX;
    public Transform[] tracksPos;
    public Transform trackTr;
    public LayerMask twoSidesLayerMask;
    public float twoSidesRayDistance = 2.5f;
    private Vector3 twoSidesStartPos;
    public Vector3 velicoy = new Vector3(0f, 1f, 0f);
    private readonly float yDistance = 1.55f;
    private readonly float yOffset = 0.3f;
    private readonly float ySpeed = 0.1f;

    private bool AddAction(CharacterType type, System.Action action)
    {
        System.Action action2 = null;
        if (this.actions.TryGetValue(type, out action2))
        {
            return false;
        }
        this.actions.Add(type, action);
        return true;
    }

    private void ChangeTrack()
    {
        if (this.changeTrackTime < this.trackAnimTime)
        {
            this.changeTrackTime += Time.deltaTime;
            this.characterTr.position = Vector3.Lerp(this.characterTr.position, new Vector3(this.trackPosX, this.characterTr.position.y, this.characterTr.position.z), this.changeTrackTime / this.trackAnimTime);
        }
        else
        {
            this.nowTrack = this.targetTrack;
            this.changeTrackTime = 0f;
            this.isChangeTrack = false;
            if (this.trackDic == 1)
            {
                this.RemoveAction(CharacterType.RIGHT);
            }
            else if (this.trackDic == -1)
            {
                this.RemoveAction(CharacterType.LEFT);
            }
            this.CheckNextChangeType();
        }
    }

    public int CharacterTwoSidesTiggerCheck()
    {
        this.twoSidesStartPos = this.characterTr.position + new Vector3(0f, 0.5f, 0f);
        this.rightRay = new Ray(this.twoSidesStartPos, this.rightDicPos);
        this.leftRay = new Ray(this.twoSidesStartPos, this.leftDicPos);
        Debug.DrawRay(this.twoSidesStartPos, (Vector3) (this.rightDicPos * this.twoSidesRayDistance), Color.red);
        Debug.DrawRay(this.twoSidesStartPos, (Vector3) (this.leftDicPos * this.twoSidesRayDistance), Color.green);
        if (Physics.Raycast(this.rightRay, out this.rightHit, this.twoSidesRayDistance, (int) this.twoSidesLayerMask))
        {
            if (Physics.Raycast(this.leftRay, out this.leftHit, this.twoSidesRayDistance, (int) this.twoSidesLayerMask))
            {
                return 2;
            }
            return -1;
        }
        if (Physics.Raycast(this.leftRay, out this.leftHit, this.twoSidesRayDistance, (int) this.twoSidesLayerMask))
        {
            return 1;
        }
        return 0;
    }

    private void CheckAttack()
    {
        if (!this.isAttack && this.isCanAttack)
        {
            this.StartAttack();
        }
    }

    private void CheckChangeTrack(int dic)
    {
        if (this.isChangeTrack)
        {
            Debug.LogWarning("isChangeTrack");
            this.nextActionKey = (this.trackDic <= 0) ? CharacterType.LEFT : CharacterType.RIGHT;
            this.nextTrackDic = dic;
        }
        else
        {
            this.trackDic = dic;
            this.StartChangeTrack();
        }
    }

    public void CheckFly()
    {
        if (this.isFly)
        {
            Debug.LogWarning("isFly");
        }
        else
        {
            this.StartFly();
        }
    }

    private void CheckJump()
    {
        if (this.isJump)
        {
            Debug.LogWarning("isJump");
            this.nextActionKey = CharacterType.JUMP;
        }
        else
        {
            this.StartJump();
        }
    }

    public void CheckMove()
    {
        if (!this.actions.TryGetValue(CharacterType.MOVE, out this.moveAction))
        {
            this.AddAction(CharacterType.MOVE, new System.Action(this.Move));
            this.GetAction(CharacterType.MOVE);
        }
        this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.MOVE), 1f, 0.01f, true);
    }

    private void CheckNextChangeType()
    {
        if (this.nextActionKey != CharacterType.None)
        {
            if ((this.nextActionKey == CharacterType.LEFT) || (this.nextActionKey == CharacterType.RIGHT))
            {
                Debug.Log("next Track Dic:" + this.nextTrackDic);
                this.trackDic = this.nextTrackDic;
                this.StartChangeTrack();
            }
            else if (this.nextActionKey == CharacterType.JUMP)
            {
                this.CheckJump();
            }
            this.nextActionKey = CharacterType.None;
        }
    }

    private void CheckStairs()
    {
        this.thisT.position = new Vector3(this.thisT.position.x, this.GroundWhthCharacterTheDistance().point.y + this.yOffset, this.thisT.position.z);
    }

    private void Fly()
    {
        if (this.fly)
        {
            if (this.startFlyAnimTime < this.animFsm.GetStateClipLength(ParkourManager.AnimNmae(CharacterType.STARTFLY)))
            {
                this.startFlyAnimTime += Time.deltaTime;
                this.targetFlyPos = new Vector3(this.characterTr.localPosition.x, this.targetFlyPos.y, this.characterTr.localPosition.z);
                this.characterTr.localPosition = Vector3.Lerp(this.characterTr.localPosition, this.targetFlyPos, this.startFlyAnimTime / this.animFsm.GetStateClipLength(ParkourManager.AnimNmae(CharacterType.STARTFLY)));
            }
        }
        else if (!this.fly)
        {
            if (this.endFlyAnimTime < this.animFsm.GetStateClipLength(ParkourManager.AnimNmae(CharacterType.ENDFLY)))
            {
                this.isAction = false;
                if (!this.endFlyAnim)
                {
                    this.endFlyAnim = true;
                    ParkourEvent._instance.PlayEffect("pk_tx_21", false);
                    this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.ENDFLY), 1f, 0.01f, false);
                    this.forwordSpeed /= 2f;
                }
                this.endFlyAnimTime += Time.deltaTime;
                this.targetFlyPos = new Vector3(this.characterTr.localPosition.x, this.yOffset, this.characterTr.localPosition.z);
                this.characterTr.localPosition = Vector3.Lerp(this.characterTr.localPosition, this.targetFlyPos, this.endFlyAnimTime / this.animFsm.GetStateClipLength(ParkourManager.AnimNmae(CharacterType.ENDFLY)));
            }
            else
            {
                ParkourEvent._instance.PlayEffect("pk_tx_20", true);
                Debug.Log("Remove fly....");
                this.endFlyAnim = false;
                this.isAction = true;
                this.isFly = false;
                this.fly = false;
                this.RemoveAction(CharacterType.FLY);
            }
        }
    }

    [DebuggerHidden]
    public IEnumerator GameStart(string cameraAnimName)
    {
        return new <GameStart>c__IteratorBE { cameraAnimName = cameraAnimName, <$>cameraAnimName = cameraAnimName, <>f__this = this };
    }

    public bool GetAction(CharacterType type)
    {
        System.Action action = null;
        if (this.actions.TryGetValue(type, out action))
        {
            action();
            return true;
        }
        return false;
    }

    public RaycastHit GroundWhthCharacterTheDistance()
    {
        this.downRayStartPos = new Vector3(this.thisT.position.x, this.thisT.position.y + 1f, this.thisT.position.z);
        this.downRay = new Ray(this.downRayStartPos, -Vector3.up);
        if (Physics.Raycast(this.downRay, out this.downHit))
        {
            return this.downHit;
        }
        return this.downHit;
    }

    public void Init()
    {
        this.thisT = base.transform;
        this.forwordSpeed = CharacterFSMManager.Instance().GetStateInfo(CharacterType.MOVE).speed;
        this.animFsm = this.characterTr.GetComponentInChildren<AnimFSM>();
        this.animFsm.SetStateTable("Parkour");
        this.trackAnimTime = this.animFsm.GetStateClipLength("Left") * 0.6f;
        this.jumpAnimTime = this.animFsm.GetStateClipLength("Jump");
    }

    private void Jump()
    {
        if (this.jumpTime < (this.jumpAnimTime * 0.5f))
        {
            this.jumpTime += Time.deltaTime;
            Vector3 to = new Vector3(this.characterTr.localPosition.x, this.targetJumpPos.y, this.characterTr.localPosition.z);
            this.characterTr.localPosition = Vector3.Lerp(this.characterTr.localPosition, to, this.jumpTime / (this.jumpAnimTime * 0.5f));
        }
        else if (this.jumpTime < this.jumpAnimTime)
        {
            this.jumpTime += Time.deltaTime;
            this.characterTr.localPosition = Vector3.Lerp(this.characterTr.localPosition, new Vector3(this.characterTr.localPosition.x, 0f, this.characterTr.localPosition.z), (this.jumpTime - (this.jumpAnimTime * 0.5f)) / (this.jumpAnimTime * 0.5f));
        }
        else
        {
            this.jumpTime = 0f;
            this.isJump = false;
            this.RemoveAction(CharacterType.JUMP);
            this.CheckNextChangeType();
        }
    }

    public void LateUpdate()
    {
        if (ParkourManager._instance.GameStart && !ParkourManager._instance.isPause)
        {
            this.forwordSpeed = (((ParkourManager._instance.runDistance / 1000f) * 0.09999999f) + CharacterFSMManager.Instance().GetStateInfo(CharacterType.MOVE).speed) * this.fastSpeed;
            this.GetAction(CharacterType.JUMP);
            if (!this.isHit)
            {
                this.GetAction(CharacterType.LEFT);
                this.GetAction(CharacterType.RIGHT);
            }
            this.GetAction(CharacterType.FLY);
            this.GetAction(CharacterType.MOVE);
        }
    }

    private void Move()
    {
        this.thisT.Translate((Vector3) (Vector3.forward * this.forwordSpeed));
        this.trackTr.position = new Vector3(this.trackTr.position.x, this.characterTr.position.y, this.characterTr.position.z);
    }

    private bool RemoveAction(CharacterType type)
    {
        System.Action action = null;
        if (!this.actions.TryGetValue(type, out action))
        {
            Debug.Log("Con't find Action : " + type);
            return false;
        }
        this.actions.Remove(type);
        return true;
    }

    private bool ResetChangeTrackVar()
    {
        this.targetTrack = this.nowTrack + this.trackDic;
        if (((this.targetTrack > 2) && (this.trackDic > 0)) || ((this.targetTrack < 0) && (this.trackDic < 0)))
        {
            Debug.LogWarning(string.Concat(new object[] { "Track Error!", this.targetTrack, " : ", this.trackDic }));
            this.targetTrack = (this.trackDic <= 0) ? 0 : 2;
            this.isChangeTrack = false;
            return false;
        }
        this.trackPosX = this.tracksPos[this.targetTrack].position.x;
        return true;
    }

    private void StartAttack()
    {
        this.isAttack = true;
        this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.ATTACK), 1f, 0.01f, false);
    }

    private void StartChangeTrack()
    {
        this.isChangeTrack = true;
        if (this.ResetChangeTrackVar())
        {
            if (SoundManager.mInstance != null)
            {
                SoundManager.mInstance.PlaySFX("sound_ui_t_7");
            }
            if (this.trackDic == 1)
            {
                if (this.isFly)
                {
                    this.trackAnimTime = this.animFsm.GetStateClipLength("FlyRight");
                    this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.FLYRIGHT), 1f, 0.01f, false);
                }
                else
                {
                    this.trackAnimTime = this.animFsm.GetStateClipLength("Left") * 0.6f;
                    this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.RIGHT), 1f, 0.01f, false);
                }
                this.AddAction(CharacterType.RIGHT, new System.Action(this.ChangeTrack));
            }
            else if (this.trackDic == -1)
            {
                if (this.isFly)
                {
                    this.trackAnimTime = this.animFsm.GetStateClipLength("FlyRight");
                    this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.FLYLEFT), 1f, 0.01f, false);
                }
                else
                {
                    this.trackAnimTime = this.animFsm.GetStateClipLength("Left") * 0.6f;
                    this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.LEFT), 1f, 0.01f, false);
                }
                this.AddAction(CharacterType.LEFT, new System.Action(this.ChangeTrack));
            }
        }
    }

    private void StartFly()
    {
        Debug.Log("Start Fly....");
        this.isFly = true;
        this.fly = true;
        this.startFlyAnimTime = 0f;
        this.targetFlyPos = this.flyHight;
        this.RemoveAction(CharacterType.JUMP);
        this.isJump = false;
        this.forwordSpeed *= 2f;
        this.endFlyAnimTime = 0f;
        this.AddAction(CharacterType.FLY, new System.Action(this.Fly));
        this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.STARTFLY), 1f, 0.01f, false);
        ParkourEvent._instance.PlayEffect("pk_tx_21", true);
    }

    private void StartJump()
    {
        if (SoundManager.mInstance != null)
        {
            SoundManager.mInstance.PlaySFX("sound_skill_lr_2_qt_use");
        }
        this.isJump = true;
        this.targetJumpPos = this.characterTr.localPosition + this.jumpHight;
        this.AddAction(CharacterType.JUMP, new System.Action(this.Jump));
        this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.JUMP), 1f, 0.01f, false);
    }

    public void StopMove()
    {
        if (this.GetAction(CharacterType.MOVE))
        {
            this.RemoveAction(CharacterType.MOVE);
        }
    }

    public bool TiggerCharacterTheRayHit(out Collider col)
    {
        this.tiggerRayStartPos = this.characterTr.position + new Vector3(0f, 0.5f, 0f);
        this.distanceRay = new Ray(this.tiggerRayStartPos, this.characterTr.forward);
        Debug.DrawRay(this.tiggerRayStartPos, (Vector3) (this.characterTr.forward * this.tiggerRarDistance), Color.blue);
        if (Physics.Raycast(this.distanceRay, out this.distanceHit, this.tiggerRarDistance))
        {
            if (this.distanceHit.distance < 1f)
            {
                col = this.distanceHit.collider;
                return true;
            }
            col = null;
            return false;
        }
        col = null;
        return false;
    }

    private void Update()
    {
        if (this.isAction)
        {
            if (this.TiggerCharacterTheRayHit(out this.tiggerCollider))
            {
                ParkourEvent._instance.CheckCollisionObj(this.tiggerCollider);
            }
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    this.touchStartPos = Input.GetTouch(0).position;
                    this.isTouch = true;
                    Debug.Log("Start touch....");
                }
                else if ((Input.GetTouch(0).phase == TouchPhase.Stationary) || (Input.GetTouch(0).phase == TouchPhase.Moved))
                {
                    if (this.isTouch)
                    {
                        Debug.Log(1);
                        if (Vector2.Distance(Input.GetTouch(0).position, this.touchStartPos) > 15f)
                        {
                            Debug.Log(2);
                            if (Mathf.Abs((float) (Input.GetTouch(0).position.x - this.touchStartPos.x)) > Mathf.Abs((float) (Input.GetTouch(0).position.y - this.touchStartPos.y)))
                            {
                                Debug.Log(3);
                                if ((Input.GetTouch(0).position.x - this.touchStartPos.x) > 0f)
                                {
                                    if ((this.CharacterTwoSidesTiggerCheck() != 1) && (this.CharacterTwoSidesTiggerCheck() != 2))
                                    {
                                        this.CheckChangeTrack(1);
                                    }
                                    else
                                    {
                                        this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.RIGHTHIT), 1f, 0.01f, false);
                                    }
                                }
                                else if ((Input.GetTouch(0).position.x - this.touchStartPos.x) < 0f)
                                {
                                    Debug.Log(5);
                                    if ((this.CharacterTwoSidesTiggerCheck() != -1) && (this.CharacterTwoSidesTiggerCheck() != 2))
                                    {
                                        this.CheckChangeTrack(-1);
                                    }
                                    else
                                    {
                                        this.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.LEFTHIT), 1f, 0.01f, false);
                                    }
                                }
                            }
                            else if (Mathf.Abs((float) (Input.GetTouch(0).position.x - this.touchStartPos.x)) < Mathf.Abs((float) (Input.GetTouch(0).position.y - this.touchStartPos.y)))
                            {
                                Debug.Log(6);
                                if (((Input.GetTouch(0).position.y - this.touchStartPos.y) > 0f) && !this.isFly)
                                {
                                    this.CheckJump();
                                }
                                else if (((Input.GetTouch(0).position.y - this.touchStartPos.y) < 0f) && !this.isFly)
                                {
                                    this.CheckAttack();
                                }
                            }
                            this.isTouch = false;
                        }
                    }
                }
                else if ((Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.GetTouch(0).phase == TouchPhase.Canceled))
                {
                    this.isTouch = false;
                    this.touchStartPos = Vector2.zero;
                }
            }
            if (this.OpenCheckStairs)
            {
                this.CheckStairs();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <GameStart>c__IteratorBE : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>cameraAnimName;
        internal CharacterCtrl <>f__this;
        internal string cameraAnimName;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.Init();
                    if (this.<>f__this.parkouerInfo != null)
                    {
                        this.<>f__this.parkouerInfo = null;
                    }
                    this.<>f__this.parkouerInfo = new ParkourerInfo(ParkourInit._instance.characterEntry);
                    ParkourManager._instance.parkourCamera.transform.parent.animation.Play(this.cameraAnimName);
                    this.$current = new WaitForSeconds(ParkourManager._instance.parkourCamera.transform.parent.animation[this.cameraAnimName].length - 0.7f);
                    this.$PC = 1;
                    goto Label_0167;

                case 1:
                    if (this.cameraAnimName == "PK_StartCamera")
                    {
                        this.<>f__this.animFsm.transform.localEulerAngles = Vector3.zero;
                    }
                    this.$current = new WaitForSeconds(0.7f);
                    this.$PC = 2;
                    goto Label_0167;

                case 2:
                    ParkourManager._instance.parkourCamera.cameraFollow = true;
                    this.<>f__this.CheckMove();
                    ParkourManager._instance.GameStart = true;
                    this.<>f__this.StartCoroutine(this.<>f__this.parkouerInfo.SecondesLossHP());
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0167:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

