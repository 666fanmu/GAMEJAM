using GAMEJAM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Collections.ObjectModel;

public class SafeArea : MonoBehaviour
{
    public List<GameObject> boundary = new List<GameObject>();
    private float timer = 0;
    private int seconds = 0;
    private int beforeShrink = 30;
    public float ratio = 1;
    public bool gameOver = false;
    public Text CountDown;
    public GameObject gameManager;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }
    private void Update()
    {
        //计时，每隔一段时间触发收缩毒圈的协程
        timer += Time.deltaTime;
        if (timer >= 1f&&!gameOver)
        {
            seconds++;
            if(seconds <=30) {
                beforeShrink = 30 - seconds;
            }
            else
            {
                beforeShrink = (1000 - seconds) % 10;
            }
            CountDown.text = (beforeShrink.ToString("00"));
            timer = 0;
            if (beforeShrink == 0)
            {
                StartCoroutine(Shrink());
            }
        }
    }
    IEnumerator Shrink()
    {
        //收缩毒圈的协程，在2-5秒内逐渐减少每个方向上reduceRatio比例的安全区
        float _ratio = ratio;
        float reduceRatio = Random.Range(0.12f, 0.25f);
        if (reduceRatio < 0.15f&& _ratio<=0.8f)
        {
            reduceRatio = Random.Range(-0.15f, -0.08f);
        }
        float timer2 = 0;
        float finalTime= Random.Range(2f, 5f);
        while (timer2 < finalTime)
        {
            ratio = _ratio - timer2 * reduceRatio/ finalTime;
            Init();
            timer2 += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        ratio = _ratio - reduceRatio;
        Init();
    }
    private void Init()
    {
        //在协程中持续更新毒圈的区域
        boundary[0].transform.localScale = new Vector3(0.7f - 1f / 2 * ratio, 1, 1);
        boundary[0].transform.position = new Vector3(18 * (0.35f + ratio / 4), 0, 1);
        boundary[1].transform.localScale = new Vector3(0.7f - 1f / 2 * ratio, 1, 1);
        boundary[1].transform.position = new Vector3(18 * (-0.35f - ratio / 4), 0, 1);
    }
    public void GameOver(int Playernumber)
    {
        //由四个边界触发后调用，执行一次SettleGame后不会再执行
        if (!gameOver)
        {
            gameOver = true;
            gameManager.GetComponent<GameManager>().SettleGame(Playernumber);
            
        }
    }
}
