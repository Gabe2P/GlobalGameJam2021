using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{

    public static RequestManager _Instance;

    public static RequestManager Instance { get {return _Instance;} }
 



    [SerializeField]
    private List<DeliverySpot> AvailableDeliveryPoints = new List<DeliverySpot>();


    [SerializeField]
    private List<GameObject> ItemsSpawned = new List<GameObject>();

    [SerializeField]
    //private List<List<GameObject>> Deliveries = new List<List<GameObject>>();

    
    public Dictionary<GameObject, List<GameObject>> Delivieries = new Dictionary<GameObject, List<GameObject>>();
    [SerializeField]
    private int ItemsPerDelivery = 1;

    [SerializeField]
    private float TimeBetweenDeliveries = 10f;

    private float TimeUntilNextDelivery;

    

    
    


    private void Awake()
    {
        if(_Instance != null  && _Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        TimeUntilNextDelivery = Time.time + TimeBetweenDeliveries;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeUntilNextDelivery <= Time.time && ItemsSpawned.Count >= 0 && AvailableDeliveryPoints.Count > 0)
        {
            StartDelivery();
        }
    }


    public void AddItemToRequestManager(GameObject item)
    {
        ItemsSpawned.Add(item);
    }

    public void RemoveItemFromList(GameObject item)
    {
        ItemsSpawned.Remove(item);
    }


    private void StartDelivery()
    {
        List<GameObject> Deliverables = new List<GameObject>();
        DeliverySpot DropSpot = AvailableDeliveryPoints[Random.Range(0,AvailableDeliveryPoints.Count -1)];
        AvailableDeliveryPoints.Remove(DropSpot);
        List<int> checkedDeliverySpots = new List<int>();
        
        for(int i = 0; i < ItemsPerDelivery; i++)
        {
            int index = Random.Range(0, ItemsSpawned.Count - 1);
            Deliverables.Add(ItemsSpawned[index]);
            RemoveItemFromList(ItemsSpawned[index]);

        }

        DropSpot.StartDelivery(Deliverables);

        TimeUntilNextDelivery = TimeBetweenDeliveries + Time.time;

    }


    public void CompleteDelivery(DeliverySpot spot)
    {
        AvailableDeliveryPoints.Add(spot);
    }





}
