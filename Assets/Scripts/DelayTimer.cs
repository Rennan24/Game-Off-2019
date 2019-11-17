using UnityEngine;

[System.Serializable]
public struct DelayTimer
{
    public float DelayTime;
    private float time;

    public DelayTimer(float delayTime)
    {
        DelayTime = delayTime;
        time = Time.time;
    }

    public bool HasFinished()
    {
        if (Time.time < time + DelayTime)
            return false;

        time = Time.time;
        return true;
    }

    public float TimeLeft()
    {
        var timeLeft = (time + DelayTime) - Time.time;
        return Mathf.Max(0, timeLeft);
    }
}
