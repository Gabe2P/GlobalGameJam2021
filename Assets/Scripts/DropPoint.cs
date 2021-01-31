using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour
{


    [SerializeField]
    float ExplosionForce = 50f;
    // Start is called before the first frame update
    void Start()
    {
        ItemDropManager.Instance.AddDropPoint(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,1);
        
    }

    public void EXPLOSION()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponentInParent<Rigidbody2D>();
            
            

            if (rb != null)
            {
                Vector2 direction = rb.transform.position - transform.position;

                rb.AddForce(direction * ExplosionForce);
                rb.AddForceAtPosition(direction * ExplosionForce, transform.position);
            }
        }
    }
}
