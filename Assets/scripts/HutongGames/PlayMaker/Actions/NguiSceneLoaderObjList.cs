namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Tooltip("Sends each object in the list to the GPU for pre-rendering"), ActionCategory("NGUI")]
    public class NguiSceneLoaderObjList : FsmStateAction
    {
        private Camera CameraLoader;
        private GameObject CurrentGO;
        private LoaderInfo CurrentLoaderGO;
        [Tooltip("When set, introduces a delay between each object being loaded. Useful to force showing of the progressbar")]
        public FsmFloat DelayBetweenObjectLoads;
        private bool IsFinishedLoading;
        private UILabel LblProgressText;
        private readonly Queue<LoaderInfo> LoadingQ = new Queue<LoaderInfo>();
        [RequiredField, Tooltip("The progressbar")]
        public FsmGameObject NguiProgressbar;
        [Tooltip("The progressbar label")]
        public FsmGameObject NguiProgressbarLabel;
        [Tooltip("When checked, the camera will visit each child of the scene object.")]
        public FsmBool[] ParseChildrens;
        private float ProgressStepValue;
        [RequiredField, Tooltip("The camera used to load the objects into the GPU")]
        public FsmOwnerDefault SceneLoaderCamera;
        [Tooltip("GameObject to pre-render to GPU"), CompoundArray("PreRenderList", "SceneObject", "ParseChildren"), RequiredField]
        public FsmGameObject[] SceneObjects;
        private bool SetParentInactive;
        private bool SetToInactive;
        private UISlider SldProgressbar;
        private float TimeToLoadNextObject;

        private void MoveCameraToGO(GameObject goToLoad)
        {
            this.CurrentGO = goToLoad;
            if (!goToLoad.activeSelf)
            {
                this.SetToInactive = true;
                goToLoad.SetActive(true);
            }
            this.CameraLoader.transform.position = (Vector3) (goToLoad.transform.position * 2f);
            this.CameraLoader.transform.LookAt(goToLoad.transform);
            this.SetProgressbarValue(this.ProgressStepValue);
            if (this.DelayBetweenObjectLoads != null)
            {
                this.TimeToLoadNextObject = Time.time + this.DelayBetweenObjectLoads.Value;
            }
        }

        public override void OnEnter()
        {
            int num = 0;
            int length = this.SceneObjects.Length;
            for (int i = 0; i < length; i++)
            {
                if (this.SceneObjects[i] != null)
                {
                    LoaderInfo item = new LoaderInfo {
                        LoaderGO = this.SceneObjects[i].Value
                    };
                    num++;
                    if ((this.ParseChildrens[i] != null) && this.ParseChildrens[i].Value)
                    {
                        int childCount = item.LoaderGO.transform.childCount;
                        for (int j = 0; j < childCount; j++)
                        {
                            item.ChildQ.Enqueue(item.LoaderGO.transform.GetChild(j).gameObject);
                            num++;
                        }
                    }
                    this.LoadingQ.Enqueue(item);
                }
            }
            this.SldProgressbar = this.NguiProgressbar.Value.GetComponent<UISlider>();
            if (this.NguiProgressbarLabel != null)
            {
                this.LblProgressText = this.NguiProgressbarLabel.Value.GetComponent<UILabel>();
            }
            this.CameraLoader = base.Fsm.GetOwnerDefaultTarget(this.SceneLoaderCamera).GetComponent<Camera>();
            this.ProgressStepValue = ((float) num) / 10000f;
            this.SetProgressbarValue(0f);
        }

        public override void OnUpdate()
        {
            if (!this.IsFinishedLoading && (this.TimeToLoadNextObject <= Time.time))
            {
                if (this.CurrentGO != null)
                {
                    if (this.SetToInactive)
                    {
                        this.CurrentGO.SetActive(false);
                    }
                    this.SetToInactive = false;
                }
                if (this.CurrentLoaderGO != null)
                {
                    if (this.CurrentLoaderGO.ChildQ.Count != 0)
                    {
                        this.CurrentGO = this.CurrentLoaderGO.ChildQ.Dequeue();
                        this.SetToInactive = !this.CurrentGO.activeSelf;
                        this.MoveCameraToGO(this.CurrentGO);
                        return;
                    }
                    if (!this.CurrentLoaderGO.LoaderGO.Equals(this.CurrentGO))
                    {
                        this.MoveCameraToGO(this.CurrentLoaderGO.LoaderGO);
                        return;
                    }
                    if (this.SetParentInactive)
                    {
                        this.CurrentLoaderGO.LoaderGO.SetActive(false);
                    }
                    this.SetParentInactive = false;
                    this.CurrentLoaderGO = null;
                }
                if (this.LoadingQ.Count > 0)
                {
                    this.CurrentLoaderGO = this.LoadingQ.Dequeue();
                    if (this.CurrentLoaderGO.ChildQ.Count > 0)
                    {
                        this.CurrentGO = this.CurrentLoaderGO.ChildQ.Dequeue();
                        this.SetParentInactive = !this.CurrentLoaderGO.LoaderGO.activeSelf;
                        this.SetToInactive = !this.CurrentGO.activeSelf;
                        this.MoveCameraToGO(this.CurrentGO);
                    }
                    else
                    {
                        this.CurrentGO = this.CurrentLoaderGO.LoaderGO;
                        this.SetToInactive = !this.CurrentGO.activeSelf;
                        this.MoveCameraToGO(this.CurrentGO);
                    }
                }
                else
                {
                    this.IsFinishedLoading = true;
                    if (this.IsFinishedLoading)
                    {
                        base.Finish();
                    }
                }
            }
        }

        public override void Reset()
        {
            this.SceneLoaderCamera = null;
            this.NguiProgressbar = null;
            this.NguiProgressbarLabel = null;
            this.SceneObjects = null;
            this.ParseChildrens = null;
            this.DelayBetweenObjectLoads = null;
        }

        private void SetProgressbarValue(float increment)
        {
            if (increment <= 0f)
            {
                this.SldProgressbar.value = 0f;
            }
            else
            {
                this.SldProgressbar.value += increment;
            }
            if (this.LblProgressText != null)
            {
                this.LblProgressText.text = string.Format("{0:N2}%", this.SldProgressbar.value * 100f);
            }
        }
    }
}

