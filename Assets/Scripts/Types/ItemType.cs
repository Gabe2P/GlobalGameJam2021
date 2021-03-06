//Written By Anthony Eckman 1-29-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemType", menuName = "Types/ItemType")]
public class ItemType : ScriptableObject
{

    public string Name;
    public int ID;

    public float Mass;
    public float Drag;


    public int Points;

    public Sprite sprite;
    public GameObject prefab;

}