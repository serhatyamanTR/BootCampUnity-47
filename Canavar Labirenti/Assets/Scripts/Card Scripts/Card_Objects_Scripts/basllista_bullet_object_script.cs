using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basllista_bullet_object_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("monster"))
                {
                    Debug.Log("Canavar Vuruldu");
    
                    Destroy(collision.gameObject, 1f);
                }
        }
}
