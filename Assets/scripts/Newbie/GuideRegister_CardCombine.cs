namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;

    public class GuideRegister_CardCombine
    {
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<CardOriginal, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache2;

        public static GuideController RegisterCardCombineGuide()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate (CardOriginal co) {
                    if (co.ori != null)
                    {
                        return false;
                    }
                    return CardPool.CardCanCombine(co);
                };
            }
            Func<CardOriginal, bool> func = <>f__am$cache1;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = () => Utility.CheckHaveCardCanbeCombine();
            }
            Func<bool> func2 = <>f__am$cache2;
            return GuideRegister_Hero.RegisterSelectHeroGuide(GuideEvent.CardCombine_CombineHero, func2, func);
        }

        public static GuideController RegisterCardCombinePortalGuide()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = () => Utility.CheckHaveCardCanbeCombine();
            }
            Func<bool> func = <>f__am$cache0;
            GuideController controller = GuideRegister_Hero.RegisterHeroPortal(GuideEvent.CardCombine_Portal, func);
            return new TalkBoxController(0x39) { FSM = { condition_reached = func }, next_step = controller };
        }
    }
}

