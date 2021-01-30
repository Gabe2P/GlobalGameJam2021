//Written By Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public event Action OnInteraction;
    object Interact(object source);
}
