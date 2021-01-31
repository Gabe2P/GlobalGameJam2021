using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryCanvas : MonoBehaviour
{
    [SerializeField]

    DeliverySpot mySpot;


    Image spriteImage;

    



    // Start is called before the first frame update
    void Start()
    {
        spriteImage = GetComponentInChildren<Image>();
        spriteImage.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if(mySpot.RequestedItems != null)
        {
            spriteImage.color = Color.black;
            spriteImage.sprite = mySpot.RequestedItems.GetComponent<ItemController>().Item.sprite;
        }
        else
        {
            spriteImage.color = Color.clear;
        }
       
    }
}
