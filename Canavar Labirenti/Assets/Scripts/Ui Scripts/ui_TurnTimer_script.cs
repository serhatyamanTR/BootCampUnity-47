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
    public float turnTimerRemain;

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
                    turnTimerRemain += Time.deltaTime;
                    if ( turnTimerRemain <= turnTimer)
                        {
                            timerCoverImage.fillAmount = turnTimerRemain / turnTimer;
                            timerBaseImage.transform.localScale = Vector3.one*(turnTimerRemain/turnTimer);
                            var tempcolor =timerBaseImage.color;
                            tempcolor.a = turnTimerRemain / turnTimer;
                            timerBaseImage.color = tempcolor;
                        }
                        else
                            {
                                if(turnTimerRemain >= turnTimer && turnTimerRemain <= turnTimer+0.5f )
                                    { 
                                        gameObject.transform.localScale = Vector3.one*(turnTimerRemain-29); /* ax+b = scale,     x=30 = turntimer için 30a+b=1, 31a+b=1.3, a=3/10 b=-8 */
                                    }
                                    else    
                                        {
                                            if(turnTimerRemain>= turnTimer+0.5f && turnTimerRemain < turnTimer+1)
                                                {
                                                    gameObject.transform.localScale = Vector3.one*((-turnTimerRemain)+32); /* ax+b = scale,     x=30,5 = turntimer için 30,5a+b=1,3, 31a+b=1, a=-3/5, b=+196/10 */
                                                }
                                                else
                                                    {
                                                        gameObject.transform.localScale = Vector3.one;
                                                        deckManager_Script.isTurnPlayer1 = false;
                                                    }
                                        }
                        }
                }
        }
}
