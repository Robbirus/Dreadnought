using UnityEngine;

public class MenuPause : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;

    // Update is called once per frame
    void Update()
    {
    
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnMenu()
    {
        GetComponent<LoadingController>().ApplyMenu();
    }

}
