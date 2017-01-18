using UnityEngine;
using System.Collections;

public class TrashCubePart : MonoBehaviour 
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
