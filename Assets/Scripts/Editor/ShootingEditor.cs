﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shooting)), CanEditMultipleObjects]
public class ShootingEditor : Editor
{
    SerializedObject serObj;
    public SerializedProperty
        currentWeapon_Prop,
        weaponType_Prop,
        projectileType_Prop,
        sustainedProjectile_Prop,
        damage_Prop,
        prjectileSpeed_Prop,
        fireFreq_Prop,
        overheatMax_Prop,
        heatUp_Prop,
        cooldownHeatDownAmt_Prop,
        overheatedHeatDownAmt_Prop,
        cooldown_Prop,
        poolSize_Prop;

    Shooting shooting;


    void OnEnable()
    {
        currentWeapon_Prop = serializedObject.FindProperty("currentWeapon");
        weaponType_Prop = serializedObject.FindProperty("weaponType");
        projectileType_Prop = serializedObject.FindProperty("projectileType");
        sustainedProjectile_Prop = serializedObject.FindProperty("sustainedProjectile");
        damage_Prop = serializedObject.FindProperty("damage");
        prjectileSpeed_Prop = serializedObject.FindProperty("projectileSpeed");
        fireFreq_Prop = serializedObject.FindProperty("fireFreq");
        overheatMax_Prop = serializedObject.FindProperty("overheatMax");
        heatUp_Prop = serializedObject.FindProperty("heatUpAmount");
        cooldownHeatDownAmt_Prop = serializedObject.FindProperty("cooldownHeatDownAmt");
        overheatedHeatDownAmt_Prop = serializedObject.FindProperty("overheatedHeatDownAmt");
        cooldown_Prop = serializedObject.FindProperty("cooldown");
        poolSize_Prop = serializedObject.FindProperty("poolSize");

        shooting = target as Shooting;

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(currentWeapon_Prop);

        //EditorGUILayout.PropertyField(weaponType_Prop, new GUIContent("Weapon Type"));

        //DrawDefaultInspector();

        int enumIndex = currentWeapon_Prop.enumValueIndex;
        Shooting.CurrentWeapon type = (Shooting.CurrentWeapon)currentWeapon_Prop.enumValueIndex;

        //weapons.projectileType[enumIndex] = (GameObject)thisGameObject.objectReferenceValue;

        //===========Sets the rest of the primitive values through the same system========
        damage_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Damage"), damage_Prop.intValue);

        switch (type)
        {
            case Shooting.CurrentWeapon.Projectile: //if a projectile based system              
                prjectileSpeed_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Projectile Speed"), prjectileSpeed_Prop.floatValue);
                fireFreq_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Fire Frequency"), fireFreq_Prop.floatValue);
                poolSize_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Pool Size"), poolSize_Prop.intValue);
                EditorGUILayout.PropertyField(projectileType_Prop, new GUIContent("Projectile Object"));
                break;
            case Shooting.CurrentWeapon.Laser:      // if a non-projectile based system
            case Shooting.CurrentWeapon.Flamethrower:
                overheatMax_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Overheat Max"), overheatMax_Prop.intValue);
                heatUp_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Heat Up Amount"), heatUp_Prop.floatValue);
                cooldown_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Weapon Cooldown"), cooldown_Prop.floatValue);
                cooldownHeatDownAmt_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Cooldown Regenation Normal Amount"), cooldownHeatDownAmt_Prop.floatValue);
                overheatedHeatDownAmt_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Cooldown Regenetation Overheat Amount"), overheatedHeatDownAmt_Prop.floatValue);
                EditorGUILayout.PropertyField(sustainedProjectile_Prop, new GUIContent("Sustained Object"));
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
