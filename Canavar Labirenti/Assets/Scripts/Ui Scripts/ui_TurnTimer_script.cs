using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;

public class ui_timer_script : MonoBehaviour
{
    private UnityEngine.UI.Image    timerCoverImage;
    private UnityEngine.UI.Image    timerBaseImage;

    private DeckManager_script deckManager_Script;
    

    public float turnTimer = 30f;
    private float turnTimerRemain;

    // Start is called before the first frame update
        void Start()
    {
        deckManager_Script = GameObject.Find("DeckManagerPanel").GetComponent<DeckManager_script>();
        timerBaseImage = gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        timerCoverImage = gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        
        turnTimerRemain = 0;
    }

    // Update is called once per frame
    void Update()
        {
            if (deckManager_Script.isTurnPlayer1)
                {
                    if ( turnTimerRemain < turnTimer)
                    {
                        turnTimerRemain += Time.deltaTime;
                        timerCoverImage.fillAmount = turnTimerRemain / turnTimer;
                        timerBaseImage.transform.localScale = Vector3.one*(turnTimerRemain/turnTimer);
                        var tempcolor =timerBaseImage.color;
                        tempcolor.a = turnTimerRemain / turnTimer;
                        timerBaseImage.color = tempcolor;
                    }
                else
                    { 
                        
                        deckManager_Script.isTurnPlayer1 = false;
                    }
                
                }
        }
}
