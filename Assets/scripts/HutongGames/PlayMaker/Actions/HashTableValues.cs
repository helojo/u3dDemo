namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Store all the values of a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy) into a PlayMaker arrayList Proxy component (PlayMakerArrayListProxy).")]
    public class HashTableValues : HashTableActions
    {
        [RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component that will store the values"), CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Result")]
        public FsmOwnerDefault arrayListGameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component that will store the values ( necessary if several component coexists on the same GameObject")]
        public FsmString arrayListReference;
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doHashTableValues()
        {
            if (base.isProxyValid())
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.arrayListGameObject);
                if (ownerDefaultTarget != null)
                {
                    PlayMakerArrayListProxy proxy = base.GetArrayListProxyPointer(ownerDefaultTarget, this.arrayListReference.Value, false);
                    if (proxy != null)
                    {
                        proxy.arrayList.AddRange(base.proxy.hashTable.Values);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doHashTableValues();
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

