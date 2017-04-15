using UnityEngine;
using System.Collections;

public class ArcReactorDemo4Mirror2d : MonoBehaviour
{

    public GameObject hitParticles;
    public float rotationSpeed;
    public float temperature;
    public Gradient colorGrad;

    private GameObject partSystemObj;
    private bool mirrorHit;
    private bool oldMirrorHit;

    public float rotationSpeedCoef = 15;
    public float minSpeed = 7;
    const float dissipateRate = 0.25f;
    const float heatRate = 0.5f;

    private Renderer rend;
    private ParticleSystem partSystem;
    private ParticleSystem.EmissionModule emission;


    public void ArcReactorReflection(ArcReactorHitInfo2D hit)
    {
        mirrorHit = true;

        temperature = Mathf.Clamp01(temperature + heatRate * Time.deltaTime);

        if (!partSystemObj.activeSelf)
            partSystemObj.SetActive(true);

        if (!emission.enabled)
            emission.enabled = true;

        partSystemObj.transform.position = hit.raycastHit.point;
        Vector2 point = hit.raycastHit.point + hit.raycastHit.normal;
        partSystemObj.transform.LookAt(new Vector3(point.x, point.y, transform.position.z));
    }

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        transform.Rotate(Vector3.forward * UnityEngine.Random.Range(0.0f, 360.0f));
        rotationSpeed = UnityEngine.Random.Range(-rotationSpeedCoef, rotationSpeedCoef);
        if (Mathf.Abs(rotationSpeed) < minSpeed)
        {
            rotationSpeed = Mathf.Sign(rotationSpeed) * minSpeed;
        }


        partSystemObj = (GameObject)GameObject.Instantiate(hitParticles);
        partSystemObj.transform.parent = transform;
        partSystemObj.SetActive(false);
        partSystem = partSystemObj.GetComponent<ParticleSystem>();
        emission = partSystem.emission;
    }

    // Update is called once per frame
    void Update()
    {
        //If ray stopped hitting mirror
        if (!mirrorHit && !oldMirrorHit && emission.enabled)
        {
            emission.enabled = false;
        }
        oldMirrorHit = mirrorHit;
        mirrorHit = false;

        if (!emission.enabled && partSystemObj.activeSelf && !partSystem.IsAlive())
        {
            partSystemObj.SetActive(false);
        }

        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));

        temperature = Mathf.Clamp01(temperature - dissipateRate * Time.deltaTime);

        if (rend != null)
            rend.material.color = colorGrad.Evaluate(temperature);
    }
}
