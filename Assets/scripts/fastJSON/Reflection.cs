namespace fastJSON
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    internal sealed class Reflection
    {
        private SafeDictionary<Type, CreateObject> _constrcache = new SafeDictionary<Type, CreateObject>();
        private SafeDictionary<Type, List<Getters>> _getterscache = new SafeDictionary<Type, List<Getters>>();
        private SafeDictionary<Type, string> _tyname = new SafeDictionary<Type, string>();
        private SafeDictionary<string, Type> _typecache = new SafeDictionary<string, Type>();
        public static readonly Reflection Instance = new Reflection();
        public bool ShowReadOnlyProperties;

        private Reflection()
        {
        }

        internal static GenericGetter CreateGetField(Type type, FieldInfo fieldInfo)
        {
            Type[] parameterTypes = new Type[] { typeof(object) };
            DynamicMethod method = new DynamicMethod("_", typeof(object), parameterTypes, type);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!type.IsClass)
            {
                LocalBuilder local = iLGenerator.DeclareLocal(type);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Unbox_Any, type);
                iLGenerator.Emit(OpCodes.Stloc_0);
                iLGenerator.Emit(OpCodes.Ldloca_S, local);
                iLGenerator.Emit(OpCodes.Ldfld, fieldInfo);
                if (fieldInfo.FieldType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Box, fieldInfo.FieldType);
                }
            }
            else
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldfld, fieldInfo);
                if (fieldInfo.FieldType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Box, fieldInfo.FieldType);
                }
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (GenericGetter) method.CreateDelegate(typeof(GenericGetter));
        }

        internal static GenericGetter CreateGetMethod(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
            {
                return null;
            }
            Type[] parameterTypes = new Type[] { typeof(object) };
            DynamicMethod method = new DynamicMethod("_", typeof(object), parameterTypes, type);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!type.IsClass)
            {
                LocalBuilder local = iLGenerator.DeclareLocal(type);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Unbox_Any, type);
                iLGenerator.Emit(OpCodes.Stloc_0);
                iLGenerator.Emit(OpCodes.Ldloca_S, local);
                iLGenerator.EmitCall(OpCodes.Call, getMethod, null);
                if (propertyInfo.PropertyType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Box, propertyInfo.PropertyType);
                }
            }
            else
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
                iLGenerator.EmitCall(OpCodes.Callvirt, getMethod, null);
                if (propertyInfo.PropertyType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Box, propertyInfo.PropertyType);
                }
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (GenericGetter) method.CreateDelegate(typeof(GenericGetter));
        }

        internal static GenericSetter CreateSetField(Type type, FieldInfo fieldInfo)
        {
            Type[] parameterTypes = new Type[2];
            parameterTypes[0] = parameterTypes[1] = typeof(object);
            DynamicMethod method = new DynamicMethod("_", typeof(object), parameterTypes, type);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!type.IsClass)
            {
                LocalBuilder local = iLGenerator.DeclareLocal(type);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Unbox_Any, type);
                iLGenerator.Emit(OpCodes.Stloc_0);
                iLGenerator.Emit(OpCodes.Ldloca_S, local);
                iLGenerator.Emit(OpCodes.Ldarg_1);
                if (fieldInfo.FieldType.IsClass)
                {
                    iLGenerator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
                }
                iLGenerator.Emit(OpCodes.Stfld, fieldInfo);
                iLGenerator.Emit(OpCodes.Ldloc_0);
                iLGenerator.Emit(OpCodes.Box, type);
                iLGenerator.Emit(OpCodes.Ret);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ldarg_1);
                if (fieldInfo.FieldType.IsValueType)
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
                }
                iLGenerator.Emit(OpCodes.Stfld, fieldInfo);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Ret);
            }
            return (GenericSetter) method.CreateDelegate(typeof(GenericSetter));
        }

        internal static GenericSetter CreateSetMethod(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
            {
                return null;
            }
            Type[] parameterTypes = new Type[2];
            parameterTypes[0] = parameterTypes[1] = typeof(object);
            DynamicMethod method = new DynamicMethod("_", typeof(object), parameterTypes);
            ILGenerator iLGenerator = method.GetILGenerator();
            if (!type.IsClass)
            {
                LocalBuilder local = iLGenerator.DeclareLocal(type);
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Unbox_Any, type);
                iLGenerator.Emit(OpCodes.Stloc_0);
                iLGenerator.Emit(OpCodes.Ldloca_S, local);
                iLGenerator.Emit(OpCodes.Ldarg_1);
                if (propertyInfo.PropertyType.IsClass)
                {
                    iLGenerator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                }
                iLGenerator.EmitCall(OpCodes.Call, setMethod, null);
                iLGenerator.Emit(OpCodes.Ldloc_0);
                iLGenerator.Emit(OpCodes.Box, type);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
                iLGenerator.Emit(OpCodes.Ldarg_1);
                if (propertyInfo.PropertyType.IsClass)
                {
                    iLGenerator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                }
                else
                {
                    iLGenerator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                }
                iLGenerator.EmitCall(OpCodes.Callvirt, setMethod, null);
                iLGenerator.Emit(OpCodes.Ldarg_0);
            }
            iLGenerator.Emit(OpCodes.Ret);
            return (GenericSetter) method.CreateDelegate(typeof(GenericSetter));
        }

        internal object FastCreateInstance(Type objtype)
        {
            object obj3;
            try
            {
                CreateObject obj2 = null;
                if (this._constrcache.TryGetValue(objtype, out obj2))
                {
                    return obj2();
                }
                if (objtype.IsClass)
                {
                    DynamicMethod method = new DynamicMethod("_", objtype, null);
                    ILGenerator iLGenerator = method.GetILGenerator();
                    iLGenerator.Emit(OpCodes.Newobj, objtype.GetConstructor(Type.EmptyTypes));
                    iLGenerator.Emit(OpCodes.Ret);
                    obj2 = (CreateObject) method.CreateDelegate(typeof(CreateObject));
                    this._constrcache.Add(objtype, obj2);
                }
                else
                {
                    DynamicMethod method2 = new DynamicMethod("_", typeof(object), null);
                    ILGenerator generator2 = method2.GetILGenerator();
                    LocalBuilder local = generator2.DeclareLocal(objtype);
                    generator2.Emit(OpCodes.Ldloca_S, local);
                    generator2.Emit(OpCodes.Initobj, objtype);
                    generator2.Emit(OpCodes.Ldloc_0);
                    generator2.Emit(OpCodes.Box, objtype);
                    generator2.Emit(OpCodes.Ret);
                    obj2 = (CreateObject) method2.CreateDelegate(typeof(CreateObject));
                    this._constrcache.Add(objtype, obj2);
                }
                obj3 = obj2();
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Failed to fast create instance for type '{0}' from assemebly '{1}'", objtype.FullName, objtype.AssemblyQualifiedName), exception);
            }
            return obj3;
        }

        internal List<Getters> GetGetters(Type type)
        {
            List<Getters> list = null;
            if (this._getterscache.TryGetValue(type, out list))
            {
                return list;
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Getters> list2 = new List<Getters>();
            foreach (PropertyInfo info in properties)
            {
                if (info.CanWrite || this.ShowReadOnlyProperties)
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                    if ((customAttributes == null) || (customAttributes.Length <= 0))
                    {
                        GenericGetter getter = CreateGetMethod(type, info);
                        if (getter != null)
                        {
                            Getters item = new Getters {
                                Name = info.Name,
                                Getter = getter,
                                propertyType = info.PropertyType
                            };
                            list2.Add(item);
                        }
                    }
                }
            }
            foreach (FieldInfo info2 in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] objArray2 = info2.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                if ((objArray2 == null) || (objArray2.Length <= 0))
                {
                    GenericGetter getter2 = CreateGetField(type, info2);
                    if (getter2 != null)
                    {
                        Getters getters2 = new Getters {
                            Name = info2.Name,
                            Getter = getter2,
                            propertyType = info2.FieldType
                        };
                        list2.Add(getters2);
                    }
                }
            }
            this._getterscache.Add(type, list2);
            return list2;
        }

        internal string GetTypeAssemblyName(Type t)
        {
            string str = string.Empty;
            if (this._tyname.TryGetValue(t, out str))
            {
                return str;
            }
            string assemblyQualifiedName = t.AssemblyQualifiedName;
            this._tyname.Add(t, assemblyQualifiedName);
            return assemblyQualifiedName;
        }

        internal Type GetTypeFromCache(string typename)
        {
            Type type = null;
            if (this._typecache.TryGetValue(typename, out type))
            {
                return type;
            }
            Type type2 = Type.GetType(typename);
            this._typecache.Add(typename, type2);
            return type2;
        }

        private delegate object CreateObject();

        internal delegate object GenericGetter(object obj);

        internal delegate object GenericSetter(object target, object value);
    }
}

