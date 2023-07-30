using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

/// <summary>
/// Types of rooms. Determines what items should be counted as "essential."
/// </summary>
public enum RoomType
{
    Bedroom,
    Office,
    [Display(Name = "Living Room")]
    LivingRoom,
    [Display(Name = "Dining Room")]
    DiningRoom
}
