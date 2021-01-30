//Written By Gabriel Tupy 1-29-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    IGrabbable Grab(object source);

    void Release(object source);
}
