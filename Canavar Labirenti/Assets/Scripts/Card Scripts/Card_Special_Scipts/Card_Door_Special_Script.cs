using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Door_Special_Script : MonoBehaviour
    {
        public Selection_Point_interact selection_Point_Interact;
        public Card_State_script card_State_script;
        public GameObject CardSpecialObject;
        public float gate_anim_speed;
        // Start is called before the first frame update
        void Start()
            {
                
            }

        // Update is called once per frame


        public void CardSpecialEffect()
            {
                Instantiate(CardSpecialObject, selection_Point_Interact.selectedObjectTransform);
            }

        public void CardOutEffect()
            {
                CardSpecialObject.transform.Translate(CardSpecialObject.transform.up*gate_anim_speed*Time.deltaTime);
                Destroy ( CardSpecialObject, 2f);
            }
    }
