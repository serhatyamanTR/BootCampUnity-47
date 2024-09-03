using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_func_manager_script : MonoBehaviour
{
    public PlayerCountDropdown playerCountDropdown;
    private Scene _scene;
    public GameObject OptionsPanel;

    //0. sahne Main Meni
    //1. sahne Tutoriel
    //2-6 arası 1 kişilik
    //7-11 arası 2 kişilik
    //12-16 arası 3 kişilik
    //17-21 arası 4 kişilik



    private void Awake()
        {
            _scene = SceneManager.GetActiveScene();//caching
        }

    public void StartTutoriel()
        {
            SceneManager.LoadScene(1);
        }

    public void StartLevel()
        {
            int sceenIndexToStart = Random.Range(playerCountDropdown.PlayerCount*5 + 2 , playerCountDropdown.PlayerCount*5 + 7);

            SceneManager.LoadScene(sceenIndexToStart);

            Debug.Log("Star için Yüklenen sahne indexi = " + sceenIndexToStart);
                        
        }
    public void OptionsButton()
        {
            if (OptionsPanel != null)
                {
                    if(OptionsPanel.activeSelf)
                        {
                            OptionsPanel.SetActive(false);
                        }
                        else
                            {
                                OptionsPanel.SetActive(true);
                            }
                }
                else
                    {
                        Debug.LogError("Options Panel atanmamış");                    
                    }
        }


    public void exitButton()
    {
        Application.Quit();
    }
}
