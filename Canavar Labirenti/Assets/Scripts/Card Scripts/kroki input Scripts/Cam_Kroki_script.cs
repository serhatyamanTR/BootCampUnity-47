using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Kroki_script : MonoBehaviour
{
    public Camera   mainCam;
    //public float OffSet_X;
    //public float OffSet_Y;
    public float Koor_coeff;
    private Vector3 SelectionKoor;
    public GameObject SelectionPoint;
    public Transform KrokiKoorOrijinBelirteci;

    // Start is called before the first frame update
    void Start()
        {
            SelectionKoor = transform.position;
        }

    // Update is called once per frame
    void Update()
        {
            SelectionKoordinatCalculator();

            Vector3 mousePosition = Input.mousePosition;
            if(Physics.Raycast(mainCam.ScreenPointToRay(mousePosition), out RaycastHit  hitInfo))
                {
                    if(hitInfo.collider.CompareTag("kroki"))
                        {
                            Debug.Log("Krokiye Bakılan mouse pozisyonu = " + mousePosition);
                            Debug.Log("Krokiye Bakılan view pozisyonu = " + hitInfo.point);
                            Debug.Log("Krokiye orijin = " + KrokiKoorOrijinBelirteci.position);
                            SelectionKoor = new Vector3 (hitInfo.point.x-KrokiKoorOrijinBelirteci.position.x,0f, hitInfo.point.y-KrokiKoorOrijinBelirteci.position.y)*Koor_coeff;
                            Debug.Log("SelectionKoor = " + SelectionKoor);
                            SelectionPoint.transform.position = SelectionKoor;
                        }
                        

                }
        }

    public  void SelectionKoordinatCalculator()
        {

        }

}

