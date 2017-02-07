using UnityEngine;
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
        cooldown_Prop;

    Shooting shooting;


    void OnEnable()
    {
        serObj = new SerializedObject(target);
        currentWeapon_Prop = serObj.FindProperty("currentWeapon");
        weaponType_Prop = serObj.FindProperty("weaponType");
        projectileType_Prop = serObj.FindProperty("projectileType");
        sustainedProjectile_Prop = serObj.FindProperty("sustainedProjectile");
        damage_Prop = serObj.FindProperty("damage");
        prjectileSpeed_Prop = serObj.FindProperty("projectileSpeed");
        fireFreq_Prop = serObj.FindProperty("fireFreq");
        overheatMax_Prop = serObj.FindProperty("overheatMax");
        heatUp_Prop = serObj.FindProperty("heatUpAmount");
        cooldownHeatDownAmt_Prop = serObj.FindProperty("cooldownHeatDownAmt");
        overheatedHeatDownAmt_Prop = serObj.FindProperty("overheatedHeatDownAmt");
        cooldown_Prop = serObj.FindProperty("cooldown");

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
