using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Weapons)), CanEditMultipleObjects]
public class WeaponsEditor : Editor
{
    public SerializedProperty
        weaponType_Prop,
        projectileType_Prop,
        damage_Prop,
        prjectileSpeed_Prop,
        fireFreq_Prop,
        cooldown_Prop,
        duration_Prop;

    void OnEnable()
    {
        weaponType_Prop = serializedObject.FindProperty("weaponType");
        projectileType_Prop = serializedObject.FindProperty("projectileType");
        damage_Prop = serializedObject.FindProperty("damage");
        prjectileSpeed_Prop = serializedObject.FindProperty("projectileSpeed");
        fireFreq_Prop = serializedObject.FindProperty("fireFreq");
        cooldown_Prop = serializedObject.FindProperty("cooldown");
        duration_Prop = serializedObject.FindProperty("duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponType_Prop, new GUIContent("Object"));
        Weapons.WeaponType type = (Weapons.WeaponType)weaponType_Prop.enumValueIndex;

        switch(type)
        {
            case Weapons.WeaponType.Projectile:
                EditorGUILayout.PropertyField(projectileType_Prop, new GUIContent("Projectile Type"));
                EditorGUILayout.PropertyField(damage_Prop, new GUIContent("Damage Amount"));
                EditorGUILayout.PropertyField(prjectileSpeed_Prop, new GUIContent("Projectile Speed"));
                EditorGUILayout.PropertyField(fireFreq_Prop, new GUIContent("Fire Frequency"));
                break;
            case Weapons.WeaponType.Laser:
                EditorGUILayout.PropertyField(projectileType_Prop, new GUIContent("Projectile Type"));
                EditorGUILayout.PropertyField(damage_Prop, new GUIContent("Damage Amount"));
                EditorGUILayout.PropertyField(cooldown_Prop, new GUIContent("Weapon Cooldown"));
                EditorGUILayout.PropertyField(duration_Prop, new GUIContent("Active Duration"));
                break;
            case Weapons.WeaponType.FlameThrower:
                EditorGUILayout.PropertyField(projectileType_Prop, new GUIContent("Projectile Type"));
                EditorGUILayout.PropertyField(damage_Prop, new GUIContent("Damage Amount"));
                EditorGUILayout.PropertyField(cooldown_Prop, new GUIContent("Weapon Cooldown"));
                EditorGUILayout.PropertyField(duration_Prop, new GUIContent("Active Duration"));
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
