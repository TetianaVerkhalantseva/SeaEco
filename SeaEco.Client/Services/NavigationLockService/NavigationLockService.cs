namespace SeaEco.Services.NavigationLockService;

public class NavigationLockService
{
    public bool IsNavigationLocked { get; private set; }

    public event Action? OnChange;

    public void Lock()
    {
        IsNavigationLocked = true;
        NotifyStateChanged();
    }

    public void Unlock()
    {
        IsNavigationLocked = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
