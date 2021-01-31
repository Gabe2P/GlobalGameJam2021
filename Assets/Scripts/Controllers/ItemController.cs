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
    public event Action<string, float> CallAudio;

    [SerializeField] private ItemType itemType = null;
    private Rigidbody2D motor = null;
    private HingeJoint2D joint = null;

    private bool isGrabbed = false;

    
    private bool Dropping = false;
    private bool Bouncing = false;
    private bool BounceUp = true;
    Transform DropTarget;
    
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
    }

    private void Update()
    {
        if (joint != null)
        {
            if (joint.connectedBody != null)
            {
                isGrabbed = true;
            }
            else
            {
                isGrabbed = false;
            }
        }

        if (motor != null)
        {
            if (motor.velocity.magnitude >= .1f)
            {
                CallAudio?.Invoke("Drag", 0f);
            }
            else
            {
                motor.velocity = Vector2.zero;
            }
        }
    }

    public IGrabbable Grab(Rigidbody2D player, Vector2 contactPoint)
    {
        if (joint != null)
        {
            joint.anchor = joint.transform.InverseTransformPoint(contactPoint);
            joint.connectedAnchor = player.transform.InverseTransformPoint(contactPoint); ;
            joint.enabled = true;
            joint.connectedBody = player;
            OnGrab?.Invoke();
        }
        return this;
    }

    public void Release(Vector2 input)
    {
        if (joint != null)
        {
            joint.connectedBody = null;
            joint.enabled = false;
            if (input == Vector2.zero)
            {
                motor.velocity = Vector3.zero;
            }
            OnRelease?.Invoke();
        }
    }

    public void StartDrop(Transform targetSpot)
    {
        Dropping = true;
        DropTarget = targetSpot;

        

        GetComponentInChildren<PolygonCollider2D>().enabled = false;
    }
    public void DropBoi()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 30 * Time.deltaTime, transform.position.z);


        if (transform.position.y - DropTarget.position.y < .1f)
        {
            Dropping = false;
            GetComponentInChildren<PolygonCollider2D>().enabled = true;

        }
    }


    public void ItemBounce()
    {
        float BouncePosition;
        //float BouncePosition = Mathf.Sin(Time.time * 30 ) * BounceHeight;
        if (BounceUp)
        {
            BouncePosition = Mathf.PingPong(Time.time, .1f);
        }
        else
        {
            BouncePosition = Mathf.PingPong(Time.time, .1f) * -1f;
        }

         transform.position = new Vector3(transform.position.x,transform.position.y + BouncePosition, transform.position.z);

        //if(transform.position.y <= DropTarget.position.y)
        //{
        //    BounceCount++;
        //    BounceHeight = BounceHeight * .85f;
        //    BounceRate = BounceRate * .85f;
        //}

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
