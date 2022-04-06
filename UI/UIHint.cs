using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIHint : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    [SerializeField] private float activeRadius;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform lookTransform;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas.worldCamera = Camera.main;
        lookTransform = Camera.main.transform;
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (canvas.worldCamera == null) canvas.worldCamera = Camera.main;
        if (lookTransform == null) lookTransform = Camera.main.transform;

        hint.transform.LookAt(lookTransform);
        if (Vector3.Distance(transform.position, lookTransform.position) < activeRadius)
        {
            Show();
        }
        else
        {
            Close();
        }
    }

    private void Show() => canvasGroup.DOFade(1, 0.5f).OnComplete(() => { canvasGroup.alpha = 1; });

    private void Close()
    {
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => { 
            canvasGroup.alpha = 0;
            return;
        });
    }
}
