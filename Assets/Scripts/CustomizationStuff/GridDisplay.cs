using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDisplay : MonoBehaviour
{
    public Vector2 displayDimensions;

    public Image gridSection;
    public Image borderSection;
    public Image wallSection;
    public Image floorSection;
    public Text dimensions;

    public Sprite[] testSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

    }

    public void CreateGrid(Sprite borderSectionSprite, Sprite wallSectionSprite, Sprite floorSectionSprite, int width, int height)
    {
        borderSection.sprite = borderSectionSprite;
        wallSection.sprite = wallSectionSprite;
        floorSection.sprite = floorSectionSprite;

        float cellSize;

        if (width >= height * (4f/3))
        {
            cellSize = displayDimensions.x * 0.9f / width;
            gridSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.4375f * width;
            borderSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.4375f * width/3;
            wallSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.4375f * width/3;
            floorSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.4375f * width/3;

            gridSection.rectTransform.sizeDelta = new Vector2(displayDimensions.x * 0.9f, height * cellSize);
            borderSection.rectTransform.sizeDelta = new Vector2(displayDimensions.x, (cellSize * height) + 20);
            wallSection.rectTransform.sizeDelta = new Vector2(displayDimensions.x * 0.9f, cellSize * 3);
            floorSection.rectTransform.sizeDelta = new Vector2(displayDimensions.x * 0.9f, cellSize * (height - 3));
        }
        else
        {
            cellSize = displayDimensions.y * 0.9f / height;
            gridSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.91f * height;
            borderSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.91f * height/3;
            wallSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.91f * height/3;
            floorSection.GetComponent<Image>().pixelsPerUnitMultiplier = 1.91f * height/3;

            gridSection.rectTransform.sizeDelta = new Vector2(width * cellSize, displayDimensions.y * 0.9f);
            borderSection.rectTransform.sizeDelta = new Vector2((cellSize * width) + 20, displayDimensions.y);
            wallSection.rectTransform.sizeDelta = new Vector2(width * cellSize, cellSize * 3);
            floorSection.rectTransform.sizeDelta = new Vector2(width * cellSize, cellSize * (height - 3));
        }

        dimensions.text = width + " x " + height;
    }
}
