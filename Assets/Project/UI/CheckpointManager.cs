using UnityEngine;
using TMPro;
public class CheckpointManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject emailPanel;
    public GameObject passwordPanel;
    public GameObject errorPanel;
    public GameObject gameplayPanel;
    public GameObject loadingPanel;

    public TMP_InputField emailInput;
    public TMP_InputField codeInput;
    public UserData userData;
    void Start()
    {
        ShowStartPanel();
    }

    private void CloseAll()
    { 
        startPanel.SetActive(false);
        emailPanel.SetActive(false);
        passwordPanel.SetActive(false);
        errorPanel.SetActive(false);
        gameplayPanel.SetActive(false);
    }

    public void ShowStartPanel()
    {
        CloseAll();
        startPanel.SetActive(true);
    }
    public void ShowEmailPanel()
    {
        CloseAll();
        emailPanel.SetActive(true);
    }
    public void ShowPasswordPanel()
    {
        CloseAll();
        userData.Email = emailInput.text;
        passwordPanel.SetActive(true);
    }
    public void ShowLoadingPanel()
    {
        CloseAll();
        loadingPanel.SetActive(true);
    }
    public void SaveEmail()
    {
        userData.Email = emailInput.text;
    }
    public void SaveCode()
    {
        userData.Code = codeInput.text;
    }
    public void ShowErrorPanel()
    {
        CloseAll();
        errorPanel.SetActive(true);
    }
    public void ShowGameplayPanel()
    {
        CloseAll();
        gameplayPanel.SetActive(true);
    }
}
