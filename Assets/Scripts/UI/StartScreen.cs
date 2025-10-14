using System;

public class StartScreen : Window
{
    public event Action PlayButtonClicked;

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
        PlayButtonClicked?.Invoke();
    }
}
