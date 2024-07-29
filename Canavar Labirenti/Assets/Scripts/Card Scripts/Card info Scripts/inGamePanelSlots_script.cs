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
        card_State_Script.parentAfterDrag = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
