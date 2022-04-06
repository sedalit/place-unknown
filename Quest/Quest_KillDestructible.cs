using UnityEngine;

public class Quest_KillDestructible : Quest
{
    [SerializeField] private Destructible[] targets;

    private int destroyedTargetsCount = 0;

    private void Start()
    {
        foreach (var target in targets)
        {
            target.EventOnDeath.AddListener(OnTargetDeath);
        }
    }

    private void OnTargetDeath()
    {
        destroyedTargetsCount++;

        if (destroyedTargetsCount == targets.Length)
        {
            foreach (var target in targets)
            {
                target.EventOnDeath.RemoveListener(OnTargetDeath);
            }
            Completed?.Invoke();
        }
    }
}
