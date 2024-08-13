using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Target : MonoBehaviour
{
    [SerializeField] private int danceIdx;
    private Animator anim;

    readonly string ParameterName = "DanceIdx";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetFloat(ParameterName, danceIdx);
    }

    public void SetAnimSpeed(float value)
    {
        anim.speed = value;
    }

    public void SetAnimEnable(bool value)
    {
        anim.enabled = value;
    }
}
