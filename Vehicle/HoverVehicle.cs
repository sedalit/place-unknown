using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverVehicle : Vehicle
{
    [SerializeField] private float thrustForward;
    [SerializeField] private float thrustTorque;
    [SerializeField] private float dragLinear;
    [SerializeField] private float dragAngular;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float hoverForce;
    [SerializeField] private float maxLinearSpeed;
    [SerializeField] private Transform[] hoverJets;

    private Rigidbody rigidbody;
    private bool isGrounded;

    protected override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() => CalculateForce();

    private void CalculateForce()
    {
        isGrounded = false;
        foreach (var jet in hoverJets)
        {
            if (ApplyJetForce(jet))
            {
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            rigidbody.AddRelativeForce(-Vector3.right * thrustForward * TargetInputControl.z);
            rigidbody.AddRelativeTorque(Vector3.up * thrustTorque * TargetInputControl.x);
        }
        float dragFactor = thrustForward / maxLinearSpeed;
        Vector3 linearDragForce = -rigidbody.velocity * dragFactor;
        if (isGrounded)
        {
            rigidbody.AddForce(linearDragForce, ForceMode.Acceleration);
        }
        Vector3 angularDragForce = -rigidbody.angularVelocity * dragAngular;
        rigidbody.AddTorque(angularDragForce, ForceMode.Acceleration);
    }

    public bool ApplyJetForce(Transform target)
    {
        Ray ray = new Ray(target.position, -target.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float s = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 force = s * hoverForce * hit.normal;
            rigidbody.AddForceAtPosition(force, target.position, ForceMode.Acceleration);
            return true;
        }
        return false;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Freeze();
    }

    public override void Freeze()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void Unfreeze()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.None;
    }

}
