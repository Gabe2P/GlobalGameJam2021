using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICallAudioEvents
{
    public event Action<string> CallAudio;
}
