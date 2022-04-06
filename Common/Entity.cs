using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Базовый класс всех интерактивных объектов на сцене.
///</summary>

public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Название объекта для пользователя.
    /// </summary>
    [SerializeField] private string m_Nickname;

}
