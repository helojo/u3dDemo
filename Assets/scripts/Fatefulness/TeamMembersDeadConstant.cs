namespace Fatefulness
{
    using Battle;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TeamMembersDeadConstant : Concentrator
    {
        private AiManager aimgr;
        private List<object> member_list = new List<object>();
        private AiActor oneof;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("parameter1", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with team members getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the team members getter!");
            }
            if (this.aimgr == null)
            {
                this.aimgr = intent2.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                if (this.aimgr == null)
                {
                    throw new UnityException("need global context about AIManager to explain the team members getter!");
                }
            }
            if (this.oneof == null)
            {
                Transition port = base.GetPort("parameter1");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named parameter1");
                }
                Intent intent3 = new Intent(Intent.IntentType.review);
                port.Review(intent3);
                this.oneof = intent3.GetObject(IntentDecl.variable) as AiActor;
            }
            if ((this.member_list.Count <= 0) && (this.oneof != null))
            {
                AiTeam selfTeam = this.oneof.SelfTeam;
                if (selfTeam == null)
                {
                    throw new UnityException("unexsited team object of myself");
                }
                foreach (AiActor actor in selfTeam.DeadActorList)
                {
                    this.member_list.Add(actor);
                }
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.list_datasource, this.member_list);
        }

        public override Transition Launcher()
        {
            return base.GetPort("parameter1");
        }

        public override string Name()
        {
            return "TeamMembersDead";
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
            this.oneof = null;
            this.member_list.Clear();
        }
    }
}

