using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        string displayName;
        displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();
        if (String.IsNullOrEmpty(displayName))
        {
            displayName = enumValue.ToString();
        }
        return displayName;
    }
}
