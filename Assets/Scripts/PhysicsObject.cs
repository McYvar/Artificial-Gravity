using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    #region Variables
    [SerializeField] bool useRotation;
    [Header("If we use rotation")]
    [SerializeField, Range(0f, 0.1f)] float rotationSpeed;

    [Space(25)]
    [SerializeField] bool useGravity = true;

    public List<PhysicsObject> physicsObject = new List<PhysicsObject>();
    static PhysicsObject priorityObject;
    [SerializeField] bool canPrioritize = false;

    Material thisMaterial;

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

        //thisMaterial.CopyPropertiesFromMaterial(GetComponent<MeshRenderer>().material);
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
                gravityDirection += gravityFormula(obj);
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
        return (rb.mass * obj.rb.mass) / Mathf.Pow(Vector3.Distance(obj.transform.position, transform.position), 2) * (obj.transform.position - transform.position).normalized;
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

}
