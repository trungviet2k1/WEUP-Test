using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float speed = 2f;
    public float spacing = 1.5f;
    public float delayBeforeMovement = 5f;

    private GameObject[,] enemies;
    private Vector3[,] targetPositions;
    private bool allEnemiesMoving = false;
    private float timer = 0f;
    private int currentShapeIndex = 0;
    private int[] rowsByShape = { 4, 5, 5, 3 };
    private int[] columnsByShape = { 4, 7, 9, 7 };
    private bool startMoving = false;

    enum FormationShape
    {
        Square,
        Diamond,
        Triangle,
        Rectangle
    }

    FormationShape[] shapes = { FormationShape.Square, FormationShape.Diamond, FormationShape.Triangle, FormationShape.Rectangle };

    void Start()
    {
        CreateFormation();
    }

    void Update()
    {
        if (!startMoving)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeMovement)
            {
                startMoving = true;
                allEnemiesMoving = true;
                timer = 0f;
            }
        }
        else if (allEnemiesMoving)
        {
            MoveAllEnemies();
        }

        CheckAllEnemiesArrived();
    }

    void CreateFormation()
    {
        int rows = rowsByShape[(int)shapes[currentShapeIndex]];
        int columns = columnsByShape[(int)shapes[currentShapeIndex]];

        enemies = new GameObject[rows, columns];
        targetPositions = new Vector3[rows, columns];

        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float startX = -(columns - 1) * spacing / 2f;
        float startY = screenHeight / 2f + rows * spacing;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float randomX = Random.Range(-screenWidth / 2f, screenWidth / 2f);
                float randomY = Random.Range(startY, startY + screenHeight / 2f);
                Vector3 startPos = new(randomX, randomY, 0f);
                GameObject enemy = Instantiate(enemyPrefab, startPos, Quaternion.identity);
                enemies[row, col] = enemy;

                Vector3 targetPosition = new(-(columns - 1) * spacing / 2f + col * spacing,
                                                     Camera.main.transform.position.y + Camera.main.orthographicSize / 2f - row * spacing,
                                                     0f);
                targetPositions[row, col] = targetPosition;
            }
        }
    }

    void MoveAllEnemies()
    {
        int rows = rowsByShape[(int)shapes[currentShapeIndex]];
        int columns = columnsByShape[(int)shapes[currentShapeIndex]];

        bool allEnemiesArrived = true;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                enemies[row, col].transform.position = Vector3.MoveTowards(enemies[row, col].transform.position,
                                                                           targetPositions[row, col],
                                                                           speed * Time.deltaTime);

                if (enemies[row, col].transform.position == targetPositions[row, col])
                {
                    enemies[row, col].GetComponent<BoxCollider2D>().enabled = true;
                }
                else
                {
                    allEnemiesArrived = false;
                }
            }
        }

        if (allEnemiesArrived)
        {
            allEnemiesMoving = false;
        }
    }

    void CheckAllEnemiesArrived()
    {
        int rows = rowsByShape[(int)shapes[currentShapeIndex]];
        int columns = columnsByShape[(int)shapes[currentShapeIndex]];

        bool allArrived = true;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (enemies[row, col] != null && enemies[row, col].transform.position != targetPositions[row, col])
                {
                    allArrived = false;
                    break;
                }
            }
        }

        FindObjectOfType<PlayerMovement>().UpdateAllEnemiesArrived(allArrived);
    }
}