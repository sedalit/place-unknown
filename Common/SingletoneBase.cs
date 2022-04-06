using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletoneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singletone")]
    [SerializeField] private bool dontDestroyOnLoad;
    public static T Instance { get; set; }

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this as T;
        if(dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
