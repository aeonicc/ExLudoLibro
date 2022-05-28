using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class WarningsPannel : MonoBehaviour
    {
        public GameObject _warningTextFather;

        public void StartBlinkForSeconds(string String)
        {
            StartCoroutine(BlinkForSeconds(String));
        }

        public IEnumerator BlinkForSeconds(string phrase)
        {
            _warningTextFather.transform.GetChild(0).gameObject.SetActive(true);
            _warningTextFather.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = phrase;
            yield return new WaitForSeconds(1.5f);
            _warningTextFather.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}