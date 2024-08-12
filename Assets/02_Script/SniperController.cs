using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEngine.Rendering.Universal;

public class SniperController : MonoBehaviour
{
    public event Action OnShotEvt;

    [SerializeField] private InputDataSO input;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject sniperObj;
    [SerializeField] private GameObject pointObj;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float shotAngle;

    private Camera cam;
    private bool IsAiming;
    private float rotationX;
    private float rotationY;
    private float angleX;
    private float angleY;

    private void Awake()
    {
        cam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        angleX = transform.eulerAngles.y;
        angleY = transform.eulerAngles.x;
        rotationX = angleX;
        rotationY = angleY;

        input = input.Init();
        input.OnMouseMoveEvt += SniperMove;
        input.OnMouseLeftButtonUpEvt += Shot;
        input.OnMouseRightButtonDownEvt += () => { Animing(true); };
        input.OnMouseRightButtonUpEvt += () => { Animing(false); };
    }

    private void SniperMove(Vector2 vec)
    {
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
        if (!IsAiming) return;

        OnShotEvt?.Invoke();
    }

    private void Animing(bool value)
    {
        IsAiming = value;

        pointObj.SetActive(IsAiming);
        cam.fieldOfView = IsAiming ? 5f : 60f;
        sniperObj.transform.localPosition = IsAiming ? new Vector3(0, -0.135f, 1f) : new Vector3(0.3f, -0.3f, 1f);
        if (volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = IsAiming ? 0.6f : 0f;
        }

    }
}
