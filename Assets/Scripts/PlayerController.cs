using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float MovementSpeed = 10f;
    // Start is called before the first frame update


    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void MovePlayer(Vector2 movementDirection)
    {

        transform.position += new Vector3(MovementSpeed * movementDirection.x * Time.deltaTime, MovementSpeed * movementDirection.y * Time.deltaTime, transform.position.z);

    }
}
