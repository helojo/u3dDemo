using HutongGames.PlayMaker;
using System;
using UnityEngine;

public class NguiEventForwarder : MonoBehaviour
{
    public bool debug;
    public PlayMakerFSM targetFSM;

    private void ForwardNGUIEvent(NGuiPlayMakerEventsEnum nguiEvent)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            if (this.debug)
            {
                Debug.Log(string.Format("NGUI Event Forwarder - Sending {0} to {1} > {2}", nguiEvent.ToString(), this.targetFSM.gameObject.name, this.targetFSM.FsmName));
            }
            this.targetFSM.SendEvent(NGuiPlayMakerEvents.GetFsmEventEnumValue(nguiEvent));
        }
    }

    private void OnClick()
    {
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnClickEvent);
    }

    private void OnDoubleClick()
    {
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnDoubleClickEvent);
    }

    private void OnDrag(Vector2 delta)
    {
        Fsm.EventData.Vector3Data = new Vector3(delta.x, delta.y);
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnDragEvent);
    }

    private void OnDragFinished()
    {
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnDragFinishedEvent);
    }

    private void OnDrop(GameObject drag)
    {
        Fsm.EventData.GameObjectData = drag;
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnDropEvent);
    }

    private void OnEnable()
    {
        if (this.targetFSM == null)
        {
            this.targetFSM = base.GetComponent<PlayMakerFSM>();
        }
        if (this.targetFSM == null)
        {
            base.enabled = false;
            Debug.LogWarning("No FSM Target assigned. NGUIEventForwarder is now disabled Name:" + base.transform.name);
        }
    }

    private void OnHover(bool isOver)
    {
        this.ForwardNGUIEvent(!isOver ? NGuiPlayMakerEventsEnum.OnHoverExitEvent : NGuiPlayMakerEventsEnum.OnHoverEnterEvent);
    }

    private void OnInput(string text)
    {
        Fsm.EventData.StringData = text;
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnInputEvent);
    }

    private void OnKey(KeyCode key)
    {
        Fsm.EventData.StringData = key.ToString();
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnKeyEvent);
    }

    private void OnPress(bool isDown)
    {
        this.ForwardNGUIEvent(!isDown ? NGuiPlayMakerEventsEnum.OnPresReleasedEvent : NGuiPlayMakerEventsEnum.OnPressDownEvent);
    }

    private void OnScroll(float delta)
    {
        Fsm.EventData.FloatData = delta;
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnScrollEvent);
    }

    private void OnScrollBarChange()
    {
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnScrollBarChangeEvent);
    }

    private void OnSelect(bool selected)
    {
        this.ForwardNGUIEvent(!selected ? NGuiPlayMakerEventsEnum.OnSelectReleasedEvent : NGuiPlayMakerEventsEnum.OnSelectDownEvent);
    }

    public void OnSelectionChange()
    {
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnSelectionChangeEvent);
    }

    public void OnSliderChange()
    {
        UISlider component = base.gameObject.GetComponent<UISlider>();
        if (component != null)
        {
            Fsm.EventData.FloatData = component.value;
        }
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnSliderChangeEvent);
    }

    public void OnSubmitChange()
    {
        this.ForwardNGUIEvent(NGuiPlayMakerEventsEnum.OnSubmitEvent);
    }

    private void OnTooltip(bool show)
    {
        this.ForwardNGUIEvent(!show ? NGuiPlayMakerEventsEnum.OnTooltipHideEvent : NGuiPlayMakerEventsEnum.OnTooltipShowEvent);
    }
}

