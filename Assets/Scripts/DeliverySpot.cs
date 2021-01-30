using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpot : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> RequestedItems = new List<GameObject>();



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
        
        if(collision.GetComponentInParent<ItemController>() != null)
        {

            
            CheckItems(collision.transform.parent.gameObject);
        }
    }
   

    private void CheckItems(GameObject item)
    {
        foreach(GameObject requestedItem in RequestedItems)
        {
            if(requestedItem.GetComponent<ItemController>().Item == item.GetComponent<ItemController>().Item)
            {
                Debug.Log("Gimme Banana");
                //RequestedItems.Remove(requestedItem);
                DeliveredItem = item;
                DeliveredItem.GetComponent<ItemController>().Release(new Vector2(0,0));
                DeliveredItem.GetComponent<ItemController>().enabled = false;
                DeliveredItem.GetComponentInChildren<PolygonCollider2D>().enabled = false;
                FDelivering = true;

                break;
            }
        } 
        
    }

    public void DeliverItem()
    {
        DeliveredItem.transform.position = new Vector3(DeliveredItem.transform.position.x, DeliveredItem.transform.position.y + AscensionSpeed * Time.deltaTime, DeliveredItem.transform.position.z);

        if (DeliveredItem.transform.position.y >= transform.position.y + AscensionHeight)
        {
            FDelivering = false;
            RequestedItems.Remove(DeliveredItem);
            Destroy(DeliveredItem);
            CompleteDelivery();
        }
    }


    public void StartDelivery(List<GameObject> items)
    {
        ActiveDelivery = true;
        RequestedItems = items;

    }

    private void CompleteDelivery()
    {
        if (RequestedItems.Count <= 0)
        {
            RequestManager.Instance.CompleteDelivery(this);
            ActiveDelivery = false;
        }
    }

        
}
