// --------------------------------
// <copyright file="HideUnlessAttributeDrawer.cs" company="Rumor Games">
//     Copyright (C) Rumor Games, LLC.  All rights reserved.
// </copyright>
// --------------------------------

using UnityEditor;
using UnityEngine;

/// <summary>
/// HideUnlessAttributeDrawer class.
/// </summary>
[CustomPropertyDrawer(typeof(HideUnlessAttribute))]
public class HideUnlessAttributeDrawer : HideableAttributeDrawer {
    /// <summary>
    /// Gets the HideUnlessAttribute to draw.
    /// </summary>
    private HideUnlessAttribute Attribute {
        get {
            return (HideUnlessAttribute)this.attribute;
        }
    }

    /// <summary>
    /// Checks whether the property is supposed to be hidden.
    /// </summary>
    /// <param name="property">The SerializedProperty to test.</param>
    /// <returns>True if the property should be hidden.</returns>
    protected override bool IsHidden(SerializedProperty property) {
        var attributeProperty = property.serializedObject.FindProperty(this.Attribute.FieldName);
        if (attributeProperty.propertyType == SerializedPropertyType.Boolean) {
            return !attributeProperty.boolValue;
        }

        Debug.LogWarning("HideUnless attribute is referencing an invalid field name: " + this.Attribute.FieldName);
        return false;
    }
}