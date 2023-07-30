using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

/// <summary>
/// Framework for a Client, someone who delivers missions.
/// </summary>
[CreateAssetMenu(fileName = "Client", menuName = "Missions/Client", order = 2)]
[System.Serializable]
public class Client : ScriptableObject
{
    [Title("Basic Client Info")]
    public string clientName = "Client Name";
    public string closingPhrase = "Thanks";

    [Title("Advanced Client Info")]
    public int clientTier = 1;


    [Space(10)]
    [Title("Client Preference Info")]
    public List<Preference> preferences;

    [Space(10)]
    [Title("Client Mission Strings")]
    public List<ClientMissionString> clientMissionStrings;
}

/// <summary>
/// Framework for Client-specific Mission names and descriptions,
/// used in generated missions.
/// </summary>
[System.Serializable]
public class ClientMissionString 
{
    public string missionName = "Mission Name";
    [TextArea(5, 10)]
    public string missionDesc = "Mission Description";
}
