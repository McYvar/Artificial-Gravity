using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    #region
    // Variables
    public bool useGravity = true;

    // If we use gravity
    public float gravityStrenght;

    public bool useRegion = false;
    // If we use regions
    public float regionSize = 1;
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (useRegion)
        {
            Gizmos.color = new Color(252/255f, 227/255f, 3/255f, 1);
            Gizmos.DrawWireSphere(transform.position, regionSize);
        }
    }
}
