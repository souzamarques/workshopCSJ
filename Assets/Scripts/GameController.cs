using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour{
    // Start is called before the first frame update
    
    public GameObject gameOverPanel;

    public void ShowGameOver(){
        gameOverPanel.SetActive(true);
    }
    
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
