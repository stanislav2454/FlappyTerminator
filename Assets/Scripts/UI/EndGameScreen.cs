using System;

public class EndGameScreen : Window
{
    public event Action RestartButtonClicked;

    public override void Close() =>
        SetWindowState(MinAlpha, false, false);

    public override void Open() =>
        SetWindowState(MaxAlpha, true, true);

    protected override void OnButtonClick() =>
        RestartButtonClicked?.Invoke();
}