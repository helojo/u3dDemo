using System;

public enum NGuiPlayMakerEventsEnum
{
    [PlayMakerNGUI_FsmEvent("NGUI / ON CLICK")]
    OnClickEvent = 0,
    [PlayMakerNGUI_FsmEvent("NGUI / ON DOUBLE CLICK")]
    OnDoubleClickEvent = 1,
    [PlayMakerNGUI_FsmEvent("NGUI / ON DRAG")]
    OnDragEvent = 11,
    [PlayMakerNGUI_FsmEvent("NGUI / ON DRAG FINISHED")]
    OnDragFinishedEvent = 12,
    [PlayMakerNGUI_FsmEvent("NGUI / ON DROP")]
    OnDropEvent = 13,
    [PlayMakerNGUI_FsmEvent("NGUI / ON HOVER - HOVERING")]
    OnHoverEnterEvent = 5,
    [PlayMakerNGUI_FsmEvent("NGUI / ON HOVER - EXIT")]
    OnHoverExitEvent = 6,
    [PlayMakerNGUI_FsmEvent("NGUI / ON INPUT")]
    OnInputEvent = 2,
    [PlayMakerNGUI_FsmEvent("NGUI / ON KEY")]
    OnKeyEvent = 4,
    [PlayMakerNGUI_FsmEvent("NGUI / ON PRESS - RELEASED")]
    OnPresReleasedEvent = 8,
    [PlayMakerNGUI_FsmEvent("NGUI / ON PRESS - PRESSED")]
    OnPressDownEvent = 7,
    [PlayMakerNGUI_FsmEvent("NGUI / ON SCROLLBAR CHANGE")]
    OnScrollBarChangeEvent = 0x10,
    [PlayMakerNGUI_FsmEvent("NGUI / ON MOUSEWHEEL SCROLL")]
    OnScrollEvent = 3,
    [PlayMakerNGUI_FsmEvent("NGUI / ON SELECT - PRESSED")]
    OnSelectDownEvent = 9,
    [PlayMakerNGUI_FsmEvent("NGUI / ON SELECTION CHANGE")]
    OnSelectionChangeEvent = 0x11,
    [PlayMakerNGUI_FsmEvent("NGUI / ON SELECT - RELEASED")]
    OnSelectReleasedEvent = 10,
    [PlayMakerNGUI_FsmEvent("NGUI / ON SLIDER CHANGE")]
    OnSliderChangeEvent = 15,
    [PlayMakerNGUI_FsmEvent("NGUI / ON SUBMIT")]
    OnSubmitEvent = 14,
    [PlayMakerNGUI_FsmEvent("NGUI / ON TOOLTIP - HIDE")]
    OnTooltipHideEvent = 0x13,
    [PlayMakerNGUI_FsmEvent("NGUI / ON TOOLTIP - SHOW")]
    OnTooltipShowEvent = 0x12
}

