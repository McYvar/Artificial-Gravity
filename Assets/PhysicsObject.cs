using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    #region
    // Variables
    public static List<GravityObject> gravityObjects = new List<GravityObject>();

    Rigidbody rb;

    GravityObject selector;

    [SerializeField] bool useRotation;
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

        gravityObjects.Clear();

        GravityObject[] temp = FindObjectsOfType<GravityObject>();
        foreach (GravityObject obj in temp)
        {
            if (obj.gameObject == this.gameObject) continue;
            gravityObjects.Add(obj);
        }

        if (gravityObjects.Count > 0)
        {
            selector = gravityObjects[0];
        }
    }


    void FixedUpdate()
    {
        rb.AddForce(gravityDirection * selector.gravityStrenght);
    }


    void Update()
    {
        if (gravityObjects.Count > 0)
        {
            foreach (GravityObject obj in gravityObjects)
            {
                float distanceToCurrentObj = Vector3.Distance(transform.position, obj.transform.position);
                float distanceToSelectorObj = Vector3.Distance(transform.position, selector.transform.position);

                if (!obj.useGravity) return;

                if (obj.useRegion)
                {
                    if (distanceToCurrentObj > obj.regionSize)
                    {
                        return;
                    }
                }

                if (distanceToCurrentObj < distanceToSelectorObj)
                {
                    selector = obj;
                }
            }
        }
        gravityDirection = (selector.transform.position - transform.position).normalized;

        if (useRotation)
        {
            transform.rotation = Quaternion.LookRotation(gravityDirection) * Quaternion.Euler(-90, 0, 0);
        }

        Debug.Log(gravityObjects.Count);
    }


    private void OnDrawGizmos()
    {
        // Gravity line
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, gravityDirection.normalized * selector.gravityStrenght);
        }
        else
        {

        }
    }
}
