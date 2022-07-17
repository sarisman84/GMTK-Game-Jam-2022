using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_set_in_game : MonoBehaviour
{
   int res,vsync,quality,effects;
   bool fscreen;
   public GameObject[] effect_lvl;
   void Start(){
    Change();
   }
   public void Change(){
      res     =  PlayerPrefs.GetInt("res");
      vsync   = PlayerPrefs.GetInt("vsync");
      quality =  PlayerPrefs.GetInt("quality");
      effects =  PlayerPrefs.GetInt("effects");
      if(PlayerPrefs.GetInt("fscreen") == 0){
          fscreen = false;
      }else{
         fscreen = true;
      }

      ChangeRez(res,fscreen);
      ChangeQuality(quality);
      ChangeEffects(effects);
      QualitySettings.vSyncCount = vsync;
   }
   void ChangeRez(int x, bool y){
       switch (x)
       {
         case 0:
         Screen.SetResolution(640, 480, fscreen);
         break;
         case 1:
          Screen.SetResolution(1280, 720, fscreen);
         break;
         case 2:
          Screen.SetResolution(1366, 768, fscreen);
         break;
         case 3:
         Screen.SetResolution(1920,1080, fscreen);
         break;
         Screen.SetResolution(Screen.currentResolution.height,Screen.currentResolution.width, fscreen);
         break;

       }
   }
   void ChangeQuality(int x){
           QualitySettings.SetQualityLevel(x, true);
   }
   void ChangeEffects(int x){
          for(int i = 0;i<effect_lvl.Length;i++){
            if(x == i){
                effect_lvl[i].SetActive(true);
            }else{
                effect_lvl[i].SetActive(false);
            }
          }
   }
}
