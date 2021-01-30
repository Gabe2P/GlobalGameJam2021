using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{

    private static ItemDropManager _Instance;

    public static ItemDropManager Instance { get { return _Instance; } }

    [SerializeField]
    private List<Transform> dropPoints = new List<Transform> { };

    [SerializeField]
    private List<ItemType> ItemsToBeDropped = new List<ItemType>();

    [SerializeField]
    float TimeBetweenDrops = 30f;



    [SerializeField]
    int DropAmmount = 1;

    int DropIncreaseCounter = 0;

    public float TimeUntilNextDrop;

    bool flag = false;


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

    private void DropItem(Transform aPosition,ItemType item)
    {
        GameObject temp = new GameObject(item.name);
        temp.AddComponent(typeof(SpriteRenderer));
        temp.GetComponent<SpriteRenderer>().sprite = item.sprite;
        //temp.GetComponent<SpriteRenderer>().color = Color.red;
        temp.AddComponent<PolygonCollider2D>();
        temp.AddComponent<Rigidbody2D>();
        temp.GetComponent<Rigidbody2D>().gravityScale = 0;

        temp.transform.position = aPosition.position;
        temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, transform.position.z);
        flag = true;
    }

    private void DropTime()
    {
        DropIncreaseCounter++;
        
        for(int i = 0; i < DropAmmount ; i ++)
        {
            Debug.Log("pizza");
            DropItem(dropPoints[Random.Range(0, dropPoints.Count - 1)], ItemsToBeDropped[Random.Range(0, ItemsToBeDropped.Count)]);
        }

        if(DropIncreaseCounter % 5 == 0)
        {
            DropAmmount++;
        }
        TimeUntilNextDrop = Time.time + TimeBetweenDrops;
    }






}
