using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Ballista_Special_Script : MonoBehaviour
{
    public Selection_Point_interact selection_Point_Interact;
    public GameObject CardSpecialObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CardSpecialEffect()
        {
            Debug.Log("Balista card özel efekt çalıştı");         
            if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("0 butonuna basıldı");
                    if (selection_Point_Interact.isObjectSelected)
                        {
                            //selection_Point_Interact.selectedObjectTransform.gameObject.SetActive(false);
                            Instantiate(CardSpecialObject, selection_Point_Interact.selectedObjectTransform.position + Vector3.up, selection_Point_Interact.selectedObjectTransform.rotation );
                            Debug.Log("oluşturma kodu çalıştı");
                        }
                }

        }
    void CardOutEffect()
        {
            CardSpecialObject.GetComponent<Renderer>().material.color = Color.red;
            CardSpecialObject.transform.Translate(CardSpecialObject.transform.up*Time.deltaTime);
            Destroy(CardSpecialObject , 2f);
        }
}
