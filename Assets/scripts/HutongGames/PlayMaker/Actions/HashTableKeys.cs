namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Store all the keys of a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy) into a PlayMaker arrayList Proxy component (PlayMakerArrayListProxy)."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableKeys : HashTableActions
    {
        [RequiredField, ActionSection("Result"), CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component that will store the keys")]
        public FsmOwnerDefault arrayListGameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component that will store the keys ( necessary if several component coexists on the same GameObject")]
        public FsmString arrayListReference;
        [CheckForComponent(typeof(PlayMakerHashTableProxy)), ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doHashTableKeys()
        {
            if (base.isProxyValid())
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.arrayListGameObject);
                if (ownerDefaultTarget != null)
                {
                    PlayMakerArrayListProxy proxy = base.GetArrayListProxyPointer(ownerDefaultTarget, this.arrayListReference.Value, false);
                    if (proxy != null)
                    {
                        proxy.arrayList.AddRange(base.proxy.hashTable.Keys);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doHashTableKeys();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.arrayListGameObject = null;
            this.arrayListReference = null;
        }
    }
}

