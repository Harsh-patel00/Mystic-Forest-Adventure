using System;

public class EventManager
{
    public static Action SpinStart;
    public static Action SpinStop;

    public static void NotifySpinStart()
    {
        SoundManager.instance.PlayReelSpinSound();
        SpinStart?.Invoke();
    }

    public static void NotifySpinStop()
    {
        SoundManager.instance.PlayReelStopSound();
        SpinStop?.Invoke();
    }
}
