//Written by Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class ItemController : MonoBehaviour, IGrabbable, ICallAnimateEvents, ICallAudioEvents
{
    public event Action OnGrab;
    public event Action OnRelease;
    public event Action<string, object> CallAnimationTrigger;
    public event Action<string, int> CallAnimationState;
    public event Action<string> CallAudio;

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

    public void SetItem(ItemType myItem)
    {
        itemType = myItem;
    }
        private void OnApplicationPause(bool pause)
    {

    }
}
