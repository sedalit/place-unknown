using UnityEngine;

public class Quest_ReachTrigger : Quest
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Completed?.Invoke();
        }
    }
}
