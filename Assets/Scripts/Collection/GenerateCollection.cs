using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCollection : MonoBehaviour
{
    public List<GameObject> collections = new List<GameObject>();
    private float timer = 0;
    private int seconds = 0;
    public int generateInterval;
    public GameObject SafeArea;
    private GameObject AllCollections;
    private void Start()
    {
        //游戏开始时生成空物体用来存放生成的拾取物,设置种子并生成6个拾取物
        AllCollections = new GameObject("AllCollections");
        Random.InitState(System.DateTime.Now.Millisecond);
        GenerateMoreCollection(6);
    }
    private void Update()
    {
        //计时，每隔一段时间生成3到5个拾取物
        timer += Time.deltaTime;
        if(timer>=1f&&!SafeArea.GetComponent<SafeArea>().gameOver)
        {
            seconds++;
            timer = 0;
            if (seconds % generateInterval == 0)
            {
                GenerateMoreCollection(Random.Range(3,6));
            }
        }
    }
    public void GenerateMoreCollection(int num)
    {
        
        float _ratio = SafeArea.GetComponent<SafeArea>().ratio;
        //在一定范围内生成多次拾取物
        for (int i = 0; i < num; i++)
        {
            GameObject Object;
            Object = Instantiate(collections[Random.Range(0, collections.Count)], new Vector3(Random.Range(-7.2f, 7.2f),
                Random.Range(-4 * _ratio, 4 * _ratio)), Quaternion.identity);
            Object.transform.SetParent(AllCollections.transform);
        }
        
    }

}
