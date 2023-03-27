using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver<in T>
{
    void NewItemAdded(T type);

    void ItemRemoved(T type);

    void ItemAltered(T type, int count);
}
