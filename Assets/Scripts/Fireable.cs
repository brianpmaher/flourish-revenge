using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Fireable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private VisualEffect flamethrowerEffect;
    [SerializeField] private AudioSource flamethrowerSoundEffect;
    [SerializeField] private GameObject flameColliderPrefab;
    [SerializeField] private Transform nozzleTransform;

    private bool isFiring;
    
    private void Awake()
    {
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
    }

    public void StartFiring()
    {
        isFiring = true;
        flamethrowerEffect.Play();
        flamethrowerSoundEffect.Play();
        StartCoroutine(SpawnFlameColliders());
    }

    public void StopFiring()
    {
        isFiring = false;
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
        StopCoroutine(SpawnFlameColliders());
    }

    private IEnumerator SpawnFlameColliders()
    {
        var flameCollider = Instantiate(flameColliderPrefab);
        flameCollider.transform.position = nozzleTransform.position;
        flameCollider.transform.rotation = nozzleTransform.rotation;
        
        yield return new WaitForSeconds(0.5f);
        
        if (isFiring)
        {
            yield return StartCoroutine(SpawnFlameColliders());
        }
    }
}