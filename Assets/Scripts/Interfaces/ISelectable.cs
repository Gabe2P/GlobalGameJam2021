//Written By Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public event Action OnSelect;
    public event Action OnUnselect;

    ISelectable Select(object source);

    ISelectable Unselect(object source);
}
