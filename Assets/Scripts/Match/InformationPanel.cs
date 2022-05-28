using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class InformationPanel : MonoBehaviour
    {
        /// <summary>
        /// This is the fight of the creation
        /// Now you will be fighting to decide the destiny of the world
        /// Choose between one of the 5 elements
        /// Wood, Metal, Water, Earth, Fire
        /// Combine your elements with your oponents to create or destroy
        /// and generate a new terrain for the new planet
        /// </summary>

        [SerializeField] private GameObject MainText;

        private bool something;

        [SerializeField] private GameObject StartButtonCanvas;

        private GameObject _StartButton;
        private TextMeshProUGUI _textMesh;

        private void Start()
        {

            //_StartButton = StartButtonCanvas.transform.GetChild(0).gameObject;
            //CallRoutine();

        }

        private void CallRoutine()
        {

            StartCoroutine(ExplanatoryTextChanger(MainText));
        }

        IEnumerator ExplanatoryTextChanger(GameObject text)
        {
            var TextComp = text.GetComponent<TextMeshProUGUI>();
            /*
            TextComp.text = "This is the fight of the creation";
            yield return new WaitForSeconds(2);
            TextComp.text = "Now you will be fighting to decide the destiny of the world";
            yield return new WaitForSeconds(2);
            TextComp.text = "Choose between o'ne of the 5 elements";
            yield return new WaitForSeconds(2);
            TextComp.text = "Wood, Metal, Water, Earth, Fire";
            yield return new WaitForSeconds(2);
            TextComp.text = "Combine your elements with your oponents to create or destroy";
            yield return new WaitForSeconds(2);
            TextComp.text = "and generate a new terrain for the new planet";
            yield return new WaitForSeconds(2);
            */
            gameObject.transform.GetChild(0).gameObject.SetActive(false);

            _StartButton.SetActive(true);
            yield return 0;
        }
    }
}



