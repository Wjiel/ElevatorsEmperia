using DG.Tweening;
using UnityEngine;

public class UIAnimationPanel : MonoBehaviour
{
    private RectTransform panelRectTransform;
    private Vector2 targetPosition;
    private float duration = .25f;

    private Vector2 offscreenPosition;

    private void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();

        offscreenPosition = panelRectTransform.anchoredPosition;
        targetPosition = new Vector2(0, panelRectTransform.anchoredPosition.y);

        panelRectTransform.anchoredPosition = new Vector2(offscreenPosition.x, -Screen.height);

        ShowPanel();
    }

    void OnEnable()
    {
        if (panelRectTransform != null)
        {
            panelRectTransform.anchoredPosition = new Vector2(offscreenPosition.x, -Screen.height);

            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        panelRectTransform.DOAnchorPos(targetPosition, duration)
             .SetEase(Ease.InOutCirc)
            .SetUpdate(true);
    }

    public void HidePanel()
    {
        panelRectTransform.DOAnchorPos(new Vector2(offscreenPosition.x, -Screen.height), duration)
             .SetEase(Ease.InOutCirc)
            .SetUpdate(true);
    }
}
