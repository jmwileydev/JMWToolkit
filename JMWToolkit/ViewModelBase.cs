#nullable enable
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JMWToolkit;

/// <summary>
/// My personal ViewModelBase class. It implements INotifyPropertyChanged like most others.
/// On top of that is adds commands to create RelayCommand's and AsyncRelayCommands. These
/// methods take an optional list of properties that will cause the NotifyCanExecuteChanged
/// method to be called.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    private Dictionary<string, List<IRelayCommand>> _propertiesToCommands = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName != null && _propertiesToCommands.ContainsKey(propertyName))
        {
            foreach(var command in _propertiesToCommands[propertyName])
            {
                command.NotifyCanExecuteChanged();
            }
        }
    }

    public AsyncRelayCommand CreateAsyncRelayCommand(Func<Task> execute, Func<bool> canExecute, params string[] properties)
    {
        var command = new AsyncRelayCommand(execute, canExecute);
        AddPropertiesAndCommands(command, properties);
        return command;
    }

    public AsyncRelayCommand<T> CreateAsyncRelayCommand<T>(Func<T?, Task> execute, Predicate<T?> canExecute, params string[] properties)
    {
        var command = new AsyncRelayCommand<T>(execute, canExecute);
        AddPropertiesAndCommands(command, properties);
        return command;
    }

    private void AddPropertiesAndCommands(IRelayCommand command, string[] properties)
    {
        foreach (var property in properties)
        {
            if (!_propertiesToCommands.ContainsKey(property))
            {
                _propertiesToCommands[property] = new();
            }

            _propertiesToCommands[property].Add(command);
        }
    }

    public RelayCommand CreateRelayCommand(Action execute, Func<bool> canExecute, params string[] properties)
    {
        var command = new RelayCommand(execute, canExecute);
        AddPropertiesAndCommands(command, properties);
        return command;
    }

    public RelayCommand<T> CreateRelayCommand<T>(Action<T?> execute, Predicate<T?> canExecute, params string[] properties)
    {
        var command = new RelayCommand<T>(execute, canExecute);
        AddPropertiesAndCommands(command, properties);
        return command;
    }
}



