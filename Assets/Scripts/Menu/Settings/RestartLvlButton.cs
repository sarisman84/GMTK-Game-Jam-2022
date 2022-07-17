using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartLvlButton : MonoBehaviour
{
  public void ResetLevel(){
     SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
  }
  public void ToMainMenu(){

  }
  public void Resume(){
    GameObject.Find("p_pausemenumanager").GetComponent<PauseMenuOpener>().CloseMenu();
  }
}
