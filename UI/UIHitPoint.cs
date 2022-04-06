using UnityEngine;
using UnityEngine.UI;

public class UIHitPoint : MonoBehaviour
{
    [SerializeField] private Destructible targetDestructible;
    [SerializeField] private Slider slider;

    private void Start() => slider.maxValue = targetDestructible.MaxHitPoints;

    private void Update() => slider.value = targetDestructible.CurrentHitPoints;
 
}
