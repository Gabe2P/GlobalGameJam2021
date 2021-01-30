using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    private RequestManager myRequestManger;
    private static ItemDropManager _Instance;

    public static ItemDropManager Instance { get { return _Instance; } }

    [SerializeField]
    private List<Transform> dropPoints = new List<Transform> { };

    [SerializeField]
    private List<GameObject> ItemsToBeDropped = new List<GameObject>();

    [SerializeField]
    float TimeBetweenDrops = 30f;



    [SerializeField]
    int DropAmmount = 1;

    int DropIncreaseCounter = 0;

    public float TimeUntilNextDrop;

    


    private void Awake()
    {
        if(_Instance !=null && _Instance != this)
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
        myRequestManger = RequestManager.Instance;
        TimeUntilNextDrop = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeUntilNextDrop <= Time.time)
        {
            DropTime();
        }
    }

    private void DropItem(Transform aPosition,GameObject item)
    {
        //GameObject temp = new GameObject(item.name);
        //temp.AddComponent(typeof(SpriteRenderer));
        //temp.GetComponent<SpriteRenderer>().sprite = item.sprite;
        ////temp.GetComponent<SpriteRenderer>().color = Color.red;
        //temp.AddComponent<PolygonCollider2D>();
        //temp.AddComponent<Rigidbody2D>();
        //temp.GetComponent<Rigidbody2D>().gravityScale = 0;
        //temp.GetComponent<Rigidbody2D>().drag = item.Drag;
        //temp.GetComponent<Rigidbody2D>().angularDrag = item.Drag;
        //temp.AddComponent<HingeJoint2D>();
        //temp.AddComponent<ItemController>();
        //temp.GetComponent<ItemController>().SetItem(item, temp.GetComponent<Rigidbody2D>(), temp.GetComponent<HingeJoint2D>());

        GameObject temp = Instantiate(item);
        

        temp.transform.position = aPosition.position;
        temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + 20, transform.position.z);
        temp.GetComponent<ItemController>().StartDrop(aPosition);

        myRequestManger.AddItemToRequestManager(temp);
        
    }

    private void DropTime()
    {
        DropIncreaseCounter++;
        
        for(int i = 0; i < DropAmmount ; i ++)
        {
            
            DropItem(dropPoints[Random.Range(0, dropPoints.Count - 1)], ItemsToBeDropped[Random.Range(0, ItemsToBeDropped.Count)]);
        }

        if(DropIncreaseCounter % 5 == 0)
        {
            DropAmmount++;
        }
        TimeUntilNextDrop = Time.time + TimeBetweenDrops;
    }

     






}
