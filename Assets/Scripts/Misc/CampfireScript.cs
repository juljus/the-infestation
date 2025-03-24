using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    private GameObject gameManager;
    private MapCompletion mapCompletion;

    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEngine.UI.Image bar;
    [SerializeField] private TMPro.TMP_Text text;

    [SerializeField] private GameObject campfireArea;
    
    private bool buttonShouldBeShown = false;
    private bool buttonShown = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
        mapCompletion = gameManager.GetComponent<MapCompletion>();
    }

    private void FixedUpdate()
    {
        // if the button should be shown and it hasn't been shown yet, show the button
        if (buttonShouldBeShown && !buttonShown)
        {
            mapCompletion.ShowCampfireMenuButton();
            mapCompletion.SetCampfireStandingNextTo(int.Parse(gameObject.name));
            buttonShown = true;
        }

        // if the button should not be shown and it has been shown, hide the button
        if (!buttonShouldBeShown && buttonShown)
        {
            mapCompletion.HideCampfireMenuButton();
            buttonShown = false;
        }

        // set button should be shown to false
        buttonShouldBeShown = false;
    }

    public void SetIfBurning(bool isBurning)
    {
        if (isBurning)
        {
            animator.SetBool("isBurning", true);

            // hide the campfire area
            campfireArea.SetActive(false);
        }
        else
        {
            animator.SetBool("isBurning", false);

            // show the campfire area
            campfireArea.SetActive(true);
        }
    }

    public void SetBarFillAndText(float fillAmount, string newText)
    {
        bar.fillAmount = fillAmount;
        text.text = newText;
    }

    public void HideBar()
    {
        bar.transform.parent.gameObject.SetActive(false);
    }

    public void ShowBar()
    {
        bar.transform.parent.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // if the collision is the player check if there are enemies nearby
        if (collision.CompareTag("Player"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyCheckRadius);
            if (colliders.Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Enemy") || collider.CompareTag("Boss") || collider.CompareTag("Minion"))
                    {
                        return;
                    }
                }
            }

            // if no enemies nearby, set the campfire button as should be shown
            buttonShouldBeShown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.GetComponent<MapCompletion>().HideCampfireMenuButton();
        }
    }
}
