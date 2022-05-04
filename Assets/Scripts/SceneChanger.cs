using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject instructionPanel;
   
    public void GotoGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void InstructionPanel()
    {
        instructionPanel.SetActive(true);
    }

    public void BackButtonClick()
    {
        instructionPanel.SetActive(false);
    }
}
