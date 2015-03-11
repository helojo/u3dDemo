using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TeamMove
{
    public void ResumeMove()
    {
        if (this.teamMove != null)
        {
            this.teamMove.enabled = true;
        }
        if (this.teamMove2 != null)
        {
            this.teamMove2.enabled = true;
        }
        if (this.audio != null)
        {
            this.audio.Play();
        }
    }

    public void StopMove()
    {
        if (this.teamMove != null)
        {
            this.teamMove.enabled = false;
            this.teamMove.isPaused = true;
        }
        if (this.teamMove2 != null)
        {
            this.teamMove2.enabled = false;
            this.teamMove2.isPaused = true;
        }
        if (this.audio != null)
        {
            this.audio.Pause();
        }
    }

    public AudioSource audio { get; set; }

    public iTween teamMove { get; set; }

    public iTween teamMove2 { get; set; }
}

