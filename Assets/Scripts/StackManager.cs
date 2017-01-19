using UnityEngine;
using System.Collections;

public class StackManager : MonoBehaviour 
{

    private const float ERROR_MARGIN = 0.5f;
    private const float MAX_BOUND = 6;
    private const float BOUND_GAIN = 0.25f;

    private bool endGame = false;

    public GameObject[] stack;
    private int stackIndex;
    private int scoreCount = 0;

    // camera
    private Vector3 desiredPosition;
    public float stackMovingSpeed = 2f;

    // move tile
    private float tileTransition = 0;
    private float tileSpeed = 1.5f;
    private bool flag = false;
    private float secondaryPosition;

    // section coupage
    private Vector3 lastTilePosition;
    private float combo = 0;
    private Vector2 currentBounds = new Vector2(MAX_BOUND, MAX_BOUND);

    public Color32[] stackColors = new Color32[4];
    public Material stackMat;

    // UI
    public GameObject canvas;

	void Start() 
    {
        stack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            stack[i] = transform.GetChild(i).gameObject;
            ColorsUtil.ColorMesh(stack[i].GetComponent<MeshFilter>().mesh, i);
        }
        canvas.SetActive(false);
	}
	
	void Update() 
    {
        if (Input.anyKeyDown && !endGame)
        {
            if (PlaceTile())
            {
                SpawnTile();
                ++scoreCount;
                flag = !flag;
            }
            else
            {
                EndGame();
            }
        }

        MoveTile();

        transform.position = Vector3.Lerp(transform.position, desiredPosition, stackMovingSpeed * Time.deltaTime);
	}

    private void SpawnTile()
    {
        lastTilePosition = stack[stackIndex].transform.position;
        --stackIndex;
        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }
        stack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        stack[stackIndex].transform.localScale = new Vector3(currentBounds.x, 1, currentBounds.y);

        desiredPosition = Vector3.down * scoreCount;

        ColorsUtil.ColorMesh(stack[stackIndex].GetComponent<MeshFilter>().mesh, scoreCount);
    }

    private void MoveTile()
    {
        if (endGame)
            return;

        tileTransition += Time.deltaTime * tileSpeed;
        if (flag)
        {
            stack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * MAX_BOUND, scoreCount, secondaryPosition);
        }
        else
        {
            stack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * MAX_BOUND);
        }

    }

    private bool PlaceTile()
    {
        Transform current = stack[stackIndex].transform;

        if (flag)
        {
            float deltaX = lastTilePosition.x - current.position.x;
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                combo = 0;
                currentBounds.x -= Mathf.Abs(deltaX);
                if (currentBounds.x <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.x + current.localPosition.x / 2;
                current.localScale = new Vector3(currentBounds.x, 1, currentBounds.y);
                CreateCube(
                        new Vector3(current.position.x > 0 
                            ? current.position.x + current.localScale.x/2
                            : current.position.x - current.localScale.x/2
                            , current.position.y
                            , current.position.z)
                        , new Vector3(Mathf.Abs(deltaX), 1, current.localScale.z)
                );
                current.localPosition = new Vector3(middle - lastTilePosition.x/2, scoreCount, lastTilePosition.z);
            }
            else
            {
                if (combo >= 5)
                {
                    currentBounds.x += BOUND_GAIN;
                    if (currentBounds.x > MAX_BOUND)
                    {
                        currentBounds.x = MAX_BOUND;
                    }
                    float middle = lastTilePosition.x + current.localPosition.x / 2;
                    current.localScale = new Vector3(currentBounds.x, 1, currentBounds.y);
                    current.localPosition = new Vector3(middle - lastTilePosition.x / 2, scoreCount, lastTilePosition.z);
                }
                ++combo;
                current.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }   
        else
        {
            float deltaZ = lastTilePosition.z - current.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                combo = 0;
                currentBounds.y -= Mathf.Abs(deltaZ);
                if (currentBounds.y <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.z + current.localPosition.z / 2;
                current.localScale = new Vector3(currentBounds.x, 1, currentBounds.y);
                CreateCube(
                        new Vector3(
                            current.position.x
                            , current.position.y
                            , current.position.z > 0
                            ? current.position.z + current.localScale.z / 2
                            : current.position.z - current.localScale.z / 2)
                        , new Vector3(current.localScale.x, 1, Mathf.Abs(deltaZ))
                );
                current.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - lastTilePosition.z / 2);
            }
            else
            {
                if (combo >= 5)
                {
                    currentBounds.y += BOUND_GAIN;
                    if (currentBounds.y > MAX_BOUND)
                    {
                        currentBounds.y = MAX_BOUND;
                    }
                    float middle = lastTilePosition.z + current.localPosition.z / 2;
                    current.localScale = new Vector3(currentBounds.x, 1, currentBounds.y);
                    current.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - lastTilePosition.z / 2);
                }
                ++combo;
                current.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }

        secondaryPosition = flag ? current.localPosition.x : current.localPosition.z;

        return true;
    }

    private void EndGame()
    {
        endGame = true;
        stack[stackIndex].AddComponent<Rigidbody>();
        canvas.SetActive(true);
    }

    private void CreateCube(Vector3 position, Vector3 scale)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = scale;
        cube.transform.localPosition = position;
        cube.GetComponent<MeshRenderer>().material = stackMat;
        ColorsUtil.ColorMesh(cube.GetComponent<MeshFilter>().mesh, scoreCount);
        cube.AddComponent<Rigidbody>();
        cube.AddComponent("TrashCubePart");
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void BackToMainMenu()
    {
        Application.LoadLevel("main_menu");
    }
}
