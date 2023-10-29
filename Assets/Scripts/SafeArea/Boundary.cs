using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public GameObject SafeArea;
    void OnTriggerEnter2D(Collider2D other)
    {
        //接触毒圈触发游戏结束效果
        if (other.gameObject.GetComponent<Function>() != null)
        {
            Debug.Log(other.gameObject.GetComponent<Function>().Playernumber);
            SafeArea.GetComponent<SafeArea>().GameOver(other.gameObject.GetComponent<Function>().Playernumber);
            Destroy(other.gameObject);
        }
    }
}
