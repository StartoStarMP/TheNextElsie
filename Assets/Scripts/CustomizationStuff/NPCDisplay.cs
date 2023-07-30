using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDisplay : MonoBehaviour
{
    public Sprite[] backgrounds;
    public Sprite[] faces;
    public Sprite[] hairstyles;
    public Sprite[] eyes;
    public Sprite[] mouths;
    public Color[] skinColors;
    public Color[] hairColors;

    public Image background;
    public Image background2;
    public Image face;
    public Image hair;
    public Image eye;
    public Image mouth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateNPCDisplay(ClientInfo clientInfo)
    {
        background2.sprite = clientInfo.backgroundSprite;
        face.sprite = clientInfo.faceSprite;
        hair.sprite = clientInfo.hairSprite;
        eye.sprite = clientInfo.eyeSprite;
        mouth.sprite = clientInfo.mouthSprite;

        background.color = clientInfo.bg1Color;
        background2.color = clientInfo.bg2Color;
        face.color = clientInfo.faceColor;
        hair.color = clientInfo.hairColor;
    }
}
