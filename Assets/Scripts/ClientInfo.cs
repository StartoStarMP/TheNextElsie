using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Client", menuName = "Client")]
public class ClientInfo : ScriptableObject
{
    public Sprite backgroundSprite;
    public Sprite faceSprite;
    public Sprite hairSprite;
    public Sprite eyeSprite;
    public Sprite mouthSprite;
    public Color bg1Color;
    public Color bg2Color;
    public Color faceColor;
    public Color hairColor;
}