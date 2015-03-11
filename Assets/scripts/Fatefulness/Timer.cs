namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class Timer : Ticker
    {
        private AiManager _aimgr;

        public override string Name()
        {
            return "WaitFor_LogicTime";
        }

        public override void Reset()
        {
            base.Reset();
            this._aimgr = null;
        }

        public override void Update()
        {
            base.Update();
            base.DoTick();
        }

        private AiManager aimgr
        {
            get
            {
                if (this._aimgr == null)
                {
                    Intent intent = base.root_fragment.global_context;
                    if (intent == null)
                    {
                        throw new UnityException("need global context to explain the timer!");
                    }
                    this._aimgr = intent.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                    if (this._aimgr == null)
                    {
                        throw new UnityException("need global context about AIManager to explain the timer!");
                    }
                }
                return this._aimgr;
            }
        }

        protected override float CurrentTime
        {
            get
            {
                return this.aimgr.Time;
            }
        }
    }
}

