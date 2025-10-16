using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    protected const float MinAlpha = 0f;
    protected const float MaxAlpha = 1f;

    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _actionButton;

    protected CanvasGroup WindowGroup => _windowGroup;
    protected Button ActionButton => _actionButton;

    private void OnEnable()
    {
        _actionButton?.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _actionButton?.onClick.RemoveListener(OnButtonClick);
    }

    public abstract void Open();
    public abstract void Close();

    protected abstract void OnButtonClick();

    protected void SetWindowState(float alpha, bool interactable, bool blocksRaycasts)
    {
        if (WindowGroup != null)
        {
            WindowGroup.alpha = alpha;
            WindowGroup.interactable = interactable;
            WindowGroup.blocksRaycasts = blocksRaycasts;
        }

        if (ActionButton != null)
            ActionButton.interactable = interactable;
    }
}