//Written By Gabriel Tupy 1-30-2021

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICallAudioEvents
{
    public event Action<string, float> PlayAudio;
    public event Action<string> StopAudio;
}
