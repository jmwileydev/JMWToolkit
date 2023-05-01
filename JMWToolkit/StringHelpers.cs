using System.Windows;

namespace JMWToolkit;

/// <summary>
/// Static StringHelpers. I am sure this class will grow over time. For now two simple
/// routines to load a string resource.
/// </summary>
public static class StringHelpers
{
    public static string LoadAndFormatResource(string format, params object[] args)
    {
        var string_format = (string)Application.Current.FindResource(format);
        return string.Format(string_format, args);
    }

    public static string LoadStringResource(string resourceId)
    {
        return (string)Application.Current.FindResource(resourceId);
    }
}
