﻿using UnityEngine;

namespace Assets.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        public SoundManager soundManager;

        // Use this for initialization
        void Start()
        {
            soundManager.PlayMenuBackground();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}