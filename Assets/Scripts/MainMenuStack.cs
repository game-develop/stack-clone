using UnityEngine;
using System.Collections;

public class MainMenuStack : MonoBehaviour 
{

    private const float MAX_BOUND = 6;

    private float tileTransition = 0;
    private float tileSpeed = 1.5f;
    private float startXPosition;
    private GameObject upperCube;

    public GameObject[] stack;
    
	void Start() 
    {
        stack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            stack[i] = transform.GetChild(i).gameObject;
            ColorsUtil.ColorMesh(stack[i].GetComponent<MeshFilter>().mesh, i);
        }
        upperCube = stack[0];
        startXPosition = upperCube.transform.localPosition.x;
	}
	
	void Update() 
    {
        tileTransition += Time.deltaTime * tileSpeed;
        upperCube.transform.localPosition = new Vector3(
            startXPosition + Mathf.Sin(tileTransition) * MAX_BOUND, 
            upperCube.transform.localPosition.y, 
            upperCube.transform.localPosition.z
        );
	}
}
