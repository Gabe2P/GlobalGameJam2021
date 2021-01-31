//Written By Gabriel Tupy 1-30-2021

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICallParticleEvents
{
    public event Action<Vector2> CallParticles;
}
