using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private GameObject[] hudObjects;
        [SerializeField] private MatchController mc;

        private int PannelNumber = 0;
        [SerializeField] private GameObject ButtonNext;
        [SerializeField] private GameObject ButtonStart;

        public void Start()
        {
            ButtonNext.SetActive(true);
            hudObjects[PannelNumber].SetActive(true);
        }

        public void ChangePannel()
        {
            hudObjects[PannelNumber].SetActive(false);
            PannelNumber++;
            if (hudObjects.Length == PannelNumber)
            {
                hudObjects[0].transform.parent.gameObject.SetActive(false);
                ButtonStart.SetActive(false);
                mc.MatchStart();
                return;
            }

            if (PannelNumber == hudObjects.Length - 1)
            {
                ButtonNext.SetActive(false);
                ButtonStart.SetActive(true);
            }

            hudObjects[PannelNumber].SetActive(true);

        }
    }
}