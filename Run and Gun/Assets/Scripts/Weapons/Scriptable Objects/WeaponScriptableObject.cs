using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapon", order = 0)]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnPoint2;
    public Vector3 SpawnRotation;
    public Vector3 HitscanOrigin;
    public GameObject ShootOrigin;
    

    public UseConfirgurationScriptableObject UseConfig;
    public TrailConfigScriptableObject TrailConfig;
    public DamageConfigScriptableObject DamageConfig;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastUseTime;
    private ParticleSystem UseSystemRight;
    private ParticleSystem UseSystemLeft;
    private ObjectPool<TrailRenderer> TrailPool;
    private GameObject Origin;
    private ParticleSystem ShootOriginSystem;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour, int EquipSlot)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastUseTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        if(EquipSlot == 1)
        {
            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(Parent, false);
            Model.transform.localPosition = SpawnPoint;
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            UseSystemRight = Model.GetComponentInChildren<ParticleSystem>();
        } else if (EquipSlot == 2)
        {
            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(Parent, false);
            Model.transform.localPosition = SpawnPoint2;
            Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            UseSystemLeft = Model.GetComponentInChildren<ParticleSystem>();
        }

        Origin = Instantiate(ShootOrigin);
        Origin.transform.SetParent(Parent, false);
        Origin.transform.localPosition = HitscanOrigin;

        
        
        ShootOriginSystem = Origin.GetComponentInChildren<ParticleSystem>();
    }

    public void Use(int EquipSlot)
    {
        if(Time.time > UseConfig.FireRate + LastUseTime)
        {
            LastUseTime = Time.time;
            for(int i = 0; i < UseConfig.ProjectileAmount; i++)
            {
                if (EquipSlot == 1)
                {
                    UseSystemRight.Play();
                }
                else if (EquipSlot == 2)
                {
                    UseSystemLeft.Play();
                }

                Vector3 aimDirection = ShootOriginSystem.transform.forward  //UseSystem.transform.forward
                    + new Vector3(
                        Random.Range(-UseConfig.Spread.x, UseConfig.Spread.x),
                        Random.Range(-UseConfig.Spread.y, UseConfig.Spread.y),
                        Random.Range(-UseConfig.Spread.z, UseConfig.Spread.z
                        ));
                aimDirection.Normalize();

                if (UseConfig.ProjectileType == 1)
                {

                    if (Physics.Raycast(
                    ShootOriginSystem.transform.position, //UseSystem.transform.forward
                    aimDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    UseConfig.HitMask
                    ))
                    {
                        if (EquipSlot == 1)
                        {
                            ActiveMonoBehaviour.StartCoroutine(PlayTrail(UseSystemRight.transform.position, hit.point, hit)); //Plays trail on hit
                        }
                        else if (EquipSlot == 2)
                        {
                            ActiveMonoBehaviour.StartCoroutine(PlayTrail(UseSystemLeft.transform.position, hit.point, hit)); //Plays trail on hit
                        }

                    }
                    else
                    {
                        //ActiveMonoBehaviour.StartCoroutine(PlayTrail(UseSystemRight.transform.position, UseSystemRight.transform.position + (aimDirection * TrailConfig.MissDistance), new RaycastHit()));
                        Debug.Log("Daw");

                        if (EquipSlot == 1)
                        {
                            ActiveMonoBehaviour.StartCoroutine(PlayTrail(UseSystemRight.transform.position, UseSystemRight.transform.position + (aimDirection * TrailConfig.MissDistance), new RaycastHit()));
                        }
                        else if (EquipSlot == 2)
                        {
                            ActiveMonoBehaviour.StartCoroutine(PlayTrail(UseSystemLeft.transform.position, UseSystemLeft.transform.position + (aimDirection * TrailConfig.MissDistance), new RaycastHit()));
                        }
                    }

                }
            }
            
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while(remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(StartPoint, EndPoint, Mathf.Clamp01(1 -(remainingDistance/distance)));
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if(Hit.collider != null)
        {
            //Debug.Log("woo");
        }

        if (Hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log("Hit for: " + DamageConfig.Damage);
            damageable.TakeDamage(DamageConfig.Damage);
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
    }
    private TrailRenderer CreateTrail()
    {

        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
