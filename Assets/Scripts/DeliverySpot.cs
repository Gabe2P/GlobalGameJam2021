using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpot : MonoBehaviour
{
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





    // Start is called before the first frame update
    void Start()
    {
        RequestManager.Instance.AvailableDeliveryPoints.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(FDelivering)
        {
            DeliverItem();
        }
        
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
        RequestedItems = items;

    }

    private void CompleteDelivery()
    {
        
            RequestManager.Instance.CompleteDelivery(this);
            ActiveDelivery = false;
        
    }

        
}
