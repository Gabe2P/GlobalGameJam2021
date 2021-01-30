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





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<ItemController>() != null && ActiveDelivery)
        {
            Debug.Log("Gimme Banana");
            CheckItems(collision.GetComponent<ItemController>().Item);
        }
    }

    private void CheckItems(ItemType item)
    {
        foreach(GameObject requestedItem in RequestedItems)
        {
            if(requestedItem.GetComponent<ItemType>() == item)
            {
                RequestedItems.Remove(requestedItem);
                Destroy(requestedItem);

                break;
            }
        } 
        if(RequestedItems.Count <= 0)
        {
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
        RequestManager.Instance.CompleteDelivery(this);
        ActiveDelivery = false;
    }

        
}
