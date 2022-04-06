using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedDestructibleHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private UISigh sigh;

    private Destructible targetDestructible;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        CastRay();
        if (targetDestructible != null && sigh.ImageSigh.enabled == true && targetDestructible.IsDead == false)
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.maxValue = targetDestructible.MaxHitPoints;
            healthSlider.value = targetDestructible.CurrentHitPoints;
        }
        else
        {
            healthSlider.gameObject.SetActive(false);
        }
    }

    private void CastRay()
    {
        RaycastHit hit;
        Vector3 point = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = mainCamera.ScreenPointToRay(point);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent(out Destructible destructible))
            {
                targetDestructible = destructible;
            }
            else
            {
                targetDestructible = null;
            }
        }
    }

}
