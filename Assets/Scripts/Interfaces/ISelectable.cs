//Written By Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public event Action OnSelect;
    public event Action OnUnselect;

    object Select(object source);

    object Unselect(object source);
}
