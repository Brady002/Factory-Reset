using UnityEngine;
[CreateAssetMenu(fileName = "Utilize Config", menuName ="Weapons/Activation Configuration", order = 2)]
public class UseConfirgurationScriptableObject : ScriptableObject
{
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0f, 0f, 0f);
    public float FireRate = 1f;
    public int ProjectileAmount = 1;
    public float MaxDistance = 100f;
    public int ProjectileType = 1; //0 = Melee, 1 = Hitscan, 2 = Projectile
    public Transform ShootOrigin;
    /*public enum ProjectileType
    {
        Hitscan,
        Projectile,
        Melee,
    }*/
}
