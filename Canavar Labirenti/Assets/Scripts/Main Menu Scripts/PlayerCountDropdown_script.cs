using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCountDropdown : MonoBehaviour
{
    public int PlayerCount;

    public void PlayerCountSelectionFunc(int Index)
        { 
            switch(Index)
                {
                    case 0:
                        PlayerCount = 2; 
                        break;
                    case 1:
                        PlayerCount = 3; 
                        break;
                    case 2:
                        PlayerCount = 4; 
                        break;
                }
        }

}
