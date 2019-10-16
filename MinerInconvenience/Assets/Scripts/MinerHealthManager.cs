using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerHealthManager : MonoBehaviour
{
    public Image[] healthImages;
    public Image healthImagePrefab;
    private const int maxLives = 5;

    private Vector2 origin = new Vector2(25, -25);
    private int offsetX = 35;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Kicks off before start method
    void Awake()
    {
        // Instantiate all of the player life images
        for (int i = 0; i < maxLives; i++)
        {
            healthImages[i] = Instantiate(healthImagePrefab, origin, Quaternion.identity);
            healthImages[i].rectTransform.SetParent(this.transform, false);
            origin.x += offsetX;
        }
    }

    /// <summary>
    /// Update the UI for the player lives
    /// </summary>
    /// <param name="lives">The player life count</param>
    public void UpdateLifeDisplay(float lives)
    {
        for (int i = 0; i < maxLives; i++)
        {
            if (i < lives)
            {
                if (!healthImages[i].IsActive()) healthImages[i].gameObject.SetActive(true);

                if (lives % 1 == 0.5 && i + 1 > lives) healthImages[i].GetComponent<MinerHealthUI>().UpdateLifeSprite(1);
                else healthImages[i].GetComponent<MinerHealthUI>().UpdateLifeSprite(0);
            }
            else
            {
                if (healthImages[i].IsActive()) healthImages[i].gameObject.SetActive(false);
            }
        }
    }
}
