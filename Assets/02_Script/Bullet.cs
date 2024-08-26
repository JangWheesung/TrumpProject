using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class Bullet : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private SniperController sniperController;
    [SerializeField] private Target target;
    [Header("Object")]
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject bloodPrefab;
    [Header("Value")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask hitLayer;

    private Rigidbody rb;
    private float currentTime = 0f;
    private float bulletSpeed;
    private bool isRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sniperController.OnShotEvt += Fly;
    }

    private void Update()
    {
        RangeTarget();
        MissingTarget();
    }

    private void RangeTarget()
    {
        if (isRange) return;

        Collider[] trumpGetCol = Physics.OverlapSphere(transform.position, radius, targetLayer);

        if (trumpGetCol.Length > 0)
        {
            SetSlowMotion(0.1f, 0.1f);

            currentTime += Time.deltaTime;
            if (currentTime >= 2f)
            {
                currentTime = 0f;
                isRange = true;

                ShotTarget();
                SetSlowMotion(100f, 1f);
            }
        }
    }

    private void ShotTarget()
    {
        target.SetAnimEnable(false);

        RaycastHit hit;
        if (IsTargetHit(out hit)) //성공
        {
            Instantiate(bloodPrefab, hit.point + (-transform.forward * 1.7f), 
                Quaternion.LookRotation(-transform.forward), hit.transform);

            GameEndManager.Instance.GameClearUI();
        }
        else //실패
        {
            target.SetAnimEnable(true);

            GameEndManager.Instance.GameFailUI();
        }

        ShotEnd();
    }

    private void MissingTarget()
    {
        if (isRange) return;

        Vector3 bulletVec = transform.right;
        Vector3 trumpVec = (target.transform.position - transform.position).normalized;

        var cross = Vector3.Cross(bulletVec, trumpVec);

        if (cross.y > 0)
        {
            GameEndManager.Instance.GameFailUI();
            ShotEnd();
        }
    }

    private void ShotEnd()
    {
        VCamManager.Instance.VCamToTarget();
        SetFlyVolume(false);
    }

    private void Fly()
    {
        VCamManager.Instance.VCamToBullet();

        StartCoroutine(FlyTimeSequence());
    }

    private IEnumerator FlyTimeSequence()
    {
        SetSlowMotion(1, 0.2f);
        yield return new WaitForSeconds(2f);
        SetSlowMotion(80f, 0.8f);

        SetFlyVolume(true);

        yield return null;
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

    private void SetSlowMotion(float bSpeed, float aSpeed)
    {
        bulletSpeed = bSpeed;
        rb.velocity = transform.forward * bulletSpeed;
        target.SetAnimSpeed(aSpeed);
    }

    private bool IsTargetHit(out RaycastHit point)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, 10f, hitLayer);

        float minDistance = 99f;
        bool hitTarget = false;
        point = hits[0];

        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.name);
            float currentDistance = Vector3.Distance(transform.position, hit.point);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                hitTarget = hit.collider.tag == "Trump_Body" ? true : false;

                point = hit;
            }
        }
        return hitTarget;
    }
}
