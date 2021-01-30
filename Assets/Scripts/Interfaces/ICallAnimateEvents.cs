//Written By Gabriel Tupy 1-29-2021

using System;

public interface ICallAnimateEvents
{
    public event Action<string, object> CallAnimationTrigger;
    public event Action<string, int> CallAnimationState;
}
