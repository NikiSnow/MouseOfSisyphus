//SF=safe platorm
//V=vertical
//G=gorizontal
//DF=down first
//TF=Top first
//SM=Second middle
//B7=below (down with one gorizontal line)
//U7=upper (top with one vertical line)

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using static UnityEditor.Progress;
using System.IO;
using static UnityEngine.GraphicsBuffer;


public class GameManager : MonoBehaviour
{
    public GameObject labr;
    public string letterLine="";
    public string letterSF = "S";
    public int num = 0;
    public int num2 = 0;
    List<string> path = new List<string>() {"V", "V", "V", "G", "G", "G", "G", "G"};
    List<string> points = new List<string> { };
    int r = 0;
    public GameObject Mo;
    public float stepSize = 0.1f; // Размер шага (в метрах)
    public float speed = 1.0f; // Скорость движения (в метрах в секунду)
    private bool isMoving = false; // Флаг для отслеживания движения
    bool ShoudMove;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ShoudMove && !isMoving)
        {
            StartCoroutine(MoveAlongPath());
        }
    }
    public void restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void clear()
    {

    }
    public void Generate()
    {
        path = new List<string>{"V","V","V","G","G","G","G","G"};
        points = new List<string> {};
        Shuffle(path);
        string meow="";
        foreach (string item in path)
        {
            meow = meow + item;
        }
        Debug.Log(meow);
        meow = "";
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] == "V")
            {
                if (i == 0)
                {
                    points.Add("M");
                    continue;
                }
                else if (points[i - 1] == "M")
                {
                    points.Add("D");
                    continue;
                }
                else if (points[i - 1] == "D")
                {
                    points.Add("B");
                    continue;
                }
                else if (points[i-1]== "T")
                {
                    points.Add("M");
                    continue;
                }
            }
            else if (path[i] == "G")
            {
                if (i == 0)
                {
                    points.Add("T");
                    continue;
                }
                else if (points[i - 1] == "T")
                {
                    points.Add("T");
                    continue;
                }
                else if (points[i - 1] == "M")
                {
                    points.Add("M");
                    continue;
                }
                else if (points[i - 1] == "D")
                {
                    points.Add("D");
                    continue;
                }
                else if (points[i - 1] == "B")
                {
                    points.Add("B");
                    continue;
                }
            }

        }
        foreach (string item in points)
        {
            meow = meow + item;
        }
        Debug.Log(meow);
        num = 0;
        num2 = 0;
        for (int i = 0; i < labr.transform.childCount; i++)
        {
            // Получаем ссылку на очередной дочерний объект
            GameObject childObject = labr.transform.GetChild(i).gameObject;

            // Делаем что-то с дочерним объектом, например, выводим его имя
            //Debug.Log("Child Object Name: " + childObject.name);
            letterLine = path[num];
            if (num != 7)
            {
                num++;
            }
            TouchChildren(childObject);
        }
    }
    public void TouchChildren(GameObject child)
    {
        //Debug.Log("MeowStart");
        int randomNumber = UnityEngine.Random.Range(1, 5);
        for (int i = 0; i < child.transform.childCount; i++)
        {
            // Получаем ссылку на очередной дочерний объект
            GameObject childObject = child.transform.GetChild(i).gameObject;
            Debug.Log("meow777");
            if (childObject.name.EndsWith("SF"))
            {
                r = UnityEngine.Random.Range(0, 4);
            }
            if (childObject.name.EndsWith("SF")&& childObject.name.StartsWith("S"))
            {
                Debug.Log("Sf S");
                r = UnityEngine.Random.Range(0, 3);
                Debug.Log(r);
            }
            else if (childObject.name.EndsWith("V"))
            {
                //Debug.Log("V1");
                childObject.active = false;
                if (r == 0 || r == 2)
                {
                    childObject.active = true;

                }

                if (points[num2] == "M"&&num2==0)
                {
                    Debug.Log(points[num2] +" "+ num2 + " " + num);
                    childObject.active = true;
                }

                if (letterLine == "V" && childObject.name.StartsWith(points[num2]))
                {
                    childObject.active = true;
                    num2++;
                }
            }
            else if (childObject.name.EndsWith("G"))
            {
                //Debug.Log("G1");
                childObject.active = false;
                if (r == 1 || r == 2)
                {
                    childObject.active = true;

                }
                if (points[num2] == "T" && num2 == 0)
                {
                    Debug.Log(points[num2] +" "+ num2+" "+num);
                    childObject.active = true;
                }
                if (letterLine == "G" && childObject.name.StartsWith(points[num2]))
                {
                    childObject.active = true;
                    num2++;
                }

            }


            //дочерний объект выводим имя
            //Debug.Log("Child Object Name: " + childObject.name);
        }

    }
    public void FindExit()
    {
        ShoudMove = true;
    }

    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            System.Random rng = new System.Random();
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    private IEnumerator MoveAlongPath()
    {
        Mo.transform.position = new Vector2(-8.05f, 4.25f);
        Mo.transform.rotation = Quaternion.Euler(0, 0, 0);
        isMoving = true;

        foreach (string direction in path)
        {
            Vector3 targetPosition = Mo.transform.position; // Используем позицию объекта Mo
            Quaternion targetRotation = Mo.transform.rotation;

            switch (direction)
            {
                case "G":
                    targetPosition += Vector3.right * stepSize;
                    targetRotation = Quaternion.Euler(0, 0, 90); // Поворот направо
                    break;
                case "V":
                    targetPosition += Vector3.down * stepSize;
                    targetRotation = Quaternion.Euler(0, 0, 0); // Поворот вниз
                    break;
                    // Можно добавить другие направления при необходимости
            }

            // Двигаем объект Mo к целевой позиции
            while (Vector3.Distance(Mo.transform.position, targetPosition) > 0.01f)
            {
                Mo.transform.position = Vector3.MoveTowards(Mo.transform.position, targetPosition, speed * Time.deltaTime);
                Mo.transform.rotation = Quaternion.RotateTowards(Mo.transform.rotation, targetRotation, speed * Time.deltaTime * 100); // Плавный поворот
                yield return null; // Ждем один кадр
            }

            // Устанавливаем точную целевую позицию и ротацию, чтобы избежать неточностей
            Mo.transform.position = targetPosition;
            Mo.transform.rotation = targetRotation;
            yield return new WaitForSeconds(0.1f); // Задержка перед следующим шагом (опционально)
        }

        isMoving = false;
        ShoudMove = false;
    }
}
