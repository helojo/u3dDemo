namespace fastJSON
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public sealed class JSON
    {
        [ThreadStatic]
        private static JSON _instance;
        private JSONParameters _params;
        private SafeDictionary<string, SafeDictionary<string, myPropInfo>> _propertycache = new SafeDictionary<string, SafeDictionary<string, myPropInfo>>();
        private bool _usingglobals;
        public JSONParameters Parameters = new JSONParameters();

        private JSON()
        {
        }

        public string Beautify(string input)
        {
            return Formatter.PrettyPrint(input);
        }

        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType == typeof(int))
            {
                return (int) ((long) value);
            }
            if (conversionType == typeof(long))
            {
                return (long) value;
            }
            if (conversionType == typeof(string))
            {
                return (string) value;
            }
            if (conversionType == typeof(Guid))
            {
                return this.CreateGuid((string) value);
            }
            if (conversionType.IsEnum)
            {
                return this.CreateEnum(conversionType, (string) value);
            }
            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }

        private object CreateArray(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
        {
            Array array = Array.CreateInstance(bt, data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                object obj2 = data[i];
                if (obj2 is IDictionary)
                {
                    array.SetValue(this.ParseDictionary((Dictionary<string, object>) obj2, globalTypes, bt, null), i);
                }
                else
                {
                    array.SetValue(this.ChangeType(obj2, bt), i);
                }
            }
            return array;
        }

        private DateTime CreateDateTime(string value)
        {
            bool flag = false;
            int year = (int) this.CreateLong(value.Substring(0, 4));
            int month = (int) this.CreateLong(value.Substring(5, 2));
            int day = (int) this.CreateLong(value.Substring(8, 2));
            int hour = (int) this.CreateLong(value.Substring(11, 2));
            int minute = (int) this.CreateLong(value.Substring(14, 2));
            int second = (int) this.CreateLong(value.Substring(0x11, 2));
            if (value.EndsWith("Z"))
            {
                flag = true;
            }
            if (!this._params.UseUTCDateTime && !flag)
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            DateTime time = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            return time.ToLocalTime();
        }

        private object CreateDictionary(List<object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
        {
            IDictionary dictionary = (IDictionary) Reflection.Instance.FastCreateInstance(pt);
            Type type = null;
            Type type2 = null;
            if (types != null)
            {
                type = types[0];
                type2 = types[1];
            }
            foreach (Dictionary<string, object> dictionary2 in reader)
            {
                object obj2 = dictionary2["k"];
                object obj3 = dictionary2["v"];
                if (obj2 is Dictionary<string, object>)
                {
                    obj2 = this.ParseDictionary((Dictionary<string, object>) obj2, globalTypes, type, null);
                }
                else
                {
                    obj2 = this.ChangeType(obj2, type);
                }
                if (obj3 is Dictionary<string, object>)
                {
                    obj3 = this.ParseDictionary((Dictionary<string, object>) obj3, globalTypes, type2, null);
                }
                else
                {
                    obj3 = this.ChangeType(obj3, type2);
                }
                dictionary.Add(obj2, obj3);
            }
            return dictionary;
        }

        private object CreateEnum(Type pt, string v)
        {
            return Enum.Parse(pt, v);
        }

        private object CreateGenericList(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
        {
            IList list = (IList) Reflection.Instance.FastCreateInstance(pt);
            foreach (object obj2 in data)
            {
                if (obj2 is IDictionary)
                {
                    list.Add(this.ParseDictionary((Dictionary<string, object>) obj2, globalTypes, bt, null));
                }
                else if (obj2 is List<object>)
                {
                    list.Add(((List<object>) obj2).ToArray());
                }
                else
                {
                    list.Add(this.ChangeType(obj2, bt));
                }
            }
            return list;
        }

        private Guid CreateGuid(string s)
        {
            if (s.Length > 30)
            {
                return new Guid(s);
            }
            return new Guid(Convert.FromBase64String(s));
        }

        private long CreateLong(string s)
        {
            long num = 0L;
            bool flag = false;
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '-':
                        flag = true;
                        break;

                    case '+':
                        flag = false;
                        break;

                    default:
                        num *= 10L;
                        num += ch - '0';
                        break;
                }
            }
            return (!flag ? num : -num);
        }

        private myPropInfo CreateMyProp(Type t, string name)
        {
            myPropInfo info = new myPropInfo {
                filled = true,
                CanWrite = true,
                pt = t,
                Name = name,
                isDictionary = t.Name.Contains("Dictionary")
            };
            if (info.isDictionary)
            {
                info.GenericTypes = t.GetGenericArguments();
            }
            info.isValueType = t.IsValueType;
            info.isGenericType = t.IsGenericType;
            info.isArray = t.IsArray;
            if (info.isArray)
            {
                info.bt = t.GetElementType();
            }
            if (info.isGenericType)
            {
                info.bt = t.GetGenericArguments()[0];
            }
            info.isByteArray = t == typeof(byte[]);
            info.isGuid = (t == typeof(Guid)) || (t == typeof(Guid?));
            info.changeType = this.GetChangeType(t);
            info.isEnum = t.IsEnum;
            info.isDateTime = (t == typeof(DateTime)) || (t == typeof(DateTime?));
            info.isInt = (t == typeof(int)) || (t == typeof(int?));
            info.isLong = (t == typeof(long)) || (t == typeof(long?));
            info.isString = t == typeof(string);
            info.isBool = (t == typeof(bool)) || (t == typeof(bool?));
            info.isClass = t.IsClass;
            if ((info.isDictionary && (info.GenericTypes.Length > 0)) && (info.GenericTypes[0] == typeof(string)))
            {
                info.isStringDictionary = true;
            }
            return info;
        }

        private object CreateStringKeyDictionary(Dictionary<string, object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
        {
            IDictionary dictionary = (IDictionary) Reflection.Instance.FastCreateInstance(pt);
            Type type = null;
            if (types != null)
            {
                type = types[1];
            }
            foreach (KeyValuePair<string, object> pair in reader)
            {
                string key = pair.Key;
                object obj2 = null;
                if (pair.Value is Dictionary<string, object>)
                {
                    obj2 = this.ParseDictionary((Dictionary<string, object>) pair.Value, globalTypes, type, null);
                }
                else
                {
                    obj2 = this.ChangeType(pair.Value, type);
                }
                dictionary.Add(key, obj2);
            }
            return dictionary;
        }

        public object DeepCopy(object obj)
        {
            return this.ToObject(this.ToJSON(obj));
        }

        public T DeepCopy<T>(T obj)
        {
            return this.ToObject<T>(this.ToJSON(obj));
        }

        public object FillObject(object input, string json)
        {
            this._params = this.Parameters;
            this._params.FixValues();
            Reflection.Instance.ShowReadOnlyProperties = this._params.ShowReadOnlyProperties;
            Dictionary<string, object> d = new JsonParser(json, this.Parameters.IgnoreCaseOnDeserialize).Decode() as Dictionary<string, object>;
            if (d == null)
            {
                return null;
            }
            return this.ParseDictionary(d, null, input.GetType(), input);
        }

        private Type GetChangeType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                return conversionType.GetGenericArguments()[0];
            }
            return conversionType;
        }

        private SafeDictionary<string, myPropInfo> Getproperties(Type type, string typename)
        {
            SafeDictionary<string, myPropInfo> dictionary = null;
            if (!this._propertycache.TryGetValue(typename, out dictionary))
            {
                dictionary = new SafeDictionary<string, myPropInfo>();
                foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    myPropInfo info2 = this.CreateMyProp(info.PropertyType, info.Name);
                    info2.CanWrite = info.CanWrite;
                    info2.setter = Reflection.CreateSetMethod(type, info);
                    info2.getter = Reflection.CreateGetMethod(type, info);
                    dictionary.Add(info.Name, info2);
                }
                foreach (FieldInfo info3 in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    myPropInfo info4 = this.CreateMyProp(info3.FieldType, info3.Name);
                    info4.setter = Reflection.CreateSetField(type, info3);
                    info4.getter = Reflection.CreateGetField(type, info3);
                    dictionary.Add(info3.Name, info4);
                }
                this._propertycache.Add(typename, dictionary);
            }
            return dictionary;
        }

        public object Parse(string json)
        {
            this._params = this.Parameters;
            Reflection.Instance.ShowReadOnlyProperties = this._params.ShowReadOnlyProperties;
            return new JsonParser(json, this._params.IgnoreCaseOnDeserialize).Decode();
        }

        private object ParseDictionary(Dictionary<string, object> d, Dictionary<string, object> globaltypes, Type type, object input)
        {
            object obj2 = string.Empty;
            if (d.TryGetValue("$types", out obj2))
            {
                this._usingglobals = true;
                globaltypes = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> pair in (Dictionary<string, object>) obj2)
                {
                    globaltypes.Add((string) pair.Value, pair.Key);
                }
            }
            if (d.TryGetValue("$type", out obj2))
            {
                if (this._usingglobals)
                {
                    object obj3 = string.Empty;
                    if (globaltypes.TryGetValue((string) obj2, out obj3))
                    {
                        obj2 = obj3;
                    }
                }
                type = Reflection.Instance.GetTypeFromCache((string) obj2);
            }
            if (type == null)
            {
                throw new Exception("Cannot determine type");
            }
            string fullName = type.FullName;
            object obj4 = input;
            if (obj4 == null)
            {
                obj4 = Reflection.Instance.FastCreateInstance(type);
            }
            SafeDictionary<string, myPropInfo> properties = this.Getproperties(type, fullName);
            foreach (string str2 in d.Keys)
            {
                string key = str2;
                if (this._params.IgnoreCaseOnDeserialize)
                {
                    key = key.ToLower();
                }
                if (key == "$map")
                {
                    this.ProcessMap(obj4, properties, (Dictionary<string, object>) d[key]);
                }
                else
                {
                    myPropInfo info;
                    if (properties.TryGetValue(key, out info) && (info.filled && info.CanWrite))
                    {
                        object obj5 = d[key];
                        if (obj5 != null)
                        {
                            object obj6 = null;
                            if (info.isInt)
                            {
                                obj6 = (int) ((long) obj5);
                            }
                            else if (info.isLong)
                            {
                                obj6 = (long) obj5;
                            }
                            else if (info.isString)
                            {
                                obj6 = (string) obj5;
                            }
                            else if (info.isBool)
                            {
                                obj6 = (bool) obj5;
                            }
                            else if ((info.isGenericType && !info.isValueType) && (!info.isDictionary && (obj5 is List<object>)))
                            {
                                obj6 = this.CreateGenericList((List<object>) obj5, info.pt, info.bt, globaltypes);
                            }
                            else if (info.isByteArray)
                            {
                                obj6 = Convert.FromBase64String((string) obj5);
                            }
                            else if (info.isArray && !info.isValueType)
                            {
                                obj6 = this.CreateArray((List<object>) obj5, info.pt, info.bt, globaltypes);
                            }
                            else if (info.isGuid)
                            {
                                obj6 = this.CreateGuid((string) obj5);
                            }
                            else if (info.isStringDictionary)
                            {
                                obj6 = this.CreateStringKeyDictionary((Dictionary<string, object>) obj5, info.pt, info.GenericTypes, globaltypes);
                            }
                            else if (info.isDictionary)
                            {
                                obj6 = this.CreateDictionary((List<object>) obj5, info.pt, info.GenericTypes, globaltypes);
                            }
                            else if (info.isEnum)
                            {
                                obj6 = this.CreateEnum(info.pt, (string) obj5);
                            }
                            else if (info.isDateTime)
                            {
                                obj6 = this.CreateDateTime((string) obj5);
                            }
                            else if (info.isClass && (obj5 is Dictionary<string, object>))
                            {
                                obj6 = this.ParseDictionary((Dictionary<string, object>) obj5, globaltypes, info.pt, info.getter(obj4));
                            }
                            else if (info.isValueType)
                            {
                                obj6 = this.ChangeType(obj5, info.changeType);
                            }
                            else if (obj5 is List<object>)
                            {
                                obj6 = this.CreateArray((List<object>) obj5, info.pt, typeof(object), globaltypes);
                            }
                            else
                            {
                                obj6 = obj5;
                            }
                            obj4 = info.setter(obj4, obj6);
                        }
                    }
                }
            }
            return obj4;
        }

        private void ProcessMap(object obj, SafeDictionary<string, myPropInfo> props, Dictionary<string, object> dic)
        {
            foreach (KeyValuePair<string, object> pair in dic)
            {
                myPropInfo info = props[pair.Key];
                object obj2 = info.getter(obj);
                if (Type.GetType((string) pair.Value) == typeof(Guid))
                {
                    info.setter(obj, this.CreateGuid((string) obj2));
                }
            }
        }

        private object RootDictionary(object parse, Type type)
        {
            Type[] genericArguments = type.GetGenericArguments();
            if (parse is Dictionary<string, object>)
            {
                IDictionary dictionary = (IDictionary) Reflection.Instance.FastCreateInstance(type);
                foreach (KeyValuePair<string, object> pair in (Dictionary<string, object>) parse)
                {
                    object obj2;
                    object key = this.ChangeType(pair.Key, genericArguments[0]);
                    if (pair.Value is Dictionary<string, object>)
                    {
                        obj2 = this.ParseDictionary(pair.Value as Dictionary<string, object>, null, genericArguments[1], null);
                    }
                    else if (pair.Value is List<object>)
                    {
                        obj2 = this.CreateArray(pair.Value as List<object>, typeof(object), typeof(object), null);
                    }
                    else
                    {
                        obj2 = this.ChangeType(pair.Value, genericArguments[1]);
                    }
                    dictionary.Add(key, obj2);
                }
                return dictionary;
            }
            if (parse is List<object>)
            {
                return this.CreateDictionary(parse as List<object>, type, genericArguments, null);
            }
            return null;
        }

        private object RootList(object parse, Type type)
        {
            Type[] genericArguments = type.GetGenericArguments();
            IList list = (IList) Reflection.Instance.FastCreateInstance(type);
            IEnumerator enumerator = ((IList) parse).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this._usingglobals = false;
                    object obj3 = current;
                    if (current is Dictionary<string, object>)
                    {
                        obj3 = this.ParseDictionary(current as Dictionary<string, object>, null, genericArguments[0], null);
                    }
                    else
                    {
                        obj3 = this.ChangeType(current, genericArguments[0]);
                    }
                    list.Add(obj3);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return list;
        }

        public string ToJSON(object obj)
        {
            this._params = this.Parameters;
            this._params.FixValues();
            Reflection.Instance.ShowReadOnlyProperties = this._params.ShowReadOnlyProperties;
            return this.ToJSON(obj, this.Parameters);
        }

        public string ToJSON(object obj, JSONParameters param)
        {
            this._params = param;
            this._params.FixValues();
            Reflection.Instance.ShowReadOnlyProperties = this._params.ShowReadOnlyProperties;
            Type genericTypeDefinition = null;
            if (obj == null)
            {
                return "null";
            }
            if (obj.GetType().IsGenericType)
            {
                genericTypeDefinition = obj.GetType().GetGenericTypeDefinition();
            }
            if ((genericTypeDefinition == typeof(Dictionary<,>)) || (genericTypeDefinition == typeof(List<>)))
            {
                this._params.UsingGlobalTypes = false;
            }
            if (this._params.EnableAnonymousTypes)
            {
                this._params.UseExtensions = false;
                this._params.UsingGlobalTypes = false;
                Reflection.Instance.ShowReadOnlyProperties = true;
            }
            this._usingglobals = this._params.UsingGlobalTypes;
            return new JSONSerializer(this._params).ConvertToJSON(obj);
        }

        public object ToObject(string json)
        {
            return this.ToObject(json, null);
        }

        public T ToObject<T>(string json)
        {
            return (T) this.ToObject(json, typeof(T));
        }

        public object ToObject(string json, Type type)
        {
            this._params = this.Parameters;
            this._params.FixValues();
            Reflection.Instance.ShowReadOnlyProperties = this._params.ShowReadOnlyProperties;
            Type genericTypeDefinition = null;
            if ((type != null) && type.IsGenericType)
            {
                genericTypeDefinition = type.GetGenericTypeDefinition();
            }
            if ((genericTypeDefinition == typeof(Dictionary<,>)) || (genericTypeDefinition == typeof(List<>)))
            {
                this._params.UsingGlobalTypes = false;
            }
            this._usingglobals = this._params.UsingGlobalTypes;
            return new JsonParser(json, this.Parameters.IgnoreCaseOnDeserialize).Decode();
        }

        public static JSON Instance
        {
            get
            {
                if (_instance == null)
                {
                }
                return (_instance = new JSON());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct myPropInfo
        {
            public bool filled;
            public Type pt;
            public Type bt;
            public Type changeType;
            public bool isDictionary;
            public bool isValueType;
            public bool isGenericType;
            public bool isArray;
            public bool isByteArray;
            public bool isGuid;
            public Reflection.GenericSetter setter;
            public bool isEnum;
            public bool isDateTime;
            public Type[] GenericTypes;
            public bool isInt;
            public bool isLong;
            public bool isString;
            public bool isBool;
            public bool isClass;
            public Reflection.GenericGetter getter;
            public bool isStringDictionary;
            public string Name;
            public bool CanWrite;
        }
    }
}

