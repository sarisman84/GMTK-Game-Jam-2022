using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_Mover : MonoBehaviour
{
    public GameObject[] die_pos;

    public void Move(int pos_nr)
    {
        for(int i = 0;i < die_pos.Length;i++){
            if(pos_nr == i){
               die_pos[i].SetActive(true);
            }else{
                die_pos[i].SetActive(false);
            }
        }
    }
}
