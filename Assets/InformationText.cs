using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;
using Fusion;
using GNW2.Input;
using GNW2.Player;
using UnityEngine.UI;


    public class InformationText : MonoBehaviour
    {
        [SerializeField] public Player player;
        public bool readyToFire;
        public TextMeshProUGUI text;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (readyToFire)
            {
                text.SetText("Ready to fire!");
            }
            else if (!readyToFire)
            {
                text.SetText("In cooldown...");
            }
        

    }
}

