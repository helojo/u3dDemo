namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GuideController
    {
        public bool actived;
        protected GuideFSM fsm;
        public GuideController next_step;
        protected Visualization visual;

        public virtual void BeginStep()
        {
            this.actived = true;
            this.FSM.actived_status = GuideFSM.GuideStatus.Prepare;
        }

        public void Cancel()
        {
            this.actived = false;
            if (this.visual != null)
            {
                this.visual.CreateEntity(new Action<NewbieEntity>(this.OnCancel));
            }
        }

        public void Complete()
        {
            this.actived = false;
            Utility.NewbiestUnlock();
            Utility.EnforceClear();
        }

        public bool ConditionReachable()
        {
            return ((this.FSM.condition_reached == null) || this.FSM.condition_reached());
        }

        public void GoNext()
        {
            if (this.visual != null)
            {
                this.visual.CreateEntity(new Action<NewbieEntity>(this.OnGoNext));
            }
        }

        private void OnCancel(NewbieEntity entity)
        {
            entity.Reset();
        }

        private void OnGoNext(NewbieEntity entity)
        {
            this.actived = false;
            entity.Reset();
            if (this.next_step != null)
            {
                this.next_step.BeginStep();
            }
        }

        public void RequestAwake()
        {
            if (!this.FSM.WaittingAwake())
            {
                Utility.NewbiestUnlock();
            }
            else
            {
                this.FSM.OnReceive(GuideFSM.GuideStatus.Awake, null);
            }
        }

        public void RequestCancel()
        {
            if (!this.FSM.WaittingResponse())
            {
                Utility.NewbiestUnlock();
            }
            else
            {
                this.FSM.OnReceive(GuideFSM.GuideStatus.UICancel, null);
            }
        }

        public void RequestGeneration(string tag, GameObject obj)
        {
            List<GameObject> vari = new List<GameObject> {
                obj
            };
            this.RequestMultiGeneration(tag, vari);
        }

        public void RequestMultiGeneration(string tag, List<GameObject> vari)
        {
            if (!this.FSM.WaittingGenerate())
            {
                Utility.NewbiestUnlock();
            }
            else
            {
                UIGenerateParameter parameter = new UIGenerateParameter {
                    tag = tag,
                    focuses = vari
                };
                this.FSM.OnReceive(GuideFSM.GuideStatus.UIGenerate, parameter);
            }
        }

        public void RequestUIResponse(string tag, GameObject vari)
        {
            if (!this.FSM.WaittingResponse())
            {
                Utility.NewbiestUnlock();
            }
            else
            {
                UIResponseParameter parameter = new UIResponseParameter {
                    tag = tag,
                    ui_object = vari
                };
                this.FSM.OnReceive(GuideFSM.GuideStatus.UIResponse, parameter);
            }
        }

        public GuideFSM FSM
        {
            get
            {
                if (this.fsm == null)
                {
                    this.fsm = new GuideFSM();
                }
                return this.fsm;
            }
        }

        public Visualization Visual
        {
            get
            {
                return this.visual;
            }
            set
            {
                this.visual = value;
            }
        }
    }
}

