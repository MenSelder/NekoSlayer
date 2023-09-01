using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slicer2D;
using Slicer2D.Merge;
using Utilities2D;

public class SpriteMerger : MonoBehaviour
{
    private int splatsCounter = 0;
    private Queue<Sprite> spritesQueue;

    private SpriteRenderer spriteRenderer;
    //expand general sprite
    //check on fit in

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spritesQueue = new Queue<Sprite>();
    }

    private void Start()
    {
        //Polygon2D polygon2D = Polygon2D.CreateFromCamera(Camera.main);
        //Slice2D slice2D = Slice2D.
        //Merger.Merge()
    }

    void Update()
    {
        if (spritesQueue.Count <= 0) return;

        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = spritesQueue.Peek();
        }
    }
}
