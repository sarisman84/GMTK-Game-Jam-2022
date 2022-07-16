using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    public GameObject next_panel;
    public Animator anim;
    void Start(){
          anim.Play("inverse");
    }
    public void ButtonPress(){
        anim.Play("normal");
        StartCoroutine("WaitUntilNext",.45f);
   }
   void OnEnable(){
         anim.Play("inverse");
   }
   IEnumerator WaitUntilNext(float time){
      yield return new WaitForSeconds(time);
      gameObject.SetActive(false);
      next_panel.SetActive(true);
   }
}
