using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public static UIMenu main;
    private void Awake() {
        main = this;
    }
    
    private bool menuActive = false;
    public bool MenuActive { get { return menuActive; } }

    private string mainMenuShow = "MainMenu";
    private string pauseMenuShow = "PauseMenu";
    private string endGameShow = "EndGame";
    private string hide = "Hide";

    [SerializeField]
    private KeyCode OpenPauseMenuKey = KeyCode.Q;

    [SerializeField]
    private UISingleMenu pauseMenu;
    [SerializeField]
    private UISingleMenu mainMenu;
    [SerializeField]
    private UISingleMenu endGameMenu;

    private UISingleMenu activeMenu;

    private void Show(UISingleMenu singleMenu, string description)
    {
        if (menuActive) {
            Debug.Log("Menu was already active!");
            return;
        }
        activeMenu = singleMenu;
        menuActive = true;
        Time.timeScale = 0f;
        singleMenu.gameObject.SetActive(true);
        singleMenu.SetDescription(description);
        singleMenu.Show();
    }

    private void Start() {
        if (Time.timeScale < 1f) {
            Time.timeScale = 1f;
            GameManager.Main.StartGame();
        } else {
            ShowMainMenu();
        }
    }

    public void Hide(UISingleMenu singleMenu) {
        Debug.Log($"hiding {singleMenu}...");
        singleMenu.Hide();
    }

    public void MenuHideFinished(UISingleMenu menu) {
        menu.gameObject.SetActive(false);
        menuActive = false;
        Time.timeScale = 1f;
    }

    public void ShowMainMenu()
    {
        Show(mainMenu, "Use Left & Right arrows or A & D to move. Try to stay on the road! And don't forget to drink!");
    }
    public void ShowPauseMenu()
    {
        Show(pauseMenu, $"Time is frozen.\n{GetDescription()}");
    }
    public void ShowEndMenu(string reason)
    {
        if (menuActive) {
            Hide(pauseMenu);
        }
        Show(endGameMenu, $"{reason}\n{GetDescription()}");
    }

    private string GetDescription() {
        return $"Score: {GameManager.Main.GetScore()}\nYou picked up {GameManager.Main.BeersPickedUp} beers.\nYou walked {MoveAlongRoad.main.GetTravelDistance()} miles.";
    }

    public void Continue() {
        Debug.Log("Continue...");
        Hide(activeMenu);
    }

    public void Restart() {
        GameManager.Main.Restart();
    }

    public void StartGame() {
        Hide(activeMenu);
        GameManager.Main.StartGame();
    }

    void Update() {
        if (!menuActive && Input.GetKeyDown(OpenPauseMenuKey)) {
            ShowPauseMenu();
        }
    }
}
