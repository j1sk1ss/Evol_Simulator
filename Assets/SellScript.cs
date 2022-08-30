using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SellScript : MonoBehaviour
{
    public GameObject[] Eat;
    public GameObject closest, ConScript;
    public int Damage, Hp, Pregant, MaxHeal;
    public float Speed, Hungry = 20f;
    public Vector3 NewPosition;
    new Color myColor;
    public int id;
    public bool MeetEater;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SellScript>().id != id && collision.gameObject.GetComponent<SellScript>().id != 0 && (Random.Range(1,100) < 96 && !MeetEater))
        {
            switch(MeetEater)
            {
                case (true):
                    {
                        collision.gameObject.GetComponent<SellScript>().Hp -= Damage;
                        Hp -= collision.gameObject.GetComponent<SellScript>().Damage;
                        if (collision.gameObject.GetComponent<SellScript>().Hp <= 0)
                        {
                            Hungry += 60;
                            Hp += MaxHeal / 2;
                        }
                        break;
                    }
                case (false):
                    {
                        collision.gameObject.GetComponent<SellScript>().Hp -= Damage;
                        Hp -= collision.gameObject.GetComponent<SellScript>().Damage;
                        break;
                    }
            }
        } else if (collision.gameObject.GetComponent<SellScript>().id != id && collision.gameObject.GetComponent<SellScript>().id != 0 && (Random.Range(1, 100) > 96 && !MeetEater))
        {
            collision.gameObject.GetComponent<SellScript>().Hungry -= collision.gameObject.GetComponent<SellScript>().Pregant;
            Hungry -= Pregant;
            GameObject a = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            a.transform.SetParent(GameObject.FindGameObjectWithTag("FONK").transform, false);
            a.transform.position = this.gameObject.transform.position;
                a.GetComponent<SellScript>().MaxHeal = (collision.gameObject.GetComponent<SellScript>().MaxHeal + MaxHeal) / 2;
                a.GetComponent<SellScript>().Hp = (collision.gameObject.GetComponent<SellScript>().Hp + Hp) / 2;
                a.GetComponent<SellScript>().Pregant = (collision.gameObject.GetComponent<SellScript>().Pregant + Pregant) / 2;
                a.GetComponent<SellScript>().Damage = (collision.gameObject.GetComponent<SellScript>().Damage + Damage) / 2;
                a.GetComponent<SellScript>().Speed = (collision.gameObject.GetComponent<SellScript>().Speed + Speed) / 2;
                a.GetComponent<SellScript>().MeetEater = MeetEater;
                a.GetComponent<SellScript>().id = id;
                a.GetComponent<Image>().color = this.gameObject.GetComponent<Image>().color;
        }
    }
    void Start()
    {
        if (Hp <= 0)
        {
            id = Random.RandomRange(1000000, 9999999); 
        Hungry = 40f;
        Hp = Random.Range(500, 1000);
            MaxHeal = Hp;
            Pregant = Random.Range(30, 100);
        Damage = Random.Range(1, 100);
        Speed = Random.Range(1.0f, 4.0f);
        myColor = new Color(Random.Range(0.0f, 1.0f), 1, Random.Range(0.0f, 1.0f), 1);
        this.gameObject.GetComponent<Image>().color = myColor;
        }
        if ((Damage > 50 && MaxHeal > 500) && Random.Range(1,100) > 80)
        {
            Pregant = Random.Range(60, 100);
            MeetEater = true;
            Speed += 2.0f;
        }
        StartCoroutine(MindOFCesll());
    }
    IEnumerator MindOFCesll()
    {
        switch(MeetEater)
        {
            case (false):
                {
                    Eat = GameObject.FindGameObjectsWithTag("EAT");
                    FindEat();
                    break;
                }
            case (true):
                {
                    Eat = GameObject.FindGameObjectsWithTag("CELL");
                    FindEatCell();
                    break;
                }
        }
        Hungry -= 1.7f;
        if (Hp < MaxHeal)
        {
            Hp += 20;
        }
        if (Hungry > 0 && Hp > 0)
        {
            
            if (closest && !MeetEater)
            {
            NewPosition = closest.transform.position;
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(MindOFCesll());
        } else if (Hungry <= 0)
        {
            if (closest)
            {
                NewPosition = closest.transform.position;
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(MindOFCesll());
            Hp -= 50;
            Hungry = 0;
        } 
        if (Hungry > Pregant + 10 && Hp > 0)
        {
            Hungry -= Pregant - 10;
            GameObject a = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            a.transform.SetParent(GameObject.FindGameObjectWithTag("FONK").transform, false);
            a.transform.position = this.gameObject.transform.position;
            if (Random.Range(1, 100) > 80)
            {
                a.GetComponent<SellScript>().Hp = 0;
            } else
            {
                a.GetComponent<SellScript>().MaxHeal = MaxHeal;
                a.GetComponent<SellScript>().Hp = Hp;
                a.GetComponent<SellScript>().Pregant = Pregant;
                a.GetComponent<SellScript>().Damage = Damage;
                a.GetComponent<SellScript>().Speed = Speed;
                a.GetComponent<SellScript>().MeetEater = MeetEater;
                a.GetComponent<SellScript>().id = id;
                a.GetComponent<Image>().color = this.gameObject.GetComponent<Image>().color;
            }
        }
        if (Hp < 0)
        {
            myColor = new Color(190, 190, 255, 255);
            id = 0;
            this.gameObject.GetComponent<Image>().color = myColor;
            yield return new WaitForSeconds(0.1f);
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, NewPosition, Time.deltaTime * Speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hungry += 3f;
        Destroy(collision.gameObject);
    }
    
    GameObject FindEat()
    {
        float distance = Mathf.Infinity;
        Vector3 position = this.gameObject.transform.position;
        foreach (GameObject go in Eat)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    GameObject FindEatCell()
    {
        float distance = Mathf.Infinity;
        Vector3 position = this.gameObject.transform.position;
        for (int i = 0; i < Eat.Length; i++)
        {
            if (Eat[i].GetComponent<SellScript>().id != id)
            {
                print(Eat[i].transform.position);
                Vector3 diff = Eat[i].transform.position - position;
                float curDistance = diff.sqrMagnitude;
                    closest = Eat[i];
                    distance = curDistance;
            }
        }
        return closest;
    }
}

