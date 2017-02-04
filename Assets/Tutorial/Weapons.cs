using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour
{
    public enum WeaponType{ Projectile, Laser, FlameThrower };

    [HideInInspector]
    public WeaponType weaponType;

    [HideInInspector]
    public GameObject projectileType;

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




}
