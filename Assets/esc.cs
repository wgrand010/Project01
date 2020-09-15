using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class esc : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("pressed");
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
