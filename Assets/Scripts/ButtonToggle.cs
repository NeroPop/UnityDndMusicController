using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField]
    private Sprite ButtonDefaultSprite;

    [SerializeField]
    private Sprite ButtonSelectedSprite;

    private bool Pressed;


    //Changes the button appearence whether it's selected or not.
    public void ToggleButton()
    {
        if (!Pressed)
        {
            gameObject.GetComponent<Image>().sprite = ButtonSelectedSprite;
            Pressed = true;
        }
        else if (Pressed)
        {
            gameObject.GetComponent<Image>().sprite = ButtonDefaultSprite;
            Pressed = false;
        }
    }
}
