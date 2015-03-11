namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Factory
    {
        private static Dictionary<string, System.Type> type_dic = new Dictionary<string, System.Type>();

        public static Concentrator CreateConcentrator(string key)
        {
            return (CreateInstance(key) as Concentrator);
        }

        public static object CreateInstance(string key)
        {
            if (type_dic.Count <= 0)
            {
                SystemRegister();
            }
            System.Type type = null;
            if (!type_dic.TryGetValue(key, out type))
            {
                throw new UnityException("failed to create instance typed " + key);
            }
            return Activator.CreateInstance(type);
        }

        public static Transition CreateTransition(string key)
        {
            return (CreateInstance(key) as Transition);
        }

        private static void Register<T>()
        {
            System.Type type = typeof(T);
            type_dic.Add(type.Name, type);
        }

        private static void SystemRegister()
        {
            Register<Transition>();
            Register<TransmitProxy>();
            Register<Property>();
            Register<Closure>();
            Register<PivotConcent>();
            Register<SelfBuffRemove>();
            Register<ConditionConcent>();
            Register<NumericVariable>();
            Register<ResetConcentrator>();
            Register<Counter>();
            Register<Variable>();
            Register<SharedVariable>();
            Register<ListVariable>();
            Register<DynamicCounter>();
            Register<Terminator>();
            Register<Timer>();
            Register<Ticker>();
            Register<Loop>();
            Register<Imperfection>();
            Register<EqualComparer>();
            Register<LessComparer>();
            Register<GreatComparer>();
            Register<LessOrEqualComparer>();
            Register<GreatOrEqualComparer>();
            Register<AndExpression>();
            Register<OrExpression>();
            Register<NotExpression>();
            Register<Vertor3Variable>();
            Register<Magnitude>();
            Register<RandomConcentrator>();
            Register<MathAbs>();
            Register<MathAdd>();
            Register<MathSub>();
            Register<MathMul>();
            Register<MathDiv>();
            Register<OwnerSub>();
            Register<OwnerAdd>();
            Register<AddToList>();
            Register<Contains>();
            Register<Access>();
            Register<SkillPivotConcent>();
            Register<SkillCasterConstant>();
            Register<SkillTargetConstant>();
            Register<SkillActivityTarget>();
            Register<SkillTargetSetConstant>();
            Register<SkillFighters>();
            Register<ActorHP>();
            Register<ActorMaxHP>();
            Register<ActorEnergy>();
            Register<ListCountConstant>();
            Register<TeamMembersConstant>();
            Register<EnemyTeamMembersConstant>();
            Register<TeamMembersDeadConstant>();
            Register<SkillHitTypeConstant>();
            Register<SKillHurtTypeConstant>();
            Register<SkillDoSubSkill>();
            Register<DoSubSkillByEntry>();
            Register<SkillDoEffect>();
            Register<SkillDoCast>();
            Register<HitTypeSwitchOver>();
            Register<HurtTypeSwitchOver>();
            Register<SubSkillIdentity>();
            Register<SubSkillList>();
            Register<SkillActorDistance>();
            Register<ActorPosition>();
            Register<BuffList>();
            Register<BuffStateList>();
            Register<ListCount>();
            Register<RemoveBuff>();
            Register<SetBuffActive>();
            Register<SetBuffActiveTime>();
            Register<ReduceHP>();
            Register<ReduceHPByNum>();
            Register<ChangeEnergyByNum>();
            Register<ResetBuff>();
            Register<ResetBuffState>();
            Register<SkillDamageConstant>();
            Register<BuffSkillCastTimesConstant>();
            Register<EnergyChangeConstant>();
        }
    }
}

