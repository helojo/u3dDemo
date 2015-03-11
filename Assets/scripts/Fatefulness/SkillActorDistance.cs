namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SkillActorDistance : Concentrator
    {
        private AiManager aimgr;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("From", Transition.Direction.input);
            base.RegisterPort<Transition>("To", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with skill actor distance concentrator!");
            }
            if (this.aimgr == null)
            {
                Intent intent2 = base.root_fragment.global_context;
                if (intent2 == null)
                {
                    throw new UnityException("need global context to explain the skill caster getter!");
                }
                this.aimgr = intent2.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                if (this.aimgr == null)
                {
                    throw new UnityException("need global context about AIManager to explain the skill caster getter!");
                }
            }
            Transition port = base.GetPort("From");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named From");
            }
            Intent intent3 = new Intent(Intent.IntentType.review);
            port.Review(intent3);
            AiActor actor = intent3.GetObject<AiActor>(IntentDecl.variable);
            if (actor == null)
            {
                throw new UnityException("can not find varibale named From");
            }
            Transition transition2 = base.GetPort("To");
            if (transition2 == null)
            {
                throw new UnityException("unexisted transition port named To");
            }
            transition2.Review(intent3);
            AiActor actor2 = intent3.GetObject<AiActor>(IntentDecl.variable);
            if (actor2 == null)
            {
                throw new UnityException("can not find varibale named To");
            }
            float distanceOfTwoActor = this.aimgr.GetDistanceOfTwoActor(actor, actor2);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, distanceOfTwoActor);
        }

        public override Transition Launcher()
        {
            return base.GetPort("From");
        }

        public override string Name()
        {
            return "Distance";
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
        }
    }
}

