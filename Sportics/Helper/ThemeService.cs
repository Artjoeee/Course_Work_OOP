using MaterialDesignThemes.Wpf;

public class ThemeService
{
    private static readonly ThemeService _instance = new ThemeService();
    public static ThemeService Instance => _instance;

    private readonly PaletteHelper _paletteHelper = new PaletteHelper();

    private ThemeService() { }

    public bool IsDarkTheme
    {
        get
        {
            Theme theme = _paletteHelper.GetTheme();
            return theme.GetBaseTheme() == BaseTheme.Dark;
        }
    }

    public void SetDarkTheme()
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(BaseTheme.Dark);
        _paletteHelper.SetTheme(theme);
    }

    public void SetLightTheme()
    {
        var theme = _paletteHelper.GetTheme();
        theme.SetBaseTheme(BaseTheme.Light);
        _paletteHelper.SetTheme(theme);
    }
}

