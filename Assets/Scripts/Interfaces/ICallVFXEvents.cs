//Written By Gabriel Tupy 1-30-2021

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICallVFXEvents
{
    public event Action<Vector2, Quaternion, float> CallVFX;
}
