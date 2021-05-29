
using System.Collections.Generic;
using UnityEngine;



public class Boards : MonoBehaviour
{
    public static Boards instance;
    private int xSize, ySize;
    private Ball ballGO;
    private List<Sprite> ballSprites = new List<Sprite>();
    private bool isFindMatch = false;
    private bool isShift = false;
    public bool isGamePlayed = false;
    private Ball[,] ballArray;
    
    [SerializeField] GamePlay gamePlay;

    private void Awake()
    {
        if(instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

    }

    private void Start()
    {
        isGamePlayed = true;
        InvokeRepeating("SearchEmptyBalls", 0.1f, 0.1f);
    }
    private void Update()
    {
        if (!isGamePlayed)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !isShift)
        {
            if (PlayerPrefs.HasKey("First") == false)
            {
                gamePlay.FirstPanelOff();
                PlayerPrefs.SetString("First", "OK");
            }
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (ray)
            {
                ray.collider.gameObject.GetComponent<Ball>().spriteRenderer.sprite = null;
                gamePlay.MinusMoves();
                
            }
            
            SearchEmptyBalls();
        }
    }

    private List<Ball> FindMatch(Ball ball, Vector2 dir)
    {
        List<Ball> cachFindBalls = new List<Ball>();
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, dir);
        while(hit.collider != null && hit.collider.gameObject.GetComponent<Ball>().spriteRenderer.sprite == ball.spriteRenderer.sprite)
        {
            
            cachFindBalls.Add(hit.collider.gameObject.GetComponent<Ball>());
            hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
        }

        return cachFindBalls;
    }

    private void FindAllMatch(Ball ball)
    {
        if (ball.spriteRenderer.sprite == null)
        {
            return;
        }
        DeletBalls(ball, new Vector2[2] { Vector2.up, Vector2.down });
        DeletBalls(ball, new Vector2[2] { Vector2.left, Vector2.right });
        if (isFindMatch)
        {
            isFindMatch = false;
            ball.spriteRenderer.sprite = null;
            //isSearchEmptyBall = true;
        }

    }

    private void DeletBalls(Ball ball, Vector2[] dirArray)
    {
        List<Ball> cachFindBalls = new List<Ball>();
        for(int i = 0; i < dirArray.Length; i++)
        {
            
            cachFindBalls.AddRange(FindMatch(ball, dirArray[i]));
        }

        if(cachFindBalls.Count >= 2)
        {
            switch (cachFindBalls.Count)
            {
                case 2:
                    gamePlay.AddMoves(1);
                    gamePlay.AddScore(5);
                    break;
                case 3:
                    gamePlay.AddMoves(2);
                    gamePlay.AddScore(10);
                    break;
                case 4:
                    gamePlay.AddMoves(3);
                    gamePlay.AddScore(20);
                    break;
            }
            for (int i = 0; i < cachFindBalls.Count; i++)
            {
                cachFindBalls[i].spriteRenderer.sprite = null;
            }
            isFindMatch = true;
        }
    }

    private void SearchEmptyBalls()
    {
        
        for(int x =0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
               
                if (ballArray[x, y].spriteRenderer.sprite == null)
                {
                    
                    ShiftBallsDown(x, y);
                    break;
                }
                /*if(x == xSize-1 && y == ySize-1)
                {
                    isSearchEmptyBall = false;
                }*/
            }
        }
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                FindAllMatch(ballArray[x, y]);
            }
        }

    }
    private void ShiftBallsDown(int xPos, int yPos)
    {
        
        isShift = true;
        List<SpriteRenderer> cachRenderer = new List<SpriteRenderer>();
        for(int y = yPos; y < ySize; y++) 
        {
            Ball ball = ballArray[xPos, y];
            cachRenderer.Add(ball.spriteRenderer);
            
        }
        SetNewSprite(xPos, cachRenderer);
        
        isShift = false;
    }

    private void SetNewSprite(int xPos, List<SpriteRenderer> renderer)
    {
        if(renderer.Count == 1) renderer[0].sprite = GetNewSprite(xPos, ySize - 1);
        else
        {
            for (int y = 0; y < renderer.Count-1; y++) 
            {
                renderer[y].sprite = renderer[y + 1].sprite;
                renderer[y + 1].sprite = GetNewSprite(xPos, ySize - 1);
            }
        }
        
    }

    private Sprite GetNewSprite(int xPos, int yPos)
    {
        List<Sprite> cachSprite = new List<Sprite>();
        cachSprite.AddRange(ballSprites);
        if(xPos > 0)
        {
            cachSprite.Remove(ballArray[xPos-1,yPos].spriteRenderer.sprite);
        }
        if (xPos < xSize-1)
        {
            cachSprite.Remove(ballArray[xPos + 1, yPos].spriteRenderer.sprite);
        }
        if (yPos > 0)
        {
            cachSprite.Remove(ballArray[xPos, yPos-1].spriteRenderer.sprite);
        }
        return cachSprite[Random.Range(0, cachSprite.Count)];
    }

    public Ball[,] SetBoard(int xSize, int ySize, Ball ballGO, List<Sprite> ballSprites)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.ballGO = ballGO;
        this.ballSprites = ballSprites;
        return CreateBoard();
    }

    private Ball[,] CreateBoard()
    {
        ballArray = new Ball[xSize, ySize];
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 ballSize = ballGO.spriteRenderer.bounds.size;

        Sprite cashSprite = null;

        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                var newBall = Instantiate(ballGO, transform.position, Quaternion.identity);
                newBall.transform.position = new Vector3(xPos + (ballSize.x * x), yPos + (ballSize.y * y), 0);
                newBall.transform.parent = transform;
                ballArray[x, y] = newBall;
                List<Sprite> tempSprite = new List<Sprite>();
                tempSprite.AddRange(ballSprites);
                tempSprite.Remove(cashSprite);
                if (x > 0)
                {
                    tempSprite.Remove(ballArray[x-1, y].spriteRenderer.sprite);
                }

          
                newBall.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];
                cashSprite = newBall.spriteRenderer.sprite;
            }

            
        }
        return ballArray;
    }

}

