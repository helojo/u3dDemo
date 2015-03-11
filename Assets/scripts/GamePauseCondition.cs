using System;

public class GamePauseCondition
{
    public DateTime? PauseEndTime;
    public DateTime? PauseStartTime;

    public void Log()
    {
    }

    public int TotalPauseTime
    {
        get
        {
            int totalSeconds = 0;
            if (this.PauseStartTime.HasValue && this.PauseEndTime.HasValue)
            {
                DateTime? pauseStartTime;
                DateTime? pauseEndTime = this.PauseEndTime;
                if (pauseEndTime.HasValue)
                {
                    pauseStartTime = this.PauseStartTime;
                }
                TimeSpan? nullable = !pauseStartTime.HasValue ? null : new TimeSpan?(pauseEndTime.Value - pauseStartTime.Value);
                if (nullable.HasValue)
                {
                    totalSeconds = (int) nullable.Value.TotalSeconds;
                }
                return totalSeconds;
            }
            if ((this.PauseStartTime.HasValue || !this.PauseEndTime.HasValue) && (!this.PauseStartTime.HasValue || this.PauseEndTime.HasValue))
            {
                return totalSeconds;
            }
            return -1;
        }
    }
}

