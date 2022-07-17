using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonData : MonoBehaviour
{
   public Die_Mover Die_Mover;
   public void SetNumber(int nr){
        Die_Mover.Move(nr);
   }
    
}
