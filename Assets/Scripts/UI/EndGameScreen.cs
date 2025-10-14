using System;

public class EndGameScreen : Window
{
    public event Action RestartButtonClicked;

    public override void Close()
    {
        WindowGroup.alpha = MinAlpha;
        ActionButton.interactable = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = MaxAlpha;
        ActionButton.interactable = true;
    }

    protected override void OnButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }
}