using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionInfo : ScriptableObject
{
    public string missionName;
    //[TextArea] public string missionDesc = "Mission Desc";

    [Header("Client")]
    public ClientInfo clientInfo;

    [Header("Grid")]
    public Sprite borderSprite;
    public Sprite wallSprite;
    public Sprite floorSprite;
    public int gridWidth;
    public int gridHeight;

    [Header("Affixes")]
    public List<Requirement> affixes = new List<Requirement>();

    //[Header("Mission Affixes")]
    //public List<string> requirements = new List<string>();

    [Header("Rewards")]
    public int missionMoney = 100;
    public ItemInfo itemBlueprint;
}


