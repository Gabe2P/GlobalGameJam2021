using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpot : MonoBehaviour
{
    [SerializeField]
    public GameObject RequestedItems;

    public bool ActiveDelivery = false;

    [SerializeField]
    float TimeToDeliverItems =60f;

    [SerializeField]
    private float TimeToDeliveryEnd;
    [SerializeField]
    private float CountdownTimer;

    [SerializeField]
    float AscensionSpeed = .5f;
    [SerializeField]
    float AscensionHeight = 30;



    GameObject DeliveredItem;

    [SerializeField]
    GameObject SpotLight;

    [SerializeField]
    GameObject Hand;
    GameObject ActiveHand;



    bool FDelivering = false;
    bool HandMoving = false;

    Collider2D[] itemSpotCheck;


    // Start is called before the first frame update
    void Start()
    {
        RequestManager.Instance.AvailableDeliveryPoints.Add(this);
        SpotLight.SetActive(false);
        TimeToDeliveryEnd = Time.time + TimeToDeliverItems;
    }

    // Update is called once per frame
    void Update()
    {
        if(HandMoving)
        {
            MoveHand();
        }
        if(FDelivering)
        {
            DeliverItem();
        }
        

        CountdownTimer = Time.time;

        if(CountdownTimer >= TimeToDeliveryEnd)
        {
            itemSpotCheck = null;
            Debug.Log("Times Up");
            itemSpotCheck = Physics2D.OverlapCircleAll(transform.position, 1);
            if (itemSpotCheck.Length > 0) 
            {

                if (itemSpotCheck[0].GetComponentInParent<ItemController>() != null)
                {
                    Debug.Log("item in spotlight");
                    WrongItem(itemSpotCheck[0].transform.parent.gameObject);
                    itemSpotCheck = null;
                }
                else
                {
                    FailedDelivery();
                    itemSpotCheck = null;
                }
            }
            else
            {
                FailedDelivery();
                itemSpotCheck = null;
            }
        }
        
        
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.GetComponentInParent<ItemController>() != null && RequestedItems !=null && ActiveDelivery)
        {

            
            RightDelivery(collision.transform.parent.gameObject);
        }

        if (collision.GetComponentInParent<ItemController>() != null && ActiveDelivery && CountdownTimer >= TimeToDeliveryEnd)
        {
            WrongItem(collision.transform.parent.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<ItemController>() != null && ActiveDelivery && CountdownTimer >= TimeToDeliveryEnd)
        {
            WrongItem(collision.transform.parent.gameObject);
        }
        //if(collision == null && CountdownTimer >= TimeToDeliveryEnd)
        //{
        //    FailedDelivery();
        //}    
    }

    private void RightDelivery(GameObject item)
    {
        CheckItems(item);
    }

    private void FailedDelivery()
    {

        FDelivering = false;
        RequestedItems = null;
        SpotLight.SetActive(false);
        CompleteDelivery();
    }

    private void WrongItem(GameObject item)
    {
        if (item.GetComponent<ItemController>() != null)
        {
            Debug.Log("Gimme Banana");
            RequestedItems = null;
            DeliveredItem = item;
            DeliveredItem.GetComponent<ItemController>().Release(new Vector2(0, 0));
            DeliveredItem.GetComponent<ItemController>().enabled = false;
            DeliveredItem.GetComponentInChildren<PolygonCollider2D>().enabled = false;
            HandMoving = true;

            if (ActiveHand == null)
            {
                ActiveHand = Instantiate(Hand);
                ActiveHand.transform.position = new Vector3(DeliveredItem.transform.position.x, DeliveredItem.transform.position.y + AscensionHeight, DeliveredItem.transform.position.z - 1);
            }




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
            HandMoving = true;

            if (ActiveHand == null)
            {
                ActiveHand = Instantiate(Hand);
                ActiveHand.transform.position = new Vector3(DeliveredItem.transform.position.x, DeliveredItem.transform.position.y + AscensionHeight, DeliveredItem.transform.position.z -1);
            }

            


        }


    }

    private void MoveHand()
    {
        

        ActiveHand.transform.position -= new Vector3(0, AscensionSpeed * Time.deltaTime, 0);

        if(ActiveHand.transform.position.y <= DeliveredItem.transform.position.y)
        {
            DeliveredItem.transform.parent = ActiveHand.transform;
            //DeliveredItem.transform.localPosition = new Vector3(0, 0, 1);
            HandMoving = false;
            FDelivering = true;
            ActiveHand.GetComponentInChildren<Animator>().SetTrigger("CloseHand");
        }

    }

    public void DeliverItem()
    {
        ActiveHand.transform.position += new Vector3(0, AscensionSpeed * Time.deltaTime, 0);

        if (ActiveHand.transform.position.y >= transform.position.y + AscensionHeight)
        {
            FDelivering = false;
            RequestedItems= null;
            Destroy(DeliveredItem);
            Destroy(ActiveHand);
            SpotLight.SetActive(false);

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
        SpotLight.SetActive(true);

    }

    private void CompleteDelivery()
    {
            TimeToDeliveryEnd = Time.time + TimeToDeliverItems;
            RequestManager.Instance.CompleteDelivery(this);
            ActiveDelivery = false;
        
    }

        
}
