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
                RequestedItems.Remove(requestedItem);
                Destroy(item);

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
