# JMWToolkit Dot Net Helpers
This repository will be for the JMWToolkit of helpers for C# and WPF

## IconHelper
This class is actually a copy of a class that I found online. It was posted in a blog by Christian Moser. Here
is a link to the article - https://www.wpftutorial.net/RemoveIcon.html. The class is used to remove the icon
from the title bar of a .Net Window. I use it in my MessageBoxEx class which is used to display a custom message
box into which I can multiple lines.

## MutexHelper
This is an RIA helper class used to manage the lifetime of a mutex. It should be used in a using statement
```
public class MyClass
{
    private Mutex _mutex;

    public void SomeRoutine()
    {
        using(var helper = new MutexHelper(_mutex)
        {
            // some statements requiring the mutex.
            // the constructor will call Mutex.WaitOne()
            // and then the mutex will exit when dispose
            // is called when the using statement is exited.
        }
    }
}
```

## StringHelper
This class is simply a place to throw some helpers for Strings. I am sure it will grow over time.

**LoadAndFormatResource(string format, params object[] args)** - This routine is used to load a String resource and format it using the
supplied arguments.<br>
*String format - this is the identifier for the string resource to be loaded.
*object[] args - this is the arguments to be subsituted into the string

**LoadStringResource(string resourceid)** - This routine simply loads the desired resource and casts it to a string.

## ViewModelBase
This class is a ViewModel base class that implements INotifyPropertyChanged. It also adds routines to create 
*CommunityToolkit.Mvvm.Input.RelayCommand's* and *CommunityToolkit.Mvvm.Input.AsynRelayCommand's*. The functions take an function to
execute, a predicate for CanExecute and also a list of properties that could cause the CanExecute value to change. When OnPropertyChanged
is called if needed it will call **Command.NotifyCanExecuteChanged** to let the CommandManager know about the possible change.<br>

**RelayCommand CreateRelayCommand(Action execute, Func<bool> canExecute, params string[] properties)**
**RelayCommand<T> CreateRelayCommand<T>(Action<T?> execute, Predicate<T?> canExecute, params string[] properties)**
**AsyncRelayCommand CreateAsyncRelayCommand(Func<Task> execute, Func<bool> canExecute, params string[] properties)**
**AsyncRelayCommand<T> CreateAsyncRelayCommand<T>(Func<T?, Task> execute, Predicate<T?> canExecute, params string[] properties)**

TODO add more comments for the above commands



