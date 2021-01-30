//Written by Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Joint2D))]
public class ItemController : MonoBehaviour, IGrabbable, ICallAnimateEvents
{
    public event Action OnGrab;
    public event Action OnRelease;
    public event Action<string, object> CallAnimationTrigger;

    [SerializeField] private ItemType itemType = null;
    private Rigidbody2D motor = null;
    private Joint2D joint = null;

    private Vector2 force = Vector2.zero;
    private Vector2 contactPoint = Vector2.zero;

    // Start is called before the first frame update
    void Awake()
    {
        motor = GetComponent<Rigidbody2D>();
        joint = GetComponent<Joint2D>();

        if (itemType != null)
        {
            motor.mass = itemType.Mass;
            motor.drag = itemType.Drag;
        }
    }

    public IGrabbable Grab(Rigidbody2D player, Vector2 contactPoint)
    {
        HingeJoint2D hinge = joint as HingeJoint2D;
        if (hinge != null)
        {
            hinge.anchor = hinge.transform.InverseTransformPoint(contactPoint);
            hinge.connectedAnchor = player.transform.InverseTransformPoint(contactPoint); ;
        }
        joint.enabled = true;
        joint.connectedBody = player;
        OnGrab?.Invoke();
        return this;
    }

    public void Release(Vector2 input)
    {
        joint.connectedBody = null;
        joint.enabled = false;

        if (input == Vector2.zero)
        {
            motor.velocity = Vector3.zero;
        }
        OnRelease?.Invoke();
    }

    //public IGrabbable Grab(object source)
    //{
    //    joint.enabled = true;
    //    GameObject obj = source as GameObject;
    //    if (obj != null)
    //    {
    //        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    //        if (rb != null)
    //        {
    //            joint.connectedBody = rb;
    //        }
    //    }
    //    return this;
    //}

    //public void Release(object source)
    //{
    //    joint.connectedBody = null;
    //    joint.enabled = false;
    //}
}
