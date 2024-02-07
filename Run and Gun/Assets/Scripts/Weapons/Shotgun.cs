using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shotgun : BaseWeapon
{
    // Start is called before the first frame update

    [SerializeField] private int pelletCount;
    public Vector3 Spread;
    private float LastUse = 0;
    public LayerMask HitMask;

    public TrailConfigScriptableObject TrailConfig;
    private ObjectPool<TrailRenderer> TrailPool;

    private void Start()
    {
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
    }
    public override void Fire(float damage, float range, float cooldown)
    {
        if (Time.time > cooldown + LastUse)
        {
            LastUse = Time.time;
            for (int i = 0; i < pelletCount; i++)
            {
                Vector3 aimDirection = origin.forward
                                    + new Vector3(
                                        Random.Range(-Spread.x, Spread.x),
                                        Random.Range(-Spread.y, Spread.y),
                                        Random.Range(-Spread.z, Spread.z)
                                        );
                aimDirection.Normalize();
                if (Physics.Raycast(origin.position, aimDirection, out RaycastHit hit, range, HitMask))
                {
                    StartCoroutine(PlayTrail(transform.position, hit.point, hit));
                }
                else
                {
                    StartCoroutine(PlayTrail(transform.position, transform.position + (aimDirection * range), new RaycastHit()));
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
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(StartPoint, EndPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;
        try
        {
            if (Hit.collider.GetComponent<Damagable>())
            {
                Damagable damagable = Hit.collider.GetComponent<Damagable>();
                Transform hitpoint = Hit.transform;
                damagable.SetDamage(damage, hitpoint);
            }
            else if (Hit.collider.TryGetComponent<DestructableTerrainObject>(out DestructableTerrainObject terrain))
            {
                terrain.TakeDamage(damage, canDestroy);
            }
            else if (Hit.collider.TryGetComponent<Target>(out Target target))
            {
                target.onShot.Invoke();
            }

        }
        catch
        {

        }

        yield return new WaitForSeconds(2);
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
