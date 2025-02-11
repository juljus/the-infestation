using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    private GameObject gameManager;

    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEngine.UI.Image bar;
    [SerializeField] private TMPro.TMP_Text text;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    public void SetIfBurning(bool isBurning)
    {
        print("Setting if burning: " + isBurning + " for " + gameObject.name);

        if (isBurning)
        {
            animator.SetBool("isBurning", true);
        }
        else
        {
            animator.SetBool("isBurning", false);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if enemies nearby, don't show the campfire menu button
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return;
            }
        }

        if (collision.CompareTag("Player"))
        {
            gameManager.GetComponent<MapCompletion>().ShowCampfireMenuButton();
            gameManager.GetComponent<MapCompletion>().SetCampfireStandingNextTo(int.Parse(gameObject.name));
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
