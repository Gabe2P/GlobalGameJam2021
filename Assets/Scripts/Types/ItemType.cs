//Written By Anthony Eckman 1-29-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemType", menuName = "Types/ItemType")]
public class ItemType : ScriptableObject
{

    public string Name;

    public float Mass;
    public float Drag;

    public Sprite sprite;

}