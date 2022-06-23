using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianShield : MonoBehaviour
{
    public float Speed;
    private Guardian guardian;
    [SerializeField]
    private new Collider2D collider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        
    }

    void Update()
    {
        if (guardian != null)
        {
            transform.RotateAround(guardian.transform.position, Vector3.back, Speed * Time.deltaTime);
        }
    }

    public void SetGuardian(Guardian target) => guardian = target;

    public void SetActiveStatus(bool status)
    {
        collider.enabled = status;
        spriteRenderer.enabled = status;
    }

}
