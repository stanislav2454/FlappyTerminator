using System;

public class StartScreen : Window
{
    public event Action PlayButtonClicked;

    public override void Close() =>
        SetWindowState(MinAlpha, false, false);

    public override void Open() =>
        SetWindowState(MaxAlpha, true, true);

    protected override void OnButtonClick() =>
        PlayButtonClicked?.Invoke();
}