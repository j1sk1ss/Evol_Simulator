using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationScript : MonoBehaviour
{
    public GameObject Cell, Eat;
    public int Cells, FirstEat;
    public Vector3 CellPos, EatPos;
    public bool StartCells = true;
    private void Start()
    {
        StartCoroutine(EatGeneration());
    }
    void Update()
    {
        if (Cells <= 5 && FirstEat >= 12)
        {
            Cells++;
            CellPos = new Vector3(Random.RandomRange(-205, 205), Random.RandomRange(-190, 190), -5);
            GameObject a = Instantiate(Cell, CellPos, Quaternion.identity) as GameObject;
            a.transform.SetParent(GameObject.FindGameObjectWithTag("FONK").transform,false);
        } else
        {
            StartCells = false;
        }
        if (FirstEat <= 12)
        {
            FirstEat++;
            EatPos = new Vector3(Random.RandomRange(-355, 355), Random.RandomRange(-190, 190), -5);
            GameObject b = Instantiate(Eat, EatPos, Quaternion.identity) as GameObject;
            b.transform.SetParent(GameObject.FindGameObjectWithTag("FONK").transform, false);
        }
    }
    IEnumerator EatGeneration()
    {
        yield return new WaitForSeconds(0.1f);
        EatPos = new Vector3(Random.RandomRange(-355, 355), Random.RandomRange(-190, 190), -5);
        GameObject b = Instantiate(Eat, EatPos, Quaternion.identity) as GameObject;
        b.transform.SetParent(GameObject.FindGameObjectWithTag("FONK").transform, false);
        StartCoroutine(EatGeneration());

    }
}
