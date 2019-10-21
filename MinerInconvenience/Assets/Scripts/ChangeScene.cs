using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ToGameWon()
    {
        SceneManager.LoadScene("GameWon");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }

    public void CloseApp()
    {
        Application.Quit();
    }
    
    public void ResumeGame()
    {
        GameObject.Find("PauseMenu").SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToInstructions()
    {
        gameObject.SetActive(false);
        if (canvas.gameObject.activeInHierarchy == false)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void BackToPause()
    {
        gameObject.SetActive(false);
        if (canvas.gameObject.activeInHierarchy == false)
        {
            canvas.gameObject.SetActive(true);
        }
    }
}
