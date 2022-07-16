using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomAudioSlider : MonoBehaviour
{
     public Image slider_fill;
     public float max_value,min_value;
     private float current_value;
     private float value; //Slider value 
     public string save_name;
    void Start()
    {
       if(!PlayerPrefs.HasKey(save_name)){
        current_value = max_value/2;}else{
            current_value = PlayerPrefs.GetFloat(save_name);
        }
        
    }

    void Update()
    {
        value = current_value/max_value;
        slider_fill.fillAmount = value;
    }
    public void Buttons(int x){
        current_value += x*(max_value/10);
        current_value = Mathf.Clamp(current_value,min_value,max_value);
        PlayerPrefs.SetFloat(save_name,current_value); //saves the sliders value
    }
}
