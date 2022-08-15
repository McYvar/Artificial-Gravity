using System.Collections.Generic;
using UnityEngine;

public enum PhysicsType { point, directional };

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    #region Variables
    public PhysicsType physicsType;

    bool useRotation;
    [Header("If we use rotation")]
    [SerializeField, Range(0f, 0.1f)] float rotationSpeed;

    [Space(25)]
    [SerializeField] bool useGravity = true;

    public List<PhysicsObject> physicsObject = new List<PhysicsObject>();
    static PhysicsObject priorityObject;
    [SerializeField] bool canPrioritize = false;

    [Space(25), Header("In case of directional graviy")]
    [SerializeField] Vector3 gravityFieldPointA;
    [SerializeField] Vector3 gravityFieldPointB;
    Vector3 gravityField;
    [SerializeField] float gravityStrength;

    Rigidbody rb;

    Vector3 gravityDirection;
    #endregion


    void Start()
    {
        gravityDirection = Vector3.zero;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (useRotation)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        physicsObject.Clear();

        PhysicsObject[] temp = FindObjectsOfType<PhysicsObject>();
        foreach (PhysicsObject obj in temp)
        {
            if (obj.gameObject == this.gameObject) continue;
            physicsObject.Add(obj);
        }

        if (!useGravity)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
        }

        gravityField = transform.rotation * ((transform.position + gravityFieldPointB) - (transform.position + gravityFieldPointA));
    }


    void FixedUpdate()
    {
        if (useGravity)
            rb.AddForce(gravityDirection);
    }


    void Update()
    {
        if (useRotation)
        {
            Quaternion targetRotation = Quaternion.LookRotation(gravityDirection) * Quaternion.Euler(-90, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
        gravityDirection = Vector3.zero;
        if (priorityObject != null)
        {
            gravityDirection = gravityFormula(priorityObject);
        }
        else if (physicsObject.Count > 0)
        {
            foreach (PhysicsObject obj in physicsObject)
            {
                switch (obj.physicsType)
                {
                    case PhysicsType.point:
                        gravityDirection += gravityFormula(obj);
                        break;

                    case PhysicsType.directional:

                        break;
                }
            }
        }

        if (priorityObject == this)
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        }
        else
        {
            GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }
    }


    Vector3 gravityFormula(PhysicsObject obj)
    {
        return rb.mass * obj.rb.mass / Mathf.Pow(Vector3.Distance(obj.transform.position, transform.position), 2) * (obj.transform.position - transform.position).normalized;
    }

    private void OnDrawGizmos()
    {
        // Gravity line
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
        }
        else
        {

        }
    }

    private void OnMouseDown()
    {
        if (!canPrioritize) return;

        if (priorityObject == null)
        {
            priorityObject = this;
        }
        else if (priorityObject != this)
        {
            priorityObject = this;
        }
        else
        {
            priorityObject = null;
        }
    }


    private void OnDrawGizmosSelected()
    {
        switch (physicsType)
        {
            case PhysicsType.point:
                break;
            case PhysicsType.directional:
                Gizmos.color = new Color(0, 0, 1f, 1f);
                Vector3 A = transform.rotation * (transform.position + gravityFieldPointA);
                Vector3 B = transform.rotation * (transform.position + gravityFieldPointB);
                Vector3 Center = A + (B - A) / 2;
                Vector3 Size = B - A;
                Gizmos.DrawSphere(A, 0.3f);
                Gizmos.DrawSphere(B, 0.3f);
                Gizmos.DrawLine(A, B);
                Gizmos.DrawSphere(Center, 0.3f);
                Gizmos.color = new Color(0f, 0f, 0f, 0.4f);
                Gizmos.DrawCube(Center, Size);
                break;
        }
    }
}
