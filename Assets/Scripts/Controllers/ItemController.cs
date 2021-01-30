//Written by Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class ItemController : MonoBehaviour, IGrabbable, ICallAnimateEvents
{
    public event Action OnGrab;
    public event Action OnRelease;
    public event Action<string, object> CallAnimationTrigger;

    [SerializeField] private ItemType itemType = null;
    private Rigidbody2D motor = null;
    private HingeJoint2D joint = null;

    private Vector2 force = Vector2.zero;
    private Vector2 contactPoint = Vector2.zero;

    public ItemType Item { get { return itemType; } }

    // Start is called before the first frame update
    void Awake()
    {
        motor = GetComponent<Rigidbody2D>();
        joint = GetComponent<HingeJoint2D>();

        if (itemType != null)
        {
            motor.mass = itemType.Mass;
            motor.drag = itemType.Drag;
        }
    }

    public IGrabbable Grab(Rigidbody2D player, Vector2 contactPoint)
    {
        joint.anchor = joint.transform.InverseTransformPoint(contactPoint);
        joint.connectedAnchor = player.transform.InverseTransformPoint(contactPoint); ;
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
<<<<<<< HEAD

    public void SetItem(ItemType myItem)
    {
        itemType = myItem;
    }
        private void OnApplicationPause(bool pause)
    {

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
=======
>>>>>>> 56ba50a13934f926823b109ed55a5e841c9cc048
}
