using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapons)), CanEditMultipleObjects]
public class WeaponsEditor : Editor
{
    SerializedObject serObj;
    public SerializedProperty
        weaponType_Prop,
        projectileType_Prop,
        damage_Prop,
        prjectileSpeed_Prop,
        fireFreq_Prop,
        cooldown_Prop,
        dd_Prop,
        duration_Prop;
    
    Weapons weapons;
        

    void OnEnable()
    {
        serObj = new SerializedObject(target);
        weaponType_Prop = serObj.FindProperty("weaponType");
        projectileType_Prop = serObj.FindProperty("projectileType");
        damage_Prop = serObj.FindProperty("damage");
        prjectileSpeed_Prop = serObj.FindProperty("projectileSpeed");
        fireFreq_Prop = serObj.FindProperty("fireFreq");
        cooldown_Prop = serObj.FindProperty("cooldown");
        duration_Prop = serObj.FindProperty("duration");
        weapons = target as Weapons; //grabs the script itself so that it can directly edit it
        weapons.CreateArrays();     //declares the length of the arrays
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponType_Prop, new GUIContent("Weapon Type"));

        //======Using Enums, figures out the index of the enum selected=======
        int enumIndex = weaponType_Prop.enumValueIndex;
        Weapons.WeaponType type = (Weapons.WeaponType)weaponType_Prop.enumValueIndex;

        //======Sets the GameObject through the use of a Serialized Property========
        SerializedProperty thisGameObject = projectileType_Prop.GetArrayElementAtIndex(enumIndex);
        EditorGUILayout.PropertyField(thisGameObject, new GUIContent("Projectile Type"));
        weapons.projectileType[enumIndex] = (GameObject)thisGameObject.objectReferenceValue;

        //===========Sets the rest of the primitive values through the same system========
        weapons.damage[enumIndex] = EditorGUILayout.IntField(new GUIContent("Damage"), weapons.damage[enumIndex]);

        switch (type)
        {
            case Weapons.WeaponType.Projectile: //if a projectile based system
                weapons.projectileSpeed[enumIndex] = EditorGUILayout.FloatField(new GUIContent("Projectile Speed"), weapons.projectileSpeed[enumIndex]);
                weapons.fireFreq[enumIndex] = EditorGUILayout.FloatField(new GUIContent("Fire Frequency"), weapons.fireFreq[enumIndex]);
                break;
            case Weapons.WeaponType.Laser:      // if a non-projectile based system
            case Weapons.WeaponType.FlameThrower:
                weapons.cooldown[enumIndex] = EditorGUILayout.FloatField(new GUIContent("Weapon Cooldown"), weapons.cooldown[enumIndex]);
                weapons.duration[enumIndex] = EditorGUILayout.FloatField(new GUIContent("Duration"), weapons.duration[enumIndex]);
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
