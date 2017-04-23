using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace src{
    public class handleswitch : MonoBehaviour
    {

        public MultilineInputField code;
        public MultilineInputField terminal;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (code.gameObject.Equals(EventSystem.current.currentSelectedGameObject))
                {
                    terminal.Select();
                } else
                {
                    code.Select();
                }
            }
        }

        public void SwitchEditors()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                string backup_text = GetComponentInChildren<Text>().text;
                MultilineInputField mf = GetComponent<MultilineInputField>();
                mf.FixBadDesign(backup_text);
                GetComponent<autoselection>().overrideSelect = true;
                StartCoroutine(Reseleect());
            }
        }

        IEnumerator Reseleect()
        {
            yield return 0;
            GetComponent<MultilineInputField>().Select();
        }
    }
}

