using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Door_Special_Script : MonoBehaviour
    {
        public Selection_Point_interact selection_Point_Interact;
        public SelectionAction selectionAction;
        public Card_State_script card_State_script;
        public GameObject CardSpecialObject;

        // Start is called before the first frame update
        void Start()
            {
                selectionAction = GameObject.Find("Kroki").GetComponent<SelectionAction>();
            }

        // Update is called once per frame


        public void CardSpecialEffect()
            {   
                Debug.Log("Kapı Special effect çalıştı.");
                selectionAction.PrefabToInstantiate = CardSpecialObject;
            }

        public void CardOutEffect()
            {
                Debug.Log("Kapı Out effect çalıştı.");
                CardSpecialObject.transform.Translate(CardSpecialObject.transform.up*Time.deltaTime);
                Destroy ( CardSpecialObject, 2f);
            }
    }
