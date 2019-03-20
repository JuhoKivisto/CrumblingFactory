// --------------------------------
// <copyright file="HideInEditModeAttributeDrawer.cs" company="Rumor Games">
//     Copyright (C) Rumor Games, LLC.  All rights reserved.
// </copyright>
// --------------------------------

using UnityEditor;
using UnityEngine;

/// <summary>
/// HideInEditModeAttributeDrawer class.
/// </summary>
[CustomPropertyDrawer(typeof(HideInEditModeAttribute))]
public class HideInEditModeAttributeDrawer : HideableAttributeDrawer {
    /// <summary>
    /// Checks whether the property is supposed to be hidden.
    /// </summary>
    /// <param name="property">The SerializedProperty to test.</param>
    /// <returns>True if the property should be hidden.</returns>
    protected override bool IsHidden(SerializedProperty property) {
        return !Application.isPlaying;
    }
}