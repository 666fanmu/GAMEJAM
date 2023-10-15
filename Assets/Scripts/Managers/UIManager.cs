using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAMEJAM
{
    public class UIManager:MonoBehaviour
    {
        private static UIManager _instance;

        public static UIManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }
                return _instance;
            }
        }
        
        
        
    }
}
