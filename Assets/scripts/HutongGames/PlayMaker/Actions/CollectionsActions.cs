﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Collections base action - don't use!")]
    public abstract class CollectionsActions : FsmStateAction
    {
        protected CollectionsActions()
        {
        }

        protected PlayMakerArrayListProxy GetArrayListProxyPointer(GameObject aProxy, string nameReference, bool silent)
        {
            if (aProxy == null)
            {
                if (!silent)
                {
                    Debug.LogError("Null Proxy");
                }
                return null;
            }
            PlayMakerArrayListProxy[] components = aProxy.GetComponents<PlayMakerArrayListProxy>();
            if (components.Length > 1)
            {
                if ((nameReference == string.Empty) && !silent)
                {
                    Debug.LogError("Several ArrayList Proxies coexists on the same GameObject and no reference is given to find the expected ArrayList");
                }
                foreach (PlayMakerArrayListProxy proxy in components)
                {
                    if (proxy.referenceName == nameReference)
                    {
                        return proxy;
                    }
                }
                if (nameReference != string.Empty)
                {
                    if (!silent)
                    {
                        this.LogError("ArrayList Proxy not found for reference <" + nameReference + ">");
                    }
                    return null;
                }
            }
            else if (components.Length > 0)
            {
                if (!(nameReference != string.Empty) || !(nameReference != components[0].referenceName))
                {
                    return components[0];
                }
                if (!silent)
                {
                    Debug.LogError("ArrayList Proxy reference do not match");
                }
                return null;
            }
            if (!silent)
            {
                this.LogError("ArrayList proxy not found");
            }
            return null;
        }

        protected PlayMakerHashTableProxy GetHashTableProxyPointer(GameObject aProxy, string nameReference, bool silent)
        {
            if (aProxy == null)
            {
                if (!silent)
                {
                    Debug.LogError("Null Proxy");
                }
                return null;
            }
            PlayMakerHashTableProxy[] components = aProxy.GetComponents<PlayMakerHashTableProxy>();
            if (components.Length > 1)
            {
                if ((nameReference == string.Empty) && !silent)
                {
                    Debug.LogWarning("Several HashTable Proxies coexists on the same GameObject and no reference is given to find the expected HashTable");
                }
                foreach (PlayMakerHashTableProxy proxy in components)
                {
                    if (proxy.referenceName == nameReference)
                    {
                        return proxy;
                    }
                }
                if (nameReference != string.Empty)
                {
                    if (!silent)
                    {
                        Debug.LogError("HashTable Proxy not found for reference <" + nameReference + ">");
                    }
                    return null;
                }
            }
            else if (components.Length > 0)
            {
                if (!(nameReference != string.Empty) || !(nameReference != components[0].referenceName))
                {
                    return components[0];
                }
                if (!silent)
                {
                    Debug.LogError("HashTable Proxy reference do not match");
                }
                return null;
            }
            if (!silent)
            {
                Debug.LogError("HashTable not found");
            }
            return null;
        }

        public enum FsmVariableEnum
        {
            FsmGameObject,
            FsmInt,
            FsmFloat,
            FsmString,
            FsmBool,
            FsmVector2,
            FsmVector3,
            FsmRect,
            FsmQuaternion,
            FsmColor,
            FsmMaterial,
            FsmTexture,
            FsmObject
        }
    }
}

