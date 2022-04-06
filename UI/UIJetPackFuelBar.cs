using UnityEngine;
using UnityEngine.UI;

public class UIJetPackFuelBar : MonoBehaviour
{
    [SerializeField] private JetPack targetJetPack;
    [SerializeField] private Slider barSlider;

    private void Start()
    {
        barSlider.maxValue = targetJetPack.MaxFuel;
        barSlider.value = barSlider.maxValue;
    }

    private void Update() => barSlider.value = targetJetPack.CurrentFuel;

}
