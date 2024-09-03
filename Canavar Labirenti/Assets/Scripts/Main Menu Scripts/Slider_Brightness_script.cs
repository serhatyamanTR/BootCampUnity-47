using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.Rendering.PostProcessing;
public class Slider_Brightness_script : MonoBehaviour
{
    public Slider slider_Brightness;
    public PostProcessProfile brightness;
    public PostProcessLayer postProcessLayer;
    AutoExposure exposure;
    // Start is called before the first frame update
    void Start()
        {
            brightness.TryGetSettings(out exposure);
        }

    public void AddjustBrightness(float value)
        {
            if (value !=0)
                {
                    exposure.keyValue.value = value;
                }
                else
                    {
                        exposure.keyValue.value = 0.05f;
                    }
        }
}
    