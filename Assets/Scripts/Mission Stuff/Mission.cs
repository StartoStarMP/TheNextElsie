using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// Framework for Missions.
/// </summary>
[CreateAssetMenu(fileName = "Mission", menuName = "Missions/Mission", order = 1)]
[System.Serializable]
public class Mission : ScriptableObject
{
    [Title("$missionTitle")]
    [Space(10)]

    [Header("Parameters")]
    public string missionTitle = "Mission Title";
    [TextArea] public string missionDesc = "Mission Desc";
    [Required("Mission Client is required.")]
    public Client missionClient;
    public bool isStoryMission = false;

    [Header("Rewards")]
    public int missionMoney = 100;
    public List<Reward> missionRewards = new List<Reward>();

    [Header("Mission System")]
    [Tooltip("Missions that should be unlocked once this Mission is completed.")]
    public List<Mission> nextMissions = new List<Mission>();

    [Header("Mission Requirements")]
    [Tooltip("Criteria that the player must meet in order to finish the Mission.")]
    public List<Requirement> requirements = new List<Requirement>();
}


