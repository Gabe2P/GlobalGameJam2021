using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpot : MonoBehaviour
{
    public event Action<object> OnDeliveryRequested;
    public event Action<object> OnDeliveryCompleted;
    public event Action<Vector2> OnUpdateDeliveryMarker;
    [SerializeField] private GameObject markerPrefab = null;
    [SerializeField] private Canvas point = null;


    [SerializeField]
    public GameObject RequestedItems;

    public bool ActiveDelivery = false;

    [SerializeField]
    float TimeToDeliverItems;

    private float CountdownTimer;

    [SerializeField]
    float AscensionSpeed = .5f;
    [SerializeField]
    float AscensionHeight = 30;

    GameObject DeliveredItem;

    bool FDelivering = false;

    private void MakeMarker()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, this.transform.position - point.transform.position);
        if (hitInfo)
        {
            Debug.LogError(hitInfo.point);
            GameObject clone = Instantiate(markerPrefab, hitInfo.point, Quaternion.identity, point.transform);
            DeliveryRequestMarker marker = clone.GetComponent<DeliveryRequestMarker>();
            if (marker != null)
            {
                marker.deliverySpot = this;
            }
        }
    }

    private void UpdateMarker()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, this.transform.position - point.transform.position);
        if (hitInfo)
        {
            OnUpdateDeliveryMarker?.Invoke(hitInfo.point);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //RequestManager.Instance.AvailableDeliveryPoints.Add(this);
        MakeMarker();
    }

    // Update is called once per frame
    void Update()
    {
        if(FDelivering)
        {
            DeliverItem();
        }
        UpdateMarker();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.GetComponentInParent<ItemController>() != null && RequestedItems !=null && ActiveDelivery)
        {

            
            CheckItems(collision.transform.parent.gameObject);
        }
    }
   

    private void CheckItems(GameObject item)
    {

        if (RequestedItems.GetComponent<ItemController>().Item == item.GetComponent<ItemController>().Item)
        {
            Debug.Log("Gimme Banana");
            RequestedItems = null;
            DeliveredItem = item;
            DeliveredItem.GetComponent<ItemController>().Release(new Vector2(0, 0));
            DeliveredItem.GetComponent<ItemController>().enabled = false;
            DeliveredItem.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            FDelivering = true;


        }


    }

    public void DeliverItem()
    {
        DeliveredItem.transform.position = new Vector3(DeliveredItem.transform.position.x, DeliveredItem.transform.position.y + AscensionSpeed * Time.deltaTime, DeliveredItem.transform.position.z);

        if (DeliveredItem.transform.position.y >= transform.position.y + AscensionHeight)
        {
            FDelivering = false;
            RequestedItems= null;
            Destroy(DeliveredItem);
            CompleteDelivery();
        }
        if (DeliveredItem != null)
        {
            Destroy(DeliveredItem, 10f);
        }

    }


    public void StartDelivery(GameObject items)
    {
        ActiveDelivery = true;
        OnDeliveryRequested?.Invoke(this.gameObject);
        RequestedItems = items;

    }

    private void CompleteDelivery()
    {
        RequestManager.Instance.CompleteDelivery(this);
        ActiveDelivery = false;
        OnDeliveryCompleted?.Invoke(this.gameObject);
    }

        
}
