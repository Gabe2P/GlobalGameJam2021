using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{

    public static RequestManager _Instance;

    public static RequestManager Instance { get {return _Instance;} }
 



    [SerializeField]
    public List<DeliverySpot> AvailableDeliveryPoints = new List<DeliverySpot>();


    [SerializeField]
    private List<GameObject> ItemsSpawned = new List<GameObject>();

    [SerializeField]
    //private List<List<GameObject>> Deliveries = new List<List<GameObject>>();

    
    public Dictionary<GameObject, List<GameObject>> Delivieries = new Dictionary<GameObject, List<GameObject>>();
    [SerializeField]
    private int ItemsPerDelivery = 1;

    [SerializeField]
    private float TimeBetweenDeliveries = 60f;

    private float TimeUntilNextDelivery;

    [SerializeField]
    float RampUpInterval = 120;

    [SerializeField]
    float Timer = 0;

    
    


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
        Timer += Time.deltaTime;
        RampUp();
    }

   private void RampUp()
    {
        if( Timer >= RampUpInterval )
        {
            ItemsPerDelivery++;
            Timer = 0;
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
        
        
        List<int> checkedDeliverySpots = new List<int>();
        GameObject Deliverables;
        for(int i = 0; i < ItemsPerDelivery; i++)
        {
            DeliverySpot DropSpot = AvailableDeliveryPoints[Random.Range(0, AvailableDeliveryPoints.Count - 1)];
            AvailableDeliveryPoints.Remove(DropSpot);
            int index = Random.Range(0, ItemsSpawned.Count - 1);
            Deliverables=ItemsSpawned[index];
            RemoveItemFromList(ItemsSpawned[index]);

            DropSpot.StartDelivery(Deliverables);
        }

        

        TimeUntilNextDelivery = TimeBetweenDeliveries + Time.time;

    }


    public void CompleteDelivery(DeliverySpot spot)
    {
        AvailableDeliveryPoints.Add(spot);
    }





}
