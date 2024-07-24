using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selection_Point_interact : MonoBehaviour
{
    
    
    public String cardSpellTag;
    private Color baseColor; //dokunacağım nesnenin rengini kaydetmek için
    private Vector3 baseScale;
    private Renderer lastSelectedRenderer;
    public Transform selectedObjectTransform;
    public bool isObjectSelected;

    private Cam_Kroki_script cam_Kroki_Script;


    // Start is called before the first frame update
    void Start()
        {
            lastSelectedRenderer = null;
        }

    // Update is called once per frame
    void Update()
        {
            
        }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(cardSpellTag))  //dokunduğun objenin tagini kontrolet
        {
            Renderer otherRenderer = other.GetComponent<Renderer>(); 

            // ilk önce dokunduğun objeye dokuurken yeni bir objeye dokundun mu?
            // Eğer ona dokunurken başka bir obje seçiliyse, onun rengini eski haline getir
            if (lastSelectedRenderer != null && lastSelectedRenderer != otherRenderer)
            {
                lastSelectedRenderer.material.color = baseColor;
                lastSelectedRenderer.transform.localScale = baseScale;
            }

            // Şu anda seçili objeyi güncelle
            lastSelectedRenderer = otherRenderer;
            baseColor = otherRenderer.material.color;
            baseScale = otherRenderer.transform.localScale;

            // Yeni seçilen objenin rengini değiştir
            otherRenderer.material.color = Color.yellow;
            selectedObjectTransform = otherRenderer.transform;
            otherRenderer.transform.localScale *=1.4f;
            isObjectSelected = true;
            
            Debug.Log("Dokunulan obje lokasyonu = " + otherRenderer.transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(cardSpellTag))
        {
            Renderer otherRenderer = other.GetComponent<Renderer>();

            // Eğer çıkılan obje seçili obje ise, rengini eski haline getir
            if (otherRenderer == lastSelectedRenderer)
            {
                otherRenderer.material.color = baseColor;

                otherRenderer.transform.localScale = baseScale;
                
                lastSelectedRenderer = null; // Seçili objeyi temizle

                isObjectSelected = false;

                

            }
        }
    }

}
