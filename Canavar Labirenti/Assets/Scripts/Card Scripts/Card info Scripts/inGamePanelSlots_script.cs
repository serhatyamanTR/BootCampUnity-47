    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class inGamePanelSlots_script : MonoBehaviour,IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount ==0)
                {
                    GameObject dropped  = eventData.pointerDrag;
                    Card_State_script card_State_Script = dropped.GetComponent<Card_State_script>();
                    card_State_Script.parentAfterDrag = transform;
                    dropped.GetComponent<Card_State_script>().SetState(Card_State.inGame);
                }
                else
                {
                    Debug.Log("Slot Dolu");
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
