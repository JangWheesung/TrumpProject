using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SniperController sniperController;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Volume volume;
    [SerializeField] private float bulletSpeed;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sniperController.OnShotEvt += Fly;
    }

    private void Fly()
    {
        VCamSystem.Instance.VCamToBullet();

        StartCoroutine(FlyTimeSequence());
    }

    private void SetFlyVolume(bool value)
    {
        if (volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = value;
        }

        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = value;
        }
    }

    private IEnumerator FlyTimeSequence()
    {
        bulletSpeed /= 50f;
        rb.velocity = transform.forward * bulletSpeed;

        yield return new WaitForSeconds(2f);

        bulletSpeed *= 30f;
        rb.velocity = transform.forward * bulletSpeed;

        SetFlyVolume(true);
    }
}
