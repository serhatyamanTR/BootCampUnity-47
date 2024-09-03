using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options_Control_script : MonoBehaviour
{
    // Slider referansı
    public Slider soundSlider;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    private void Start()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            
            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
                {
                    string option = resolutions[i].width + "x" + resolutions[i].height;
                    options.Add(option);
                    if  (   resolutions[i].width == Screen.currentResolution.width
                            && 
                            resolutions[i].height == Screen.currentResolution.height
                        )
                        {
                            currentResolutionIndex = i;
                        }
                }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            // Slider'ın değerini mevcut ses seviyesi ile başlat
            soundSlider.value = AudioListener.volume;
    
            // Slider'ın değer değişim olayına dinleyici ekle
            soundSlider.onValueChanged.AddListener(SetVolume);
        }

    public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); 
        }
    public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
    // Slider değeri değiştiğinde çağrılacak metod
    public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }

    public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
}
