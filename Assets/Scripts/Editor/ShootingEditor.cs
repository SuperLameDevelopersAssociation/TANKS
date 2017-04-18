using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shooting)), CanEditMultipleObjects]
public class ShootingEditor : Editor
{
    SerializedObject serObj;
    public SerializedProperty
        currentWeapon_Prop,
        weaponType_Prop,
        flamethrowerObject_Prop,
        laserObject_Prop,
        projectileDamage_Prop,
        laserDamage_Prop,
        flamethrowerDamage_Prop,
        prjectileSpeed_Prop,
        fireFreq_Prop,
        overheatMax_Prop,
        heatUp_Prop,
        cooldownHeatDownAmt_Prop,
        magazineSize_Prop,
        reloadTime_Prop,
        overheatedHeatDownAmt_Prop;

    void OnEnable()
    {
        currentWeapon_Prop = serializedObject.FindProperty("currentWeapon");
        weaponType_Prop = serializedObject.FindProperty("weaponType");
        flamethrowerObject_Prop = serializedObject.FindProperty("flamethrowerObject");
        laserObject_Prop = serializedObject.FindProperty("laserObject");
        projectileDamage_Prop = serializedObject.FindProperty("projectileDamage");
        laserDamage_Prop = serializedObject.FindProperty("laserDamage");
        flamethrowerDamage_Prop = serializedObject.FindProperty("flamethrowerDamage");
        prjectileSpeed_Prop = serializedObject.FindProperty("projectileSpeed");
        fireFreq_Prop = serializedObject.FindProperty("fireFreq");
        overheatMax_Prop = serializedObject.FindProperty("overheatMax");
        heatUp_Prop = serializedObject.FindProperty("heatUpAmount");
        cooldownHeatDownAmt_Prop = serializedObject.FindProperty("cooldownHeatDownAmt");
        overheatedHeatDownAmt_Prop = serializedObject.FindProperty("overheatedHeatDownAmt");
        magazineSize_Prop = serializedObject.FindProperty("magazineSize");
        reloadTime_Prop = serializedObject.FindProperty("reloadTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(currentWeapon_Prop);

        Shooting.CurrentWeapon type = (Shooting.CurrentWeapon)currentWeapon_Prop.enumValueIndex;

        //===========Sets the rest of the primitive values through the same system========

        switch (type)
        {
            case Shooting.CurrentWeapon.Projectile: //if a projectile based system
                projectileDamage_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Damage"), projectileDamage_Prop.intValue);
                prjectileSpeed_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Projectile Speed"), prjectileSpeed_Prop.floatValue);
                fireFreq_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Fire Frequency"), fireFreq_Prop.floatValue);
                magazineSize_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Magazine Size"), magazineSize_Prop.intValue);
                reloadTime_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Reload Time"), reloadTime_Prop.floatValue);
                break;
            case Shooting.CurrentWeapon.Laser:      // if a laser based system
                laserDamage_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Damage"), laserDamage_Prop.intValue);
                overheatMax_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Overheat Max"), overheatMax_Prop.intValue);
                heatUp_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Heat Up Amount"), heatUp_Prop.floatValue);
                cooldownHeatDownAmt_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Cooldown Regeneration Normal Amount"), cooldownHeatDownAmt_Prop.floatValue);
                overheatedHeatDownAmt_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Cooldown Regeneration Overheat Amount"), overheatedHeatDownAmt_Prop.floatValue);
                EditorGUILayout.PropertyField(laserObject_Prop, new GUIContent("Laser Object"));
                break;
            case Shooting.CurrentWeapon.Flamethrower:       // if a flamethrower based system
                flamethrowerDamage_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Damage"), flamethrowerDamage_Prop.intValue);
                overheatMax_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Overheat Max"), overheatMax_Prop.intValue);
                heatUp_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Heat Up Amount"), heatUp_Prop.floatValue);
                cooldownHeatDownAmt_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Cooldown Regeneration Normal Amount"), cooldownHeatDownAmt_Prop.floatValue);
                overheatedHeatDownAmt_Prop.floatValue = EditorGUILayout.FloatField(new GUIContent("Cooldown Regeneration Overheat Amount"), overheatedHeatDownAmt_Prop.floatValue);
                EditorGUILayout.PropertyField(flamethrowerObject_Prop, new GUIContent("Flamethrower Object"));
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
