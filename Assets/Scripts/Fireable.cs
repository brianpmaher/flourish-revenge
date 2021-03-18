using UnityEngine;
using UnityEngine.VFX;

public class Fireable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private VisualEffect flamethrowerEffect;
    [SerializeField] private AudioSource flamethrowerSoundEffect;

    private void Awake()
    {
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
    }

    public void StartFiring()
    {
        flamethrowerEffect.Play();
        flamethrowerSoundEffect.Play();
    }

    public void StopFiring()
    {
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
    }
}