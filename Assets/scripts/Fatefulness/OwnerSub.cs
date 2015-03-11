namespace Fatefulness
{
    using System;

    public class OwnerSub : OwnerAdd
    {
        public override void Excude(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    base.addtional++;
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, (float) (base.original - base.addtional));
                    break;
            }
        }

        public override string Name()
        {
            return "--";
        }
    }
}

