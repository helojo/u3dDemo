using HutongGames.PlayMaker;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayMakerUtils
{
    public static bool ApplyValueToFsmVar(Fsm fromFsm, FsmVar fsmVar, object value)
    {
        if (fromFsm == null)
        {
            return false;
        }
        if (fsmVar == null)
        {
            return false;
        }
        if (value == null)
        {
            if (fsmVar.Type == VariableType.Bool)
            {
                fromFsm.Variables.GetFsmBool(fsmVar.variableName).Value = false;
            }
            else if (fsmVar.Type == VariableType.Color)
            {
                fromFsm.Variables.GetFsmColor(fsmVar.variableName).Value = Color.black;
            }
            else if (fsmVar.Type == VariableType.Int)
            {
                fromFsm.Variables.GetFsmInt(fsmVar.variableName).Value = 0;
            }
            else if (fsmVar.Type == VariableType.Float)
            {
                fromFsm.Variables.GetFsmFloat(fsmVar.variableName).Value = 0f;
            }
            else if (fsmVar.Type == VariableType.GameObject)
            {
                fromFsm.Variables.GetFsmGameObject(fsmVar.variableName).Value = null;
            }
            else if (fsmVar.Type == VariableType.Material)
            {
                fromFsm.Variables.GetFsmMaterial(fsmVar.variableName).Value = null;
            }
            else if (fsmVar.Type == VariableType.Object)
            {
                fromFsm.Variables.GetFsmObject(fsmVar.variableName).Value = null;
            }
            else if (fsmVar.Type == VariableType.Quaternion)
            {
                fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName).Value = Quaternion.identity;
            }
            else if (fsmVar.Type == VariableType.Rect)
            {
                fromFsm.Variables.GetFsmRect(fsmVar.variableName).Value = new Rect(0f, 0f, 0f, 0f);
            }
            else if (fsmVar.Type == VariableType.String)
            {
                fromFsm.Variables.GetFsmString(fsmVar.variableName).Value = string.Empty;
            }
            else if (fsmVar.Type == VariableType.String)
            {
                fromFsm.Variables.GetFsmTexture(fsmVar.variableName).Value = null;
            }
            else if (fsmVar.Type == VariableType.Vector2)
            {
                fromFsm.Variables.GetFsmVector2(fsmVar.variableName).Value = Vector2.zero;
            }
            else if (fsmVar.Type == VariableType.Vector3)
            {
                fromFsm.Variables.GetFsmVector3(fsmVar.variableName).Value = Vector3.zero;
            }
            return true;
        }
        System.Type o = value.GetType();
        System.Type type2 = null;
        switch (fsmVar.Type)
        {
            case VariableType.Float:
                type2 = typeof(float);
                break;

            case VariableType.Int:
                type2 = typeof(int);
                break;

            case VariableType.Bool:
                type2 = typeof(bool);
                break;

            case VariableType.GameObject:
                type2 = typeof(GameObject);
                break;

            case VariableType.String:
                type2 = typeof(string);
                break;

            case VariableType.Vector2:
                type2 = typeof(Vector2);
                break;

            case VariableType.Vector3:
                type2 = typeof(Vector3);
                break;

            case VariableType.Color:
                type2 = typeof(Color);
                break;

            case VariableType.Rect:
                type2 = typeof(Rect);
                break;

            case VariableType.Material:
                type2 = typeof(Material);
                break;

            case VariableType.Texture:
                type2 = typeof(Texture2D);
                break;

            case VariableType.Quaternion:
                type2 = typeof(Quaternion);
                break;

            case VariableType.Object:
                type2 = typeof(UnityEngine.Object);
                break;
        }
        bool flag = true;
        if (!type2.Equals(o))
        {
            flag = false;
            if (type2.Equals(typeof(UnityEngine.Object)))
            {
                flag = true;
            }
            if (!flag)
            {
                if (o.Equals(typeof(ProceduralMaterial)))
                {
                    flag = true;
                }
                if (o.Equals(typeof(double)))
                {
                    flag = true;
                }
                if (o.Equals(typeof(long)))
                {
                    flag = true;
                }
            }
        }
        if (!flag)
        {
            Debug.LogError(string.Concat(new object[] { "The fsmVar value <", type2, "> doesn't match the value <", o, ">" }));
            return false;
        }
        if (o == typeof(bool))
        {
            fromFsm.Variables.GetFsmBool(fsmVar.variableName).Value = (bool) value;
        }
        else if (o == typeof(Color))
        {
            fromFsm.Variables.GetFsmColor(fsmVar.variableName).Value = (Color) value;
        }
        else if (o == typeof(int))
        {
            fromFsm.Variables.GetFsmInt(fsmVar.variableName).Value = Convert.ToInt32(value);
        }
        else if (o == typeof(long))
        {
            if (fsmVar.Type == VariableType.Int)
            {
                fromFsm.Variables.GetFsmInt(fsmVar.variableName).Value = Convert.ToInt32(value);
            }
            else if (fsmVar.Type == VariableType.Float)
            {
                fromFsm.Variables.GetFsmFloat(fsmVar.variableName).Value = Convert.ToSingle(value);
            }
        }
        else if (o == typeof(float))
        {
            fromFsm.Variables.GetFsmFloat(fsmVar.variableName).Value = (float) value;
        }
        else if (o == typeof(double))
        {
            fromFsm.Variables.GetFsmFloat(fsmVar.variableName).Value = Convert.ToSingle(value);
        }
        else if (o == typeof(GameObject))
        {
            fromFsm.Variables.GetFsmGameObject(fsmVar.variableName).Value = (GameObject) value;
        }
        else if (o == typeof(Material))
        {
            fromFsm.Variables.GetFsmMaterial(fsmVar.variableName).Value = (Material) value;
        }
        else if (o == typeof(ProceduralMaterial))
        {
            fromFsm.Variables.GetFsmMaterial(fsmVar.variableName).Value = (ProceduralMaterial) value;
        }
        else if ((o == typeof(UnityEngine.Object)) || (type2 == typeof(UnityEngine.Object)))
        {
            fromFsm.Variables.GetFsmObject(fsmVar.variableName).Value = (UnityEngine.Object) value;
        }
        else if (o == typeof(Quaternion))
        {
            fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName).Value = (Quaternion) value;
        }
        else if (o == typeof(Rect))
        {
            fromFsm.Variables.GetFsmRect(fsmVar.variableName).Value = (Rect) value;
        }
        else if (o == typeof(string))
        {
            fromFsm.Variables.GetFsmString(fsmVar.variableName).Value = (string) value;
        }
        else if (o == typeof(Texture2D))
        {
            fromFsm.Variables.GetFsmTexture(fsmVar.variableName).Value = (Texture2D) value;
        }
        else if (o == typeof(Vector2))
        {
            fromFsm.Variables.GetFsmVector2(fsmVar.variableName).Value = (Vector2) value;
        }
        else if (o == typeof(Vector3))
        {
            fromFsm.Variables.GetFsmVector3(fsmVar.variableName).Value = (Vector3) value;
        }
        else
        {
            Debug.LogWarning("?!?!" + o);
        }
        return true;
    }

    public static float GetFloatFromObject(object _obj, VariableType targetType, bool fastProcessingIfPossible)
    {
        if ((targetType == VariableType.Int) || (targetType == VariableType.Float))
        {
            return Convert.ToSingle(_obj);
        }
        if (targetType == VariableType.Vector2)
        {
            Vector2 vector = (Vector2) _obj;
            if (vector != Vector2.zero)
            {
                return (!fastProcessingIfPossible ? vector.magnitude : vector.sqrMagnitude);
            }
        }
        if (targetType == VariableType.Vector3)
        {
            Vector3 vector2 = (Vector3) _obj;
            if (vector2 != Vector3.zero)
            {
                return (!fastProcessingIfPossible ? vector2.magnitude : vector2.sqrMagnitude);
            }
        }
        if (targetType == VariableType.GameObject)
        {
            GameObject obj2 = (GameObject) _obj;
            if (obj2 != null)
            {
                MeshRenderer component = obj2.GetComponent<MeshRenderer>();
                if (component != null)
                {
                    return ((component.bounds.size.x * component.bounds.size.y) * component.bounds.size.z);
                }
            }
        }
        if (targetType == VariableType.Rect)
        {
            Rect rect = (Rect) _obj;
            return (rect.width * rect.height);
        }
        if (targetType == VariableType.String)
        {
            string s = (string) _obj;
            if (s != null)
            {
                return float.Parse(s);
            }
        }
        return 0f;
    }

    public static object GetValueFromFsmVar(Fsm fromFsm, FsmVar fsmVar)
    {
        if (fromFsm != null)
        {
            if (fsmVar == null)
            {
                return null;
            }
            if (fsmVar.useVariable)
            {
                string variableName = fsmVar.variableName;
                switch (fsmVar.Type)
                {
                    case VariableType.Float:
                        return fromFsm.Variables.GetFsmFloat(variableName).Value;

                    case VariableType.Int:
                        return fromFsm.Variables.GetFsmInt(variableName).Value;

                    case VariableType.Bool:
                        return fromFsm.Variables.GetFsmBool(variableName).Value;

                    case VariableType.GameObject:
                        return fromFsm.Variables.GetFsmGameObject(variableName).Value;

                    case VariableType.String:
                        return fromFsm.Variables.GetFsmString(variableName).Value;

                    case VariableType.Vector2:
                        return fromFsm.Variables.GetFsmVector2(variableName).Value;

                    case VariableType.Vector3:
                        return fromFsm.Variables.GetFsmVector3(variableName).Value;

                    case VariableType.Color:
                        return fromFsm.Variables.GetFsmColor(variableName).Value;

                    case VariableType.Rect:
                        return fromFsm.Variables.GetFsmRect(variableName).Value;

                    case VariableType.Material:
                        return fromFsm.Variables.GetFsmMaterial(variableName).Value;

                    case VariableType.Texture:
                        return fromFsm.Variables.GetFsmTexture(variableName).Value;

                    case VariableType.Quaternion:
                        return fromFsm.Variables.GetFsmQuaternion(variableName).Value;

                    case VariableType.Object:
                        return fromFsm.Variables.GetFsmObject(variableName).Value;
                }
            }
            else
            {
                switch (fsmVar.Type)
                {
                    case VariableType.Float:
                        return fsmVar.floatValue;

                    case VariableType.Int:
                        return fsmVar.intValue;

                    case VariableType.Bool:
                        return fsmVar.boolValue;

                    case VariableType.GameObject:
                        return fsmVar.gameObjectValue;

                    case VariableType.String:
                        return fsmVar.stringValue;

                    case VariableType.Vector2:
                        return fsmVar.vector2Value;

                    case VariableType.Vector3:
                        return fsmVar.vector3Value;

                    case VariableType.Color:
                        return fsmVar.colorValue;

                    case VariableType.Rect:
                        return fsmVar.rectValue;

                    case VariableType.Material:
                        return fsmVar.materialValue;

                    case VariableType.Texture:
                        return fsmVar.textureValue;

                    case VariableType.Quaternion:
                        return fsmVar.quaternionValue;

                    case VariableType.Object:
                        return fsmVar.objectReference;
                }
            }
        }
        return null;
    }

    public static string ParseFsmVarToString(Fsm fsm, FsmVar fsmVar)
    {
        if (fsmVar == null)
        {
            return string.Empty;
        }
        object valueFromFsmVar = GetValueFromFsmVar(fsm, fsmVar);
        if (valueFromFsmVar == null)
        {
            return string.Empty;
        }
        if (fsmVar.Type == VariableType.String)
        {
            return (string) valueFromFsmVar;
        }
        if (fsmVar.Type == VariableType.Bool)
        {
            return (!((bool) valueFromFsmVar) ? "0" : "1");
        }
        if (fsmVar.Type == VariableType.Float)
        {
            return float.Parse(valueFromFsmVar.ToString()).ToString();
        }
        if (fsmVar.Type == VariableType.Int)
        {
            return int.Parse(valueFromFsmVar.ToString()).ToString();
        }
        if (fsmVar.Type == VariableType.Vector2)
        {
            Vector2 vector = (Vector2) valueFromFsmVar;
            return (vector.x + "," + vector.y);
        }
        if (fsmVar.Type == VariableType.Vector3)
        {
            Vector3 vector2 = (Vector3) valueFromFsmVar;
            object[] objArray1 = new object[] { vector2.x, ",", vector2.y, ",", vector2.z };
            return string.Concat(objArray1);
        }
        if (fsmVar.Type == VariableType.Quaternion)
        {
            Quaternion quaternion = (Quaternion) valueFromFsmVar;
            object[] objArray2 = new object[] { quaternion.x, ",", quaternion.y, ",", quaternion.z, ",", quaternion.w };
            return string.Concat(objArray2);
        }
        if (fsmVar.Type == VariableType.Rect)
        {
            Rect rect = (Rect) valueFromFsmVar;
            object[] objArray3 = new object[] { rect.x, ",", rect.y, ",", rect.width, ",", rect.height };
            return string.Concat(objArray3);
        }
        if (fsmVar.Type == VariableType.Color)
        {
            Color color = (Color) valueFromFsmVar;
            object[] objArray4 = new object[] { color.r, ",", color.g, ",", color.b, ",", color.a };
            return string.Concat(objArray4);
        }
        if (fsmVar.Type == VariableType.GameObject)
        {
            GameObject obj3 = (GameObject) valueFromFsmVar;
            return obj3.name;
        }
        if (fsmVar.Type == VariableType.Material)
        {
            Material material = (Material) valueFromFsmVar;
            return material.name;
        }
        if (fsmVar.Type == VariableType.Texture)
        {
            Texture2D textured = (Texture2D) valueFromFsmVar;
            return textured.name;
        }
        Debug.LogWarning("ParseValueToString type not supported " + valueFromFsmVar.GetType());
        return ("<" + fsmVar.Type + "> not supported");
    }

    public static object ParseValueFromString(string source)
    {
        if (source != null)
        {
            if (source.StartsWith("string("))
            {
                source = source.Substring(7, source.Length - 8);
                return source;
            }
            if (source.StartsWith("bool("))
            {
                source = source.Substring(5, source.Length - 6);
                return (int.Parse(source) == 1);
            }
            if (source.StartsWith("int("))
            {
                source = source.Substring(4, source.Length - 5);
                return int.Parse(source);
            }
            if (source.StartsWith("float("))
            {
                source = source.Substring(6, source.Length - 7);
                return float.Parse(source);
            }
            if (source.StartsWith("vector2("))
            {
                string str = @"vector2\([x],[y]\)";
                string str2 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str = str.Replace("[x]", "(?<x>" + str2 + ")").Replace("[y]", "(?<y>" + str2 + ")");
                Match match = new Regex(@"^\s*" + str).Match(source);
                if ((match.Groups["x"].Value != string.Empty) && (match.Groups["y"].Value != string.Empty))
                {
                    return new Vector2(float.Parse(match.Groups["x"].Value), float.Parse(match.Groups["y"].Value));
                }
                return Vector2.zero;
            }
            if (source.StartsWith("vector3("))
            {
                string str3 = @"vector3\([x],[y],[z]\)";
                string str4 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str3 = str3.Replace("[x]", "(?<x>" + str4 + ")").Replace("[y]", "(?<y>" + str4 + ")").Replace("[z]", "(?<z>" + str4 + ")");
                Match match2 = new Regex(@"^\s*" + str3).Match(source);
                if (((match2.Groups["x"].Value != string.Empty) && (match2.Groups["y"].Value != string.Empty)) && (match2.Groups["z"].Value != string.Empty))
                {
                    return new Vector3(float.Parse(match2.Groups["x"].Value), float.Parse(match2.Groups["y"].Value), float.Parse(match2.Groups["z"].Value));
                }
                return Vector3.zero;
            }
            if (source.StartsWith("vector4("))
            {
                string str5 = @"vector4\([x],[y],[z],[w]\)";
                string str6 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str5 = str5.Replace("[x]", "(?<x>" + str6 + ")").Replace("[y]", "(?<y>" + str6 + ")").Replace("[z]", "(?<z>" + str6 + ")").Replace("[w]", "(?<w>" + str6 + ")");
                Match match3 = new Regex(@"^\s*" + str5).Match(source);
                if (((match3.Groups["x"].Value != string.Empty) && (match3.Groups["y"].Value != string.Empty)) && ((match3.Groups["z"].Value != string.Empty) && (match3.Groups["z"].Value != string.Empty)))
                {
                    return new Vector4(float.Parse(match3.Groups["x"].Value), float.Parse(match3.Groups["y"].Value), float.Parse(match3.Groups["z"].Value), float.Parse(match3.Groups["w"].Value));
                }
                return Vector4.zero;
            }
            if (source.StartsWith("rect("))
            {
                string str7 = @"rect\([x],[y],[w],[h]\)";
                string str8 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str7 = str7.Replace("[x]", "(?<x>" + str8 + ")").Replace("[y]", "(?<y>" + str8 + ")").Replace("[w]", "(?<w>" + str8 + ")").Replace("[h]", "(?<h>" + str8 + ")");
                Match match4 = new Regex(@"^\s*" + str7).Match(source);
                if (((match4.Groups["x"].Value != string.Empty) && (match4.Groups["y"].Value != string.Empty)) && ((match4.Groups["w"].Value != string.Empty) && (match4.Groups["h"].Value != string.Empty)))
                {
                    return new Rect(float.Parse(match4.Groups["x"].Value), float.Parse(match4.Groups["y"].Value), float.Parse(match4.Groups["w"].Value), float.Parse(match4.Groups["h"].Value));
                }
                return new Rect(0f, 0f, 0f, 0f);
            }
            if (source.StartsWith("quaternion("))
            {
                string str9 = @"quaternion\([x],[y],[z],[w]\)";
                string str10 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str9 = str9.Replace("[x]", "(?<x>" + str10 + ")").Replace("[y]", "(?<y>" + str10 + ")").Replace("[z]", "(?<z>" + str10 + ")").Replace("[w]", "(?<w>" + str10 + ")");
                Match match5 = new Regex(@"^\s*" + str9).Match(source);
                if (((match5.Groups["x"].Value != string.Empty) && (match5.Groups["y"].Value != string.Empty)) && ((match5.Groups["z"].Value != string.Empty) && (match5.Groups["z"].Value != string.Empty)))
                {
                    return new Quaternion(float.Parse(match5.Groups["x"].Value), float.Parse(match5.Groups["y"].Value), float.Parse(match5.Groups["z"].Value), float.Parse(match5.Groups["w"].Value));
                }
                return Quaternion.identity;
            }
            if (source.StartsWith("color("))
            {
                string str11 = @"color\([r],[g],[b],[a]\)";
                string str12 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str11 = str11.Replace("[r]", "(?<r>" + str12 + ")").Replace("[g]", "(?<g>" + str12 + ")").Replace("[b]", "(?<b>" + str12 + ")").Replace("[a]", "(?<a>" + str12 + ")");
                Match match6 = new Regex(@"^\s*" + str11).Match(source);
                if (((match6.Groups["r"].Value != string.Empty) && (match6.Groups["g"].Value != string.Empty)) && ((match6.Groups["b"].Value != string.Empty) && (match6.Groups["a"].Value != string.Empty)))
                {
                    return new Color(float.Parse(match6.Groups["r"].Value), float.Parse(match6.Groups["g"].Value), float.Parse(match6.Groups["b"].Value), float.Parse(match6.Groups["a"].Value));
                }
                return Color.black;
            }
            if (source.StartsWith("texture("))
            {
                source = source.Substring(8, source.Length - 9);
                byte[] data = Convert.FromBase64String(source);
                Texture2D textured = new Texture2D(0x10, 0x10);
                textured.LoadImage(data);
                return textured;
            }
            if (source.StartsWith("gameObject("))
            {
                source = source.Substring(11, source.Length - 12);
                return GameObject.Find(source);
            }
            Debug.LogWarning("ParseValueFromString failed for " + source);
        }
        return null;
    }

    public static object ParseValueFromString(string source, VariableType type)
    {
        System.Type type2 = typeof(string);
        VariableType type3 = type;
        switch ((type3 + 1))
        {
            case VariableType.Float:
                return ParseValueFromString(source);

            case VariableType.Int:
                type2 = typeof(float);
                break;

            case VariableType.Bool:
                type2 = typeof(int);
                break;

            case VariableType.GameObject:
                type2 = typeof(bool);
                break;

            case VariableType.String:
                type2 = typeof(GameObject);
                break;

            case VariableType.Vector3:
                type2 = typeof(Vector2);
                break;

            case VariableType.Color:
                type2 = typeof(Vector3);
                break;

            case VariableType.Rect:
                type2 = typeof(Color);
                break;

            case VariableType.Material:
                type2 = typeof(Rect);
                break;

            case VariableType.Object:
                type2 = typeof(Quaternion);
                break;
        }
        return ParseValueFromString(source, type2);
    }

    public static object ParseValueFromString(string source, bool useBytes)
    {
        return null;
    }

    public static object ParseValueFromString(string source, System.Type type)
    {
        if (source != null)
        {
            if (type == typeof(string))
            {
                return source;
            }
            if (type == typeof(bool))
            {
                if (string.Equals(source, "true", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                if (string.Equals(source, "false", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
                return (int.Parse(source) != 0);
            }
            if (type == typeof(int))
            {
                return int.Parse(source);
            }
            if (type == typeof(float))
            {
                return float.Parse(source);
            }
            if (type == typeof(Vector2))
            {
                string str = @"vector2\([x],[y]\)";
                string str2 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str = str.Replace("[x]", "(?<x>" + str2 + ")").Replace("[y]", "(?<y>" + str2 + ")");
                Match match = new Regex(@"^\s*" + str).Match(source);
                if ((match.Groups["x"].Value != string.Empty) && (match.Groups["y"].Value != string.Empty))
                {
                    return new Vector2(float.Parse(match.Groups["x"].Value), float.Parse(match.Groups["y"].Value));
                }
                return Vector2.zero;
            }
            if (type == typeof(Vector3))
            {
                string str3 = @"vector3\([x],[y],[z]\)";
                string str4 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str3 = str3.Replace("[x]", "(?<x>" + str4 + ")").Replace("[y]", "(?<y>" + str4 + ")").Replace("[z]", "(?<z>" + str4 + ")");
                Match match2 = new Regex(@"^\s*" + str3).Match(source);
                if (((match2.Groups["x"].Value != string.Empty) && (match2.Groups["y"].Value != string.Empty)) && (match2.Groups["z"].Value != string.Empty))
                {
                    return new Vector3(float.Parse(match2.Groups["x"].Value), float.Parse(match2.Groups["y"].Value), float.Parse(match2.Groups["z"].Value));
                }
                return Vector3.zero;
            }
            if (type == typeof(Vector4))
            {
                string str5 = @"vector4\([x],[y],[z],[w]\)";
                string str6 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str5 = str5.Replace("[x]", "(?<x>" + str6 + ")").Replace("[y]", "(?<y>" + str6 + ")").Replace("[z]", "(?<z>" + str6 + ")").Replace("[w]", "(?<w>" + str6 + ")");
                Match match3 = new Regex(@"^\s*" + str5).Match(source);
                if (((match3.Groups["x"].Value != string.Empty) && (match3.Groups["y"].Value != string.Empty)) && ((match3.Groups["z"].Value != string.Empty) && (match3.Groups["z"].Value != string.Empty)))
                {
                    return new Vector4(float.Parse(match3.Groups["x"].Value), float.Parse(match3.Groups["y"].Value), float.Parse(match3.Groups["z"].Value), float.Parse(match3.Groups["w"].Value));
                }
                return Vector4.zero;
            }
            if (type == typeof(Rect))
            {
                string str7 = @"rect\([x],[y],[w],[h]\)";
                string str8 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str7 = str7.Replace("[x]", "(?<x>" + str8 + ")").Replace("[y]", "(?<y>" + str8 + ")").Replace("[w]", "(?<w>" + str8 + ")").Replace("[h]", "(?<h>" + str8 + ")");
                Match match4 = new Regex(@"^\s*" + str7).Match(source);
                if (((match4.Groups["x"].Value != string.Empty) && (match4.Groups["y"].Value != string.Empty)) && ((match4.Groups["w"].Value != string.Empty) && (match4.Groups["h"].Value != string.Empty)))
                {
                    return new Rect(float.Parse(match4.Groups["x"].Value), float.Parse(match4.Groups["y"].Value), float.Parse(match4.Groups["w"].Value), float.Parse(match4.Groups["h"].Value));
                }
                return new Rect(0f, 0f, 0f, 0f);
            }
            if (type == typeof(Quaternion))
            {
                string str9 = @"quaternion\([x],[y],[z],[w]\)";
                string str10 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str9 = str9.Replace("[x]", "(?<x>" + str10 + ")").Replace("[y]", "(?<y>" + str10 + ")").Replace("[z]", "(?<z>" + str10 + ")").Replace("[w]", "(?<w>" + str10 + ")");
                Match match5 = new Regex(@"^\s*" + str9).Match(source);
                if (((match5.Groups["x"].Value != string.Empty) && (match5.Groups["y"].Value != string.Empty)) && ((match5.Groups["z"].Value != string.Empty) && (match5.Groups["z"].Value != string.Empty)))
                {
                    return new Quaternion(float.Parse(match5.Groups["x"].Value), float.Parse(match5.Groups["y"].Value), float.Parse(match5.Groups["z"].Value), float.Parse(match5.Groups["w"].Value));
                }
                return Quaternion.identity;
            }
            if (type == typeof(Color))
            {
                string str11 = @"color\([r],[g],[b],[a]\)";
                string str12 = @"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?";
                str11 = str11.Replace("[r]", "(?<r>" + str12 + ")").Replace("[g]", "(?<g>" + str12 + ")").Replace("[b]", "(?<b>" + str12 + ")").Replace("[a]", "(?<a>" + str12 + ")");
                Match match6 = new Regex(@"^\s*" + str11).Match(source);
                if (((match6.Groups["r"].Value != string.Empty) && (match6.Groups["g"].Value != string.Empty)) && ((match6.Groups["b"].Value != string.Empty) && (match6.Groups["a"].Value != string.Empty)))
                {
                    return new Color(float.Parse(match6.Groups["r"].Value), float.Parse(match6.Groups["g"].Value), float.Parse(match6.Groups["b"].Value), float.Parse(match6.Groups["a"].Value));
                }
                return Color.black;
            }
            if (type == typeof(GameObject))
            {
                source = source.Substring(11, source.Length - 12);
                return GameObject.Find(source);
            }
            Debug.LogWarning("ParseValueFromString failed for " + source);
        }
        return null;
    }

    public static string ParseValueToString(object item)
    {
        if (item.GetType() == typeof(string))
        {
            return ("string(" + item.ToString() + ")");
        }
        if (item.GetType() == typeof(bool))
        {
            int num = !((bool) item) ? 0 : 1;
            return ("bool(" + num + ")");
        }
        if (item.GetType() == typeof(float))
        {
            float num2 = float.Parse(item.ToString());
            return ("float(" + num2 + ")");
        }
        if (item.GetType() == typeof(int))
        {
            int num3 = int.Parse(item.ToString());
            return ("int(" + num3 + ")");
        }
        if (item.GetType() == typeof(Vector2))
        {
            Vector2 vector = (Vector2) item;
            object[] objArray1 = new object[] { "vector2(", vector.x, ",", vector.y, ")" };
            return string.Concat(objArray1);
        }
        if (item.GetType() == typeof(Vector3))
        {
            Vector3 vector2 = (Vector3) item;
            object[] objArray2 = new object[] { "vector3(", vector2.x, ",", vector2.y, ",", vector2.z, ")" };
            return string.Concat(objArray2);
        }
        if (item.GetType() == typeof(Vector4))
        {
            Vector4 vector3 = (Vector4) item;
            object[] objArray3 = new object[] { "vector4(", vector3.x, ",", vector3.y, ",", vector3.z, ",", vector3.w, ")" };
            return string.Concat(objArray3);
        }
        if (item.GetType() == typeof(Quaternion))
        {
            Quaternion quaternion = (Quaternion) item;
            object[] objArray4 = new object[] { "quaternion(", quaternion.x, ",", quaternion.y, ",", quaternion.z, ",", quaternion.w, ")" };
            return string.Concat(objArray4);
        }
        if (item.GetType() == typeof(Rect))
        {
            Rect rect = (Rect) item;
            object[] objArray5 = new object[] { "rect(", rect.x, ",", rect.y, ",", rect.width, ",", rect.height, ")" };
            return string.Concat(objArray5);
        }
        if (item.GetType() == typeof(Color))
        {
            Color color = (Color) item;
            object[] objArray6 = new object[] { "color(", color.r, ",", color.g, ",", color.b, ",", color.a, ")" };
            return string.Concat(objArray6);
        }
        if (item.GetType() == typeof(Texture2D))
        {
            byte[] inArray = ((Texture2D) item).EncodeToPNG();
            return ("texture(" + Convert.ToBase64String(inArray) + ")");
        }
        if (item.GetType() == typeof(GameObject))
        {
            GameObject obj2 = (GameObject) item;
            return ("gameObject(" + obj2.name + ")");
        }
        Debug.LogWarning("ParseValueToString type not supported " + item.GetType());
        return ("<" + item.GetType() + "> not supported");
    }

    public static string ParseValueToString(object item, bool useBytes)
    {
        return string.Empty;
    }

    public static void RefreshValueFromFsmVar(Fsm fromFsm, FsmVar fsmVar)
    {
        if (((fromFsm != null) && (fsmVar != null)) && fsmVar.useVariable)
        {
            switch (fsmVar.Type)
            {
                case VariableType.Float:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmFloat(fsmVar.variableName));
                    break;

                case VariableType.Int:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmInt(fsmVar.variableName));
                    break;

                case VariableType.Bool:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmBool(fsmVar.variableName));
                    break;

                case VariableType.GameObject:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmGameObject(fsmVar.variableName));
                    break;

                case VariableType.String:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmString(fsmVar.variableName));
                    break;

                case VariableType.Vector2:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmVector2(fsmVar.variableName));
                    break;

                case VariableType.Vector3:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmVector3(fsmVar.variableName));
                    break;

                case VariableType.Color:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmColor(fsmVar.variableName));
                    break;

                case VariableType.Rect:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmRect(fsmVar.variableName));
                    break;

                case VariableType.Material:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmMaterial(fsmVar.variableName));
                    break;

                case VariableType.Texture:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmVector3(fsmVar.variableName));
                    break;

                case VariableType.Quaternion:
                    fsmVar.GetValueFrom(fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName));
                    break;
            }
        }
    }

    public static void SendEventToGameObject(PlayMakerFSM fromFsm, GameObject target, string fsmEvent)
    {
        SendEventToGameObject(fromFsm, target, fsmEvent, null);
    }

    public static void SendEventToGameObject(PlayMakerFSM fromFsm, GameObject target, string fsmEvent, FsmEventData eventData)
    {
        if (eventData != null)
        {
            Fsm.EventData = eventData;
        }
        FsmEventTarget eventTarget = new FsmEventTarget {
            excludeSelf = 0
        };
        FsmOwnerDefault default2 = new FsmOwnerDefault {
            OwnerOption = OwnerDefaultOption.SpecifyGameObject,
            GameObject = new FsmGameObject()
        };
        default2.GameObject.Value = target;
        eventTarget.gameObject = default2;
        eventTarget.target = FsmEventTarget.EventTarget.GameObject;
        eventTarget.sendToChildren = 0;
        fromFsm.Fsm.Event(eventTarget, fsmEvent);
    }
}

