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

    
    private bool Dropping = false;
    private bool Bouncing = false;
    Transform DropTarget;
    [SerializeField]
    float BounceHeight;
    [SerializeField]
    float BounceRate;
    float BounceLimit;
    [SerializeField]
    float BounceCount;


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
            motor.angularDrag = itemType.Drag;
        }
    }
    private void Update()
    {
        if(Dropping)
        {
            DropBoi();
        }
        if(Bouncing)
        {
            ItemBounce();
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

    public void StartDrop(Transform targetSpot)
    {
        Dropping = true;
        DropTarget = targetSpot;

        BounceHeight = .2f;
        BounceRate = 10;
        BounceLimit = 6;

        GetComponentInChildren<PolygonCollider2D>().enabled = false;
    }
    public void DropBoi()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 30 * Time.deltaTime, transform.position.z);


        if (transform.position.y - DropTarget.position.y < .1f)
        {
            Dropping = false;
            Bouncing = true;
            
            
        }
    }
    public void ItemBounce()
    {
        float BouncePosition = Mathf.Sin(Time.time * 30 ) * BounceHeight;

         transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y + BouncePosition,DropTarget.position.y,DropTarget.position.y +1), transform.position.z);

        if(transform.position.y <= DropTarget.position.y)
        {
            BounceCount++;
            BounceHeight = BounceHeight * .85f;
            BounceRate = BounceRate * .85f;
        }

        if(BounceCount >= BounceLimit)
        {
            Bouncing = false;
            GetComponentInChildren<PolygonCollider2D>().enabled = true;


        }
       
    }

    //public void SetItem(ItemType myItem , Rigidbody2D body, HingeJoint2D hinge)
    //{
    //    itemType = myItem;
    //    joint = hinge;
    //    motor = body;
    //}
        private void OnApplicationPause(bool pause)
    {

    }
}
