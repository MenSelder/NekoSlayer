using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class InterfaceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameModeText;
    public TextMeshProUGUI GameModeText => gameModeText;

    [SerializeField] private Button restartButton;

    public static InterfaceUI Instance { get; private set; }

    private void Awake()
    {
        gameModeText.GetComponent<Button>();

        restartButton.onClick.AddListener(() => 
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        });
    }

    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e)
    {
        GameManager gameManager = sender as GameManager;
        gameModeText.text = gameManager.CurrentGameState.ToString();
    }
}
