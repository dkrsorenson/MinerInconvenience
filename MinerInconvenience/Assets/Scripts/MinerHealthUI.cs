using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerHealthUI : MonoBehaviour
{
    public Sprite[] playerLifeStates;
    public Image lifeImage;

    // Kicks off before start method
    void Awake()
    {
        lifeImage = GetComponent<Image>();
        lifeImage.sprite = playerLifeStates[0];
    }

    /// <summary>
    /// Update the sprite being displayed
    /// </summary>
    /// <param name="index">The index of the sprite to display</param>
    public void UpdateLifeSprite(int index)
    {
        lifeImage.sprite = playerLifeStates[index];
    }

    /// <summary>
    /// Get the number of sprites to display
    /// </summary>
    /// <returns>The number of sprites</returns>
    public int GetSpriteCount()
    {
        return playerLifeStates.Length;
    }
}
