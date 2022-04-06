using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Класс уничтожаемого объекта. То, что может иметь хитпоинты
/// </summary>
public class Destructible : Entity, ISerializableEntity
{
    #region Properties
    /// <summary>
    /// Объект игнорирует повреждения
    /// </summary>
    [SerializeField] private bool isIndestructible;
    /// <summary>
    /// Стартовое количество хитпоинтов
    /// </summary>
    [SerializeField] private int maxHitPoints;
    public int MaxHitPoints => maxHitPoints;
    /// <summary>
    /// Текущие хитпоинты
    /// </summary>
    protected int currentHitPoints;
    public int CurrentHitPoints => currentHitPoints;

    protected bool isDead = false;
    public bool IsDead => isDead;
    #endregion

    #region  Unity Events
    protected virtual void Start()
    {
        currentHitPoints = maxHitPoints;
    }
    #endregion

    #region Public API
    /// <summary>
    /// Применение урона к объекту
    /// </summary>
    public void ApplyDamage(int damage, Destructible other)
    {
        if (isIndestructible) return;
        if (currentHitPoints <= 0 && isDead) return;
        currentHitPoints -= damage;
        eventOnGetDamage?.Invoke();
        GetDamage?.Invoke(other);
        if (currentHitPoints <= 0)
        {
            isDead = true;
            OnDeath();
        }
    }

    public void Heal(int value)
    {
        if (currentHitPoints < maxHitPoints)
        {
            currentHitPoints += value;
        }
    }

    public void RestoreMaxHitPoint()
    {
        if (currentHitPoints < maxHitPoints)
        {
            currentHitPoints = maxHitPoints;
        }
    }
    #endregion

    /// <summary>
    /// Переопределяемое событие уничтожения объекта, когда хитпоинты меньше или равны нулю
    /// </summary>
    protected virtual void OnDeath()
    {
        eventOnDeath?.Invoke();
        Destroy(gameObject, 1f);
    }

    private static HashSet<Destructible> allDestructibles;
    public static IReadOnlyCollection<Destructible> AllDestructibles => allDestructibles;

    protected virtual void OnEnable()
    {
        if (allDestructibles == null)
            allDestructibles = new HashSet<Destructible>();
        allDestructibles.Add(this);
    }
    protected virtual void OnDestroy()
    {
        allDestructibles.Remove(this);
    }

    [SerializeField] private int scoreValue;
    public int ScoreValue => scoreValue;

    public const int TeamIdNeutral = 0;
    [SerializeField] private int teamId;
    public int TeamId => teamId;

    [SerializeField] private UnityEvent eventOnDeath;
    public UnityEvent EventOnDeath => eventOnDeath;


    [SerializeField] private UnityEvent eventOnGetDamage;
    public UnityAction<Destructible> GetDamage;

    public static List<Destructible> GetAllTeamMembers(int id)
    {
        List<Destructible> team = new List<Destructible>();
        foreach (var dest in allDestructibles)
        {
            if (dest.TeamId == id) team.Add(dest);
        }
        return team;
    }

    public static List<Destructible> GetAllNoneTeamMembers(int id)
    {
        List<Destructible> noneTeam = new List<Destructible>();
        foreach (var dest in allDestructibles)
        {
            if (dest.TeamId != id) noneTeam.Add(dest);
        }
        return noneTeam;
    }

    #region Serialize
    [System.Serializable]
    public class State
    {
        public Vector3 Position;
        public int CurrentHitPoints;
        public State(Destructible destructible)
        {
            Position = destructible.transform.position;
            CurrentHitPoints = destructible.CurrentHitPoints;
        }
    }

    [SerializeField] private int entityId;

    public long EntityId => entityId;

    public bool IsSerializable()
    {
        return currentHitPoints > 0;
    }

    public virtual string SerializeState()
    {
        State state = new State(this);
        return JsonUtility.ToJson(state);
    }

    public virtual void DeserializeState(string state)
    {
        State s = JsonUtility.FromJson<State>(state);
        transform.position = s.Position;
        currentHitPoints = s.CurrentHitPoints;
    }
    #endregion
}
