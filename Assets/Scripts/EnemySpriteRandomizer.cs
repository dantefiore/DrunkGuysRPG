using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteRandomizer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int rng = Random.Range(0, sprites.Count);

        spriteRenderer.sprite = sprites[rng];
    }
}
