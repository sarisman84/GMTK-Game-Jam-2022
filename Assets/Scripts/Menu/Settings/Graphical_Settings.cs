using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Graphical_Settings : MonoBehaviour
{
    int resolution_, vsync,Fullscreen_,quality_,effects_;
    public TextMeshProUGUI[] content;
    public string[] res_names,ref_names,fullS_names,qua_names,eff_names; //Gives a specific name to the content | ex: res_names = 35%,50%,80%,100% or qua_names = low,med,high;
    void Start()
    {
        resolution_ = PlayerPrefs.GetInt("res");
        vsync = PlayerPrefs.GetInt("vsync");
        Fullscreen_ = PlayerPrefs.GetInt("fscreen");
        quality_ = PlayerPrefs.GetInt("quality");
        effects_ = PlayerPrefs.GetInt("effects");
        content[0].text = res_names[resolution_];
        content[1].text = ref_names[vsync];
        content[2].text = fullS_names[Fullscreen_];
        content[3].text = qua_names[quality_];
        content[4].text = eff_names[effects_];
    }
    void Change_Settings(){
          PlayerPrefs.SetInt("res",resolution_);
          PlayerPrefs.SetInt("vsync",vsync);
          PlayerPrefs.SetInt("fscreen",Fullscreen_);
          PlayerPrefs.SetInt("quality",quality_);
          PlayerPrefs.SetInt("effects",effects_);
          GameObject.Find("p_GraphicsManager").GetComponent<Graphics_set_in_game>().Change();
          
    }
    public void ChangeResolution(int x){
             resolution_ += x;
             resolution_ = Mathf.Clamp(resolution_,0,4);
             content[0].text = res_names[resolution_];
             Change_Settings();

    }
    public void ChangeVsync(int x){
            vsync += x;
            vsync  =  Mathf.Clamp(vsync,0,1);
             content[1].text = ref_names[vsync];
             Change_Settings();
    }
    public void ChangeFScreen(int x){
            Fullscreen_ += x;
            Fullscreen_ = Mathf.Clamp(Fullscreen_,0,1);
             content[2].text = fullS_names[Fullscreen_];
             Change_Settings();
    }
    public void ChangeQuality(int x){
         quality_ += x;
         quality_ = Mathf.Clamp(quality_,0,2);
          content[3].text = qua_names[quality_];
             Change_Settings();
    }
    public void ChangeEffects(int x){
         effects_ += x;
         effects_ = Mathf.Clamp(effects_,0,2);
          content[4].text = eff_names[effects_];
             Change_Settings();
    }
}
