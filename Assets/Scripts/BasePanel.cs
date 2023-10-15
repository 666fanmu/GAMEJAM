using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAMEJAM
{
    public class BasePanel : MonoBehaviour
    {
        protected bool isREmove=false;
        protected new string name;

        public virtual void OpenPanel(string name)
        {
            this.name = name;
            this.gameObject.SetActive(true);
        }

        public virtual void ClosePanel()
        {
            isREmove = false;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
