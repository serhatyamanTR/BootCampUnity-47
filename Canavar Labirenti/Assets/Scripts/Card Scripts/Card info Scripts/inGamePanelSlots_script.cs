    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class inGamePanelSlots_script : MonoBehaviour,IDropHandler
    {

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped  = eventData.pointerDrag;
            Card_State_script card_State_Script = dropped.GetComponent<Card_State_script>();
            if (transform.childCount ==0)
                {
                    

                    card_State_Script.parentAfterDrag = transform;
                    card_State_Script.SetState(Card_State.inGame);
                    card_State_Script.SelectedinGamePanelSlot = gameObject.transform.GetSiblingIndex();
                    card_State_Script.isSlotSelected = true;
                }
                else
                {
                    Debug.Log("Slot Dolu");
                    card_State_Script.isSlotSelected = false;

                }
                    
        }

        // Start is called before the first fram    e update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
