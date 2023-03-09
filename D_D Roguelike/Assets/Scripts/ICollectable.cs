using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable<T>
{
    GameObject gameObject { get; }
    public T Pickup();
}
