using FastBuf;
using System;
using System.Runtime.CompilerServices;

public class SocialUser
{
    public int Index { get; set; }

    public bool Owner { get; set; }

    public SocialPlatformFriend Plat { get; set; }

    public QQFriendUser QQUser { get; set; }

    public SocialUserInfo UserInfo { get; set; }
}

