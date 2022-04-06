using UnityEngine;

[CreateAssetMenu()]
public class QuestProperties : ScriptableObject
{
    [TextArea]
    [SerializeField] private string description;

    [TextArea]
    [SerializeField] private string task;

    public string Description => description;
    public string Task => task;
}
