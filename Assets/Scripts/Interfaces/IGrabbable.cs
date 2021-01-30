//Written By Gabriel Tupy 1-29-2021

using System;
using UnityEngine;

public interface IGrabbable
{
    public event Action OnGrab;
    public event Action OnRelease;

    IGrabbable Grab(Rigidbody2D player, Vector2 contactPoint);

    void Release(Vector2 input);
}
