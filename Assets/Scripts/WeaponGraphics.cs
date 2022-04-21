using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGraphics : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;
    public GameObject bulletTracer;
    public AudioClip audioShoot;
    public AudioSource audioSource;
    public LineRenderer lineRenderer;

    public void PlayAudioShoot()
    {
        audioSource.PlayOneShot(audioShoot);
    }

    public void RenderTracer(Vector3 endPos)
    {
        StartCoroutine(E_RenderTracer(endPos));
    }

    IEnumerator E_RenderTracer(Vector3 endPos)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, lineRenderer.transform.position + endPos);
        yield return new WaitForEndOfFrame();
        lineRenderer.enabled = false;
    }
}
