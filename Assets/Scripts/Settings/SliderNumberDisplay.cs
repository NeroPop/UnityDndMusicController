using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SliderNumberDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text DisplayNumberTxt;

    private Slider Slider;

    private void Start()
    {
        //Gets the slider component
        Slider = GetComponent<Slider>();

        //Sets the text to display the current slider value
        float value = Slider.value;
        DisplayNumberTxt.text = (value / 10).ToString("F1");
    }

    public void OnSliderValueChanged()
    {
        //Sets the text to display the current slider value
        float value = Slider.value; 
        DisplayNumberTxt.text = (value / 10).ToString("F1");
    }
}
