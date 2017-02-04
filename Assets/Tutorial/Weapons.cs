using UnityEngine;
using System;

public class Weapons : MonoBehaviour
{
    public enum WeaponType{ Projectile, Laser, FlameThrower };

    [HideInInspector]
    public WeaponType weaponType;

    [HideInInspector]
    public GameObject[] projectileType;

    [HideInInspector]
    public int[] damage;

    [HideInInspector]
    public float[] projectileSpeed;
    [HideInInspector]
    public float[] fireFreq;
    [HideInInspector]
    public float[] cooldown;
    [HideInInspector]
    public float[] duration;

    public void CreateArrays()
    {
        int enumLength = Enum.GetNames(typeof(WeaponType)).Length;

        if (projectileType.Length == 0)
            projectileType = new GameObject[enumLength];

        if (damage.Length == 0)
            damage = new int[enumLength];

        if (projectileSpeed.Length == 0)
            projectileSpeed = new float[enumLength];

        if (fireFreq.Length == 0)
            fireFreq = new float[enumLength];

        if (cooldown.Length == 0)
            cooldown = new float[enumLength];

        if (duration.Length == 0)
            duration = new float[enumLength];
    }
}
