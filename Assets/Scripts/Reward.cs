using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Framework for a Mission Reward.
/// </summary>
[CreateAssetMenu(fileName = "Reward", menuName = "Missions/Reward", order = 3)]
[System.Serializable]
public class Reward : ScriptableObject
{
    public Sprite rewardIcon;
    public string rewardName;
}
