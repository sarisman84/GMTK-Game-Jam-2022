using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class _OnButtonHover : MonoBehaviour
{
    public GameObject[] hovers;
    void Start(){
    }
    public void OnHover(){
      hovers[0].SetActive(true);
        hovers[1].SetActive(false);
    }
    public void OnExitHover(){
      hovers[1].SetActive(true);
        hovers[0].SetActive(false); 
    }
    void OnDisable(){
      hovers[1].SetActive(true);
        hovers[0].SetActive(false); 
    }
}
