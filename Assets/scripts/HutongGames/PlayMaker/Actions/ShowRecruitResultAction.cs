namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;

    [Tooltip("show the result of recruit"), ActionCategory("MTD")]
    public class ShowRecruitResultAction : FsmStateAction
    {
        public override void OnEnter()
        {
            FsmString str = base.Fsm.Variables.FindFsmString("TransitionParameter");
            if ((str != null) && (str.Value != null))
            {
                string str2 = str.Value;
                if (!string.IsNullOrEmpty(str2))
                {
                    char[] separator = new char[] { ',' };
                    string[] strArray = str2.Split(separator);
                    if (strArray.Length == 5)
                    {
                        char[] chArray2 = new char[] { '|' };
                        string[] strArray2 = strArray[0].Split(chArray2);
                        char[] chArray3 = new char[] { '|' };
                        string[] strArray3 = strArray[1].Split(chArray3);
                        char[] chArray4 = new char[] { '|' };
                        string[] strArray4 = strArray[2].Split(chArray4);
                        char[] chArray5 = new char[] { '|' };
                        string[] strArray5 = strArray[3].Split(chArray5);
                        char[] chArray6 = new char[] { '|' };
                        string[] strArray6 = strArray[4].Split(chArray6);
                        List<int> list = new List<int>();
                        List<int> list2 = new List<int>();
                        List<int> list3 = new List<int>();
                        List<int> list4 = new List<int>();
                        List<int> list5 = new List<int>();
                        foreach (string str3 in strArray2)
                        {
                            if (!string.IsNullOrEmpty(str3) && ("-1" != str3))
                            {
                                list.Add(Convert.ToInt32(str3));
                            }
                        }
                        foreach (string str4 in strArray3)
                        {
                            if (!string.IsNullOrEmpty(str4) && ("-1" != str4))
                            {
                                list2.Add(Convert.ToInt32(str4));
                            }
                        }
                        foreach (string str5 in strArray4)
                        {
                            if (!string.IsNullOrEmpty(str5) && ("-1" != str5))
                            {
                                list3.Add(Convert.ToInt32(str5));
                            }
                        }
                        foreach (string str6 in strArray5)
                        {
                            if (!string.IsNullOrEmpty(str6) && ("-1" != str6))
                            {
                                list4.Add(Convert.ToInt32(str6));
                            }
                        }
                        foreach (string str7 in strArray6)
                        {
                            if (!string.IsNullOrEmpty(str7) && ("-1" != str7))
                            {
                                list5.Add(Convert.ToInt32(str7));
                            }
                        }
                        GameDataMgr.Instance.boostRecruit.ShowRecruitResult(list, list2, list3, list4, list5, false);
                    }
                }
            }
        }
    }
}

