using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject<T>
{
    void RegisterObserver(IObserver<T> o);
    void RemoveObserver(IObserver<T> o);
    void NotifyObservers(T type, NotificationType notificationType);

    public enum NotificationType
    {
        Added,
        Removed,
        Changed
    }
}
