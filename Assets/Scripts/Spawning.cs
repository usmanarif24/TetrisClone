using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public GameObject[] tetriminos;

    // Start is called before the first frame update
    void Start()
    {
        spawnTetrimino();
    }

    public void spawnTetrimino()
    {
        GameObject tet = Instantiate(tetriminos[Random.Range(0, tetriminos.Length)], transform.position, Quaternion.identity);
    }
}
