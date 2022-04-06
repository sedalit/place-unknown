using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject TimerCollector;

    public event UnityAction OnTimeOver;
    public event UnityAction OnTick;

    public bool IsLoop;

    private float maxValue;
    private float currentValue;
    private bool isPaused;

    public float MaxValue => maxValue;
    public float CurrentValue => currentValue;
    public bool IsPaused => isPaused;
    public bool IsCompleted => currentValue <= 0;

    private void Update()
    {
        if (isPaused) return;
        currentValue -= Time.deltaTime;
        OnTick?.Invoke();
        if (currentValue <= 0)
        {
            currentValue = 0;
            OnTimeOver?.Invoke();
            if (IsLoop)
            {
                currentValue = maxValue;
            }
        }

    }
    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }
        Timer timer = TimerCollector.AddComponent<Timer>();
        timer.maxValue = time;
        timer.IsLoop = isLoop;
        return timer;
    }

    public static Timer CreateTimer(float time)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }
        Timer timer = TimerCollector.AddComponent<Timer>();
        timer.maxValue = time;
        return timer;
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Stop()
    {
        isPaused = false;
        currentValue = 0;
    }

    public void Restart()
    {
        currentValue = maxValue;
    }

    public void Restart(float time)
    {
        maxValue = time;
        currentValue = maxValue;
    }

    public void CompleteWithoutEvent()
    {
        Destroy(this);
    }
}
