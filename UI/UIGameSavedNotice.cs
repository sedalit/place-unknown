using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameSavedNotice : MonoBehaviour
{
    private Text noticeText;

    private void Start()
    {
        noticeText = GetComponentInChildren<Text>();

        SceneSerializer.GameSaved += OnGameSaved;
    }

    private void OnDisable() => SceneSerializer.GameSaved -= OnGameSaved;

    private void OnGameSaved() => noticeText.DOFade(1, 0.5f).OnComplete(() => { noticeText.DOFade(0, 0.5f); });
}
