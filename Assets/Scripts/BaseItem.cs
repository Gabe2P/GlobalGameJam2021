using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreatItem/NewItem")]
public class BaseItem : ScriptableObject
{

    public string Name;

    public float Mass;
    public float Drag;

    public Sprite sprite;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}