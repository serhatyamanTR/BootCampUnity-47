using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Barricade_Special_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public Selection_Point_interact selection_Point_Interact;
    public GameObject CardSpecialObject;
    // Start is called before the first frame update
    public SelectionAction SelectionAction;
    void Start()
    {
        selection_Point_Interact = GameObject.Find("Selection_Point").GetComponent<Selection_Point_interact>();
        SelectionAction=GameObject.Find("Kroki").GetComponent<SelectionAction>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CardSpecialEffect()
    {
        Debug.Log("....card özel efekt çalýþtý");
        //selection_Point_Interact.selectedObjectTransform.gameObject.SetActive(false);
        //Instantiate(CardSpecialObject, selection_Point_Interact.selectedObjectTransform.position + Vector3.up, selection_Point_Interact.selectedObjectTransform.rotation);
        SelectionAction.PrefabToInstantiate=CardSpecialObject;
        Debug.Log("oluþturma kodu çalýþtý");



    }
    public void CardOutEffect()
    {
        CardSpecialObject.GetComponent<Renderer>().material.color = Color.red;
        CardSpecialObject.transform.Translate(CardSpecialObject.transform.up * Time.deltaTime);
        Destroy(CardSpecialObject, 2f);
    }
}
