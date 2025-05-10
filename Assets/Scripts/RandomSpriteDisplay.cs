using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private Sprite[] randomSprites;

    private void Awake()
    {
        if (randomSprites.Length > 0 && targetRenderer != null)
        {
            int index = Random.Range(0, randomSprites.Length);
            targetRenderer.sprite = randomSprites[index];
        }
    }
}
