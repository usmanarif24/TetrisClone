using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public GameObject UI;
    public float previousTime;
    public float falltime = 0.8f;
    public static int width = 10;
    public static int height = 20;
    public Vector3 rotationPoint;
    private static Transform[,] grid = new Transform[width, height];
    private GameObject scoreText;
    private int score;
    // Update is called once per frame
    void Update()
    {
        if (ValidMove())
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(-1, 0, 0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }
            }

            if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? falltime / 10 : falltime))
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    AddToGrid();
                    CheckLines();
                    this.enabled = false;
                    FindObjectOfType<Spawning>().spawnTetrimino();
                }
                previousTime = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint( rotationPoint), new Vector3(0, 0, 1), 90);
                if (!ValidMove())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
            }   
        
        }
        else
        {
            if (!GameObject.Find("UI(Clone)"))
            {
                Instantiate(UI);
                scoreText = UI.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject;
                scoreText.GetComponent<Text>().text = "Score: Twadi khair";
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedy = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedy] = children;

        }
    }

    public bool ReachedTop()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedy = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedy < 0 || roundedy >= height)
            {
                return false;
            }


            if (grid[roundedX, roundedy] != null)
            {
                return false;
            }
                
            
        }
        return true;
    }

    public bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedy = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedy < 0 || roundedy >= height)
            {
                return false;
            }


            if (grid[roundedX, roundedy] != null)
            {
                return false;
            }
                
            
        }
        return true;
    }



    public void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLines(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }
    bool HasLines(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        ScoreHandler.Instance.UpdateScore();
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j,y] != null)
                {
                    grid[j,y-1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

}
