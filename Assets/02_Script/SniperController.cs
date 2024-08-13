using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using System;
using UnityEngine.Rendering.Universal;

public class SniperController : MonoBehaviour
{
    public event Action OnShotEvt;

    [Header("Input")]
    [SerializeField] private InputDataSO input;
    [Header("Object")]
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject sniperObj;
    [SerializeField] private GameObject pointObj;
    [Header("Value")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float shotAngle;

    private bool IsAiming;
    private bool IsShotting;

    private float rotationX;
    private float rotationY;
    private float angleX;
    private float angleY;

    private void Awake()
    {
        input = input.Init();

        input.OnMouseMoveEvt += SniperMove;
        input.OnMouseLeftButtonUpEvt += Shot;
        input.OnMouseRightButtonDownEvt += () => { Animing(true); };
        input.OnMouseRightButtonUpEvt += () => { Animing(false); };
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        angleX = transform.eulerAngles.y;
        angleY = transform.eulerAngles.x;
        rotationX = angleX;
        rotationY = angleY;

        VCamManager.Instance.VCamToSniper();
    }

    private void SniperMove(Vector2 vec)
    {
        if (IsShotting) return;

        float speed = IsAiming ? aimSpeed : moveSpeed;
        float mouseX = vec.x * speed * Time.deltaTime;
        float mouseY = vec.y * speed * Time.deltaTime;

        rotationX += mouseX;
        rotationY -= mouseY;

        rotationX = Mathf.Clamp(rotationX, angleX - shotAngle, angleX + shotAngle);
        rotationY = Mathf.Clamp(rotationY, angleY - shotAngle, angleY + shotAngle);

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
    }

    private void Shot()
    {
        if (!IsAiming || IsShotting) return;

        IsShotting = true;
        Animing(false);

        OnShotEvt?.Invoke();
    }

    private void Animing(bool value)
    {
        IsAiming = IsShotting ? false : value;

        pointObj.SetActive(IsAiming);
        vcam.m_Lens.FieldOfView = IsAiming ? 5f : 60f;

        sniperObj.transform.localPosition = IsAiming || IsShotting 
            ? new Vector3(0, -0.135f, 1f) : new Vector3(0.3f, -0.3f, 1f);
        
        if (volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = IsAiming ? 0.6f : 0f;
        }
    }
}
