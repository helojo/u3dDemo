using fastJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfo
{
    private string chat_url = string.Empty;
    private string clientChannel;
    private string clientVersion;
    private string community_url = string.Empty;
    public System.Action failedcallback;
    public List<GameAdInfo> gameAdInfos = new List<GameAdInfo>();
    private string gameId;
    public List<GameServerInfo> gameServerInfos = new List<GameServerInfo>();
    public int gameServerMaxIndex;
    private static ServerInfo instance;
    public string patch_url = string.Empty;
    public string playerInfo_url = string.Empty;
    public string share_url = string.Empty;
    public System.Action succeedcallback;
    public string update_url = string.Empty;

    private ServerInfo()
    {
    }

    public GameServerInfo getCurrentGameServerInfo()
    {
        foreach (GameServerInfo info in this.gameServerInfos)
        {
            if (info.serverId == lastGameServerId)
            {
                return info;
            }
        }
        return null;
    }

    public GameServerInfo GetGameServerInfoBySvrId(int _SvrId)
    {
        foreach (GameServerInfo info in this.gameServerInfos)
        {
            if (info.serverId == _SvrId)
            {
                return info;
            }
        }
        return null;
    }

    public static ServerInfo getInstance()
    {
        if (instance == null)
        {
            instance = new ServerInfo();
        }
        return instance;
    }

    public List<GameServerInfo> GetServerInfoByPage(int page)
    {
        List<GameServerInfo> list = new List<GameServerInfo>();
        int num = 0;
        foreach (GameServerInfo info in this.gameServerInfos)
        {
            if (num >= 10)
            {
                return list;
            }
            if (((info.index > (page * 10)) && (info.index <= ((page + 1) * 10))) && (info.status != 5))
            {
                list.Add(info);
                num++;
            }
            if (info.index >= this.gameServerMaxIndex)
            {
                return list;
            }
        }
        return list;
    }

    private void ParseRoleList(string content)
    {
        Debug.Log(content);
        List<object> list = JSON.Instance.ToObject<List<object>>(content);
        if (list == null)
        {
            LoginPanel component = GameObject.Find("UI Root/Camera/LoginPanel").GetComponent<LoginPanel>();
            if (null != component)
            {
                component.InitRoleList(null);
            }
        }
        else if (list != null)
        {
            List<RoleInfo> roleInfoList = new List<RoleInfo>();
            foreach (object obj2 in list)
            {
                Dictionary<string, object> dictionary = (Dictionary<string, object>) obj2;
                RoleInfo item = new RoleInfo {
                    open_id = dictionary["open_id"].ToString(),
                    nick_name = dictionary["nick_name"].ToString(),
                    last_time = dictionary["last_time"].ToString()
                };
                if (!int.TryParse(dictionary["level"].ToString(), out item.level))
                {
                    item.level = 0;
                }
                if (!int.TryParse(dictionary["head_frame"].ToString(), out item.head_frame))
                {
                    item.head_frame = 0;
                }
                if (!int.TryParse(dictionary["head"].ToString(), out item.head_entry))
                {
                    item.head_entry = 0;
                }
                if (!int.TryParse(dictionary["server_id"].ToString(), out item.server_id))
                {
                    item.server_id = -1;
                }
                else
                {
                    roleInfoList.Add(item);
                }
            }
            LoginPanel panel2 = GameObject.Find("UI Root/Camera/LoginPanel").GetComponent<LoginPanel>();
            if (null != panel2)
            {
                panel2.InitRoleList(roleInfoList);
            }
        }
    }

    private void ParseServerList(string content)
    {
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(content);
        if (dictionary != null)
        {
            if (dictionary.ContainsKey("GameConfigInfo"))
            {
                Dictionary<string, object> dictionary2 = (Dictionary<string, object>) dictionary["GameConfigInfo"];
                if (dictionary2 != null)
                {
                    object obj2;
                    object obj3;
                    object obj4;
                    object obj5;
                    this.patch_url = dictionary2["PatchUrl"].ToString();
                    this.update_url = dictionary2["UpdateUrl"].ToString();
                    this.share_url = dictionary2["ShareUrl"].ToString();
                    if (dictionary2.TryGetValue("CommunityUrl", out obj2))
                    {
                        this.community_url = obj2.ToString();
                    }
                    else
                    {
                        this.community_url = this.update_url;
                    }
                    if (dictionary2.TryGetValue("ChatUrl", out obj3))
                    {
                        this.chat_url = obj3.ToString();
                    }
                    else
                    {
                        this.chat_url = "http://192.168.29.112:88/";
                    }
                    this.gameServerMaxIndex = int.Parse(dictionary2["ServerMaxIndex"].ToString());
                    if (dictionary2.TryGetValue("PlayerInfoUrl", out obj4))
                    {
                        this.playerInfo_url = obj4.ToString();
                    }
                    bool flag = StrParser.ParseDecInt(dictionary2["IsAuth"].ToString()) != 0;
                    GameDefine.getInstance().isReleaseServer = !flag;
                    if (dictionary2.TryGetValue("ServerInfoUrl", out obj5))
                    {
                        this.StartLoadPlayerInfo(obj5.ToString());
                    }
                    else
                    {
                        Debug.Log("Error SververInfoUrl------->");
                    }
                }
            }
            this.gameServerInfos.Clear();
            if (dictionary.ContainsKey("GameServerInfo"))
            {
                object obj6 = dictionary["GameServerInfo"];
                if (obj6 != null)
                {
                    List<object> list = (List<object>) obj6;
                    foreach (object obj7 in list)
                    {
                        GameServerInfo item = new GameServerInfo();
                        Dictionary<string, object> dictionary3 = (Dictionary<string, object>) obj7;
                        item.serverId = StrParser.ParseDecInt(dictionary3["D"].ToString());
                        item.name = dictionary3["N"].ToString();
                        item.ip = dictionary3["P"].ToString();
                        item.port = StrParser.ParseDecInt(dictionary3["T"].ToString());
                        item.status = StrParser.ParseDecInt(dictionary3["S"].ToString());
                        item.index = StrParser.ParseDecInt(dictionary3["I"].ToString());
                        if (dictionary3.ContainsKey("B"))
                        {
                            item.backIPList = StrParser.ParseStringList(dictionary3["B"].ToString());
                        }
                        this.gameServerInfos.Add(item);
                    }
                }
            }
            this.gameAdInfos.Clear();
            if (dictionary.ContainsKey("GameAdInfo"))
            {
                object obj8 = dictionary["GameAdInfo"];
                if (obj8 != null)
                {
                    List<object> list2 = (List<object>) obj8;
                    foreach (object obj9 in list2)
                    {
                        GameAdInfo info2 = new GameAdInfo();
                        Dictionary<string, object> dictionary4 = (Dictionary<string, object>) obj9;
                        if (dictionary4 != null)
                        {
                            info2.StartTime = DateTime.Parse(dictionary4["StartTime"].ToString());
                            info2.EndTime = DateTime.Parse(dictionary4["EndTime"].ToString());
                            info2.Timing = long.Parse(dictionary4["Timing"].ToString());
                            info2.GameServerId = dictionary4["ID"].ToString();
                            info2.Priority = StrParser.ParseDecInt(dictionary4["Priority"].ToString());
                            info2.UserType = dictionary4["UserType"].ToString();
                            info2.ShowType = StrParser.ParseDecInt(dictionary4["ShowType"].ToString(), -1);
                            info2.Desc = dictionary4["Desc"].ToString();
                            info2.Url = dictionary4["Url"].ToString();
                        }
                        this.gameAdInfos.Add(info2);
                    }
                }
            }
            if (dictionary.ContainsKey("GameExchangeInfo"))
            {
                PayMgr.Instance.ParseAppProducts((List<object>) dictionary["GameExchangeInfo"]);
            }
            Debug.Log("Server Info Download success!!!");
        }
    }

    public void StartDownload(string host, string _gameId, string _clientVersion, string _channel, System.Action succeedcb, System.Action failedcb)
    {
        this.succeedcallback = succeedcb;
        this.failedcallback = failedcb;
        this.gameId = _gameId;
        this.clientVersion = _clientVersion;
        this.clientChannel = _channel;
        object[] args = new object[] { host, this.gameId, this.clientVersion, this.clientChannel, UnityEngine.Random.Range(0, 0x186a0) };
        string message = string.Format("http://{0}/{1}/{2}/{3}?t={4}", args);
        Debug.Log(message);
        WebPageDownLoad.Begin(null, message, delegate (string content) {
            Debug.Log(content);
            if (!string.IsNullOrEmpty(content))
            {
                this.ParseServerList(content);
                this.succeedcallback();
            }
            else
            {
                this.failedcallback();
            }
        });
    }

    private void StartLoadPlayerInfo(string _PayerInfoSvrUrl)
    {
        Debug.Log(GameDefine.getInstance().lastAccountName);
        object[] args = new object[] { _PayerInfoSvrUrl, GameDefine.getInstance().lastAccountName, GameDefine.getInstance().clientChannel, UnityEngine.Random.Range(0, 0x186a0) };
        string message = string.Format("{0}?open_id={1}&channel_id={2}&t={3}", args);
        Debug.Log(message);
        WebPageDownLoad.Begin(null, message, delegate (string content) {
            if (!string.IsNullOrEmpty(content))
            {
                this.ParseRoleList(content);
                Debug.Log("DownPlayerRoleInfo Success!");
            }
            else
            {
                Debug.Log("DownPlayerRoleInfo failed!");
            }
        });
    }

    public string Chat_Url
    {
        get
        {
            return this.chat_url;
        }
    }

    public string Community_url
    {
        get
        {
            return this.community_url;
        }
    }

    public static int lastGameServerId
    {
        get
        {
            return PlayerPrefs.GetInt(GameDefine.getInstance().clientChannel + "lastGameServerId");
        }
        set
        {
            PlayerPrefs.SetInt(GameDefine.getInstance().clientChannel + "lastGameServerId", value);
        }
    }

    public class GameAdInfo
    {
        public string Desc;
        public DateTime EndTime;
        public string GameServerId;
        public int Priority;
        public int ShowType;
        public DateTime StartTime;
        public long Timing;
        public string Url;
        public string UserType;
    }

    public class GameServerInfo
    {
        public List<string> backIPList = new List<string>();
        public bool enable;
        public string host;
        public int index;
        public string ip;
        public string name;
        public int order;
        public int port;
        public int serverId;
        public int status;
        public string tv_url;
        public int weight;
    }
}

