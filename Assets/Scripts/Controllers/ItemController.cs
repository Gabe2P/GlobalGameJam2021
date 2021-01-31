//Written by Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class ItemController : MonoBehaviour, IGrabbable, ICallAnimateEvents, ICallAudioEvents, ISelectable
{
    public event Action OnGrab;
    public event Action OnRelease;
    public event Action<string, object> CallAnimationTrigger;
    public event Action<string, int> CallAnimationState;
    public event Action<string, float> PlayAudio;
    public event Action<string> StopAudio;

    [SerializeField] private ItemType itemType = null;
    [SerializeField] private GameObject Highlight = null;

    private Rigidbody2D motor = null;
    private HingeJoint2D joint = null;

    private bool isGrabbed = false;
    
    private bool Dropping = false;
    private bool Bouncing = false;
    private bool BounceUp = true;
    Transform DropTarget;

    
    float ExplosionForce = 500;
    
    float BounceLimit;
    [SerializeField]
    float BounceCount;

    GameObject GrabParticle;
    GameObject ActiveGrab;


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
    private void Start()
    {
        if (RequestManager.Instance != null)
        {
            GrabParticle = RequestManager.Instance.GrabSprite;
        }
    }

    private void Update()
    {
        if (Dropping)
        {
            DropBoi();
        }
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

        if (motor != null && Time.timeScale != 0)
        {
            if (motor.velocity.magnitude >= .1f)
            {
                if (isGrabbed)
                {
                    StopAudio?.Invoke("Pushed");
                    PlayAudio?.Invoke("Drag", 0f);
                }
                else
                {
                    StopAudio?.Invoke("Drag");
                    PlayAudio?.Invoke("Pushed", 0f);
                }
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
            if (GrabParticle != null)
            {
                ActiveGrab = Instantiate(GrabParticle);
                ActiveGrab.GetComponent<GrabParticle>().myDaddy = transform;
            }
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
            Destroy(ActiveGrab, .1f);
        }
    }

    public void StartDrop(Transform targetSpot)
    {
        Dropping = true;
        DropTarget = targetSpot;

        EXPLOSION(DropTarget);

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
    public void EXPLOSION(Transform spot)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spot.position, 5f);
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponentInParent<Rigidbody2D>() != null && hit.GetComponentInParent<Rigidbody2D>() != this.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = hit.GetComponentInParent<Rigidbody2D>();



                if (rb != null)
                {
                    Vector2 direction = rb.transform.position - spot.position;

                    rb.AddForce(direction * ExplosionForce);
                    rb.AddForceAtPosition(direction * ExplosionForce, spot.position);
                }
            }
        }
    }

    //public void SetItem(ItemType myItem , Rigidbody2D body, HingeJoint2D hinge)
    //{
    //    itemType = myItem;
    //    joint = hinge;
    //    motor = body;
    //}

    public event Action OnSelect;
    public event Action OnUnselect;

    public ISelectable Select(object source)
    {
        if (Highlight != null)
        {
            Highlight.SetActive(true);
        }
        OnSelect?.Invoke();
        return this;
    }

    public ISelectable Unselect(object source)
    {
        if (Highlight != null)
        {
            Highlight.SetActive(false);
        }
        OnUnselect?.Invoke();
        return this;
    }

    private void OnApplicationPause(bool pause)
    {

    }
}
