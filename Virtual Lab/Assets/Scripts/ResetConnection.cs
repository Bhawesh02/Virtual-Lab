
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ResetConnection : MonoBehaviour
{

    private Button resetButton;


    private void Awake()
    {
        resetButton = GetComponent<Button>();
    }
    private void Start()
    {
        resetButton.onClick.AddListener(resetConnection);
    }



    private void resetConnection()
    {
        SceneManager.LoadScene(0);
    }
}
