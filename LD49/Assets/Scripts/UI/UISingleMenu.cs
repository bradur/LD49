using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UISingleMenu : MonoBehaviour
{
    [SerializeField]
    private Text txtDescription;
    [SerializeField]
    private Text txtTitle;
    [SerializeField]
    private Button firstButton;
    [SerializeField]
    private Button secondButton;

    [SerializeField]
    private KeyCode firstButtonKey = KeyCode.UpArrow;
    [SerializeField]
    private KeyCode secondButtonKey = KeyCode.DownArrow;

    [SerializeField]
    private bool ShowFirstButton = false;
    [SerializeField]
    private bool ShowSecondButton = false;

    [SerializeField]
    private string firstButtonText;
    [SerializeField]
    private string secondButtonText;
    [SerializeField]
    private string titleText;
/*
    [TextArea(5, 20)]
    [SerializeField]
    private string infoText;
*/

    [SerializeField]
    private UnityEvent FirstButtonClick;

    [SerializeField]
    private UnityEvent SecondButtonClick;

    private Animator animator;

    bool isEnabled = false;

    private void Start() {
        firstButton.GetComponentInChildren<Text>().text = firstButtonText;
        secondButton.GetComponentInChildren<Text>().text = secondButtonText;
        firstButton.gameObject.SetActive(ShowFirstButton);
        secondButton.gameObject.SetActive(ShowSecondButton);
        txtTitle.text = titleText;
        //txtDescription.text = infoText;
    }

    private void FindAnimator()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator == null)
        {
            Debug.LogWarning($"UISingleMenu {name} needs animator!");
        }
    }

    public void Show() {
        if (isEnabled) {
            Debug.LogWarning($"{name} was already enabled!");
            return;
        }
        FindAnimator();
        isEnabled = true;
        animator.Play("singleMenuShow");
    }

    public void Hide() {
        Debug.Log("Hide " + name + " (enabled: " + isEnabled + ")");
        if (isEnabled) {
            isEnabled = false;
            animator.Play("singleMenuHide");
        }
    }

    public void MenuHideFinished() {
        UIMenu.main.MenuHideFinished(this);
    }


    public void SetDescription(string value) {
        txtDescription.text = value;
    }
    public void SetTitle(string value) {
        txtTitle.text = value;
    }

    /*public void SetEnabled(bool enabled) {
        isEnabled = enabled;
    }*/

/*
    public void ShowButtons(int number) {
        if (number == 0) {
            firstButton.gameObject.SetActive(false);
            secondButton.gameObject.SetActive(false);
        } else if (number == 1) {
            firstButton.gameObject.SetActive(true);
            secondButton.gameObject.SetActive(false);
        } else {
            firstButton.gameObject.SetActive(true);
            secondButton.gameObject.SetActive(true);
        }
    }
*/
    void Update() {
        if (!isEnabled) {
            return;
        }
        if (firstButton.gameObject.activeSelf && Input.GetKeyDown(firstButtonKey)) {
            FirstButtonWasClicked();
        }
        if (secondButton.gameObject.activeSelf && Input.GetKeyDown(secondButtonKey)) {
            SecondButtonWasClicked();
        }
    }

    public void FirstButtonWasClicked() {
        Debug.Log($"FirstButtonClicked ({isEnabled})");
        if (isEnabled) {
            FirstButtonClick.Invoke();
        }
    }
    public void SecondButtonWasClicked() {
        Debug.Log($"SecondButtonWasClicked ({isEnabled})");
        if (isEnabled) {
            SecondButtonClick.Invoke();
        }
    }

}
