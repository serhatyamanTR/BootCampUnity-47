using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selection_Poi : MonoBehaviour
{
    private Color baseColor; //dokunacağım nesnenin rengini kaydetmek için
    // Start is called before the first frame update
    void Start()
        {

        }

    // Update is called once per frame
    void Update()
        {

        }

    void OnCollisionStay(Collision collision)
        {
            // Seçim noktamın dokunduğu şeyin renderer bileşenini al
            Renderer otherRenderer = collision.gameObject.GetComponent<Renderer>();
            baseColor = otherRenderer.material.color;


            if (collision.collider.CompareTag("wall"))
                {
                    
                    // Eğer Renderer bileşeni varsa, objenin rengini sarı yapar
                    if (true)
                        {
                            otherRenderer.material.color = Color.yellow;
                        }
                }
        }
    void    OnCollisionExit(Collision collision)
        {
            Renderer otherRenderer = collision.gameObject.GetComponent<Renderer>();
            otherRenderer.material.color = baseColor;
        }

}
