using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Preferences are client-specific and determine what requirements a procedurally-generated
/// Mission will have.
/// </summary>
[System.Serializable]
public class Preference
{
    [TextArea(5, 8)]
    [Tooltip("How this Preference is displayed in in-game tooltips.")]
    public string prefName = "Loves the color red.";

    [Space(10)]
    [Tooltip("List of possible basic Requirements this Preference could call for.")]
    [DetailedInfoBox("List of possible basic Requirements this Preference could call for.", 
        "Note that setting requirement.count here is unnecessary.\n" + 
        "Also note that these will primarily be used for procedurally-generated Missions.")]
    public List<Requirement> requirements;
}

/// <summary>
/// Currently unused.
/// </summary>
public enum PreferenceType
{
    ColorPref, ItemPref
}