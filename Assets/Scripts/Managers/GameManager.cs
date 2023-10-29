using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GAMEJAM
{
    public class GameManager:MonoBehaviour
    {
        public static GameManager _instance;

        public static GameManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public GameObject Settlement1;
        public GameObject Settlement2;

        public AudioClip DeadAudio;

        private void Awake()
        {
            
        }

        /// <summary>
        /// 结算游戏
        /// </summary>
        /// <param name="playernum">人物编号</param>
        public void SettleGame(int playernum)
        {
           
           
            AudioManager.instance.AudioPlay(DeadAudio);
            if (playernum==2)
            {
                Settlement1.SetActive(true);
            }
            else
            {
                Settlement2.SetActive(true);
            }
            
        }
        


        public void OpenScene_Start()
        {
            SceneManager.LoadScene("Start");
            Time.timeScale = 1;
        }

        public void OpenScene_Main()
        {
            SceneManager.LoadScene("Main");
            Time.timeScale = 1;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

        }
        
        
        
    }
}