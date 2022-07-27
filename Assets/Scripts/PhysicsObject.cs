using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    #region Variables
    [SerializeField] bool useRotation;
    [SerializeField] bool useGravity = true;

    public List<PhysicsObject> physicsObject = new List<PhysicsObject>();

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
    }


    void FixedUpdate()
    {
        if (useGravity)
        rb.AddForce(gravityDirection);
    }


    void Update()
    {
        gravityDirection = Vector3.zero;
        if (physicsObject.Count > 0)
        {
            foreach (PhysicsObject obj in physicsObject)
            {
                gravityDirection += (rb.mass * obj.rb.mass) / Mathf.Pow(Vector3.Distance(obj.transform.position, transform.position), 2) * (obj.transform.position - transform.position).normalized;
            }
        }

        if (useRotation)
        {
            transform.rotation = Quaternion.LookRotation(gravityDirection) * Quaternion.Euler(-90, 0, 0);
        }

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
}
