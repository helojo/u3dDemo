namespace fastJSON
{
    using System;
    using System.Text;

    internal static class Formatter
    {
        public static string Indent = "    ";

        public static void AppendIndent(StringBuilder sb, int count)
        {
            while (count > 0)
            {
                sb.Append(Indent);
                count--;
            }
        }

        public static bool IsEscaped(StringBuilder sb, int index)
        {
            bool flag = false;
            while ((index > 0) && (sb[--index] == '\\'))
            {
                flag = !flag;
            }
            return flag;
        }

        public static string PrettyPrint(string input)
        {
            StringBuilder sb = new StringBuilder(input.Length * 2);
            char? nullable = null;
            int count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                char ch2 = ch;
                switch (ch2)
                {
                    case '[':
                    case '{':
                        break;

                    case ']':
                    case '}':
                        goto Label_00B0;

                    default:
                        switch (ch2)
                        {
                            case '"':
                            case '\'':
                            {
                                sb.Append(ch);
                                if (nullable.HasValue)
                                {
                                    if (!IsEscaped(sb, i))
                                    {
                                        nullable = null;
                                    }
                                }
                                else
                                {
                                    nullable = new char?(ch);
                                }
                                continue;
                            }
                            default:
                                switch (ch2)
                                {
                                    case ',':
                                    {
                                        sb.Append(ch);
                                        if (!nullable.HasValue)
                                        {
                                            sb.AppendLine();
                                            AppendIndent(sb, count);
                                        }
                                        continue;
                                    }
                                    case ':':
                                    {
                                        if (nullable.HasValue)
                                        {
                                            sb.Append(ch);
                                        }
                                        else
                                        {
                                            sb.Append(" : ");
                                        }
                                        continue;
                                    }
                                }
                                goto Label_017B;
                        }
                        break;
                }
                sb.Append(ch);
                if (!nullable.HasValue)
                {
                    sb.AppendLine();
                    AppendIndent(sb, ++count);
                }
                continue;
            Label_00B0:
                if (nullable.HasValue)
                {
                    sb.Append(ch);
                }
                else
                {
                    sb.AppendLine();
                    AppendIndent(sb, --count);
                    sb.Append(ch);
                }
                continue;
            Label_017B:
                if (nullable.HasValue || !char.IsWhiteSpace(ch))
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }
    }
}

