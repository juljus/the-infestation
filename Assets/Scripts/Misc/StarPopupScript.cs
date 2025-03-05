using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPopupScript : MonoBehaviour
{
    [SerializeField] private GameObject starGO;
    [SerializeField] private int starCount;
    [SerializeField] private float starCycleTime;
    [SerializeField] private float starInnerRadius;
    [SerializeField] private float starOuterRadius;

    private GameObject[] stars;

    private void Start()
    {
        stars = new GameObject[starCount];

        // place all stars randomly
        for (int i = 0; i < starCount; i++)
        {
            float angle = Random.Range(0, 360);
            float radius = Random.Range(starInnerRadius, starOuterRadius);
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            GameObject star = Instantiate(starGO, transform);
            star.transform.localPosition = pos;

            stars[i] = star;
        }
        
        StartCoroutine(CycleStars());
    }

    private IEnumerator CycleStars()
    {
        // delete last star and move all stars one index down then place new star at the end
        while (true)
        {
            GameObject lastStar = stars[starCount - 1];
            for (int i = starCount - 1; i > 0; i--)
            {
                stars[i] = stars[i - 1];
            }

            float angle = Random.Range(0, 360);
            float radius = Random.Range(starInnerRadius, starOuterRadius);
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            lastStar.transform.localPosition = pos;
            stars[0] = lastStar;

            yield return new WaitForSeconds(starCycleTime);
        }
    }
}
