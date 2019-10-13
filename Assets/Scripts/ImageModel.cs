using System;
using UnityEngine;
using UnityEngine.UI;

namespace Synapse
{
    [Serializable]
    public class ImageModel
    {
        public Sprite Sprite;
        public string Description;

        public bool Shown { get; set; }
        public int LastScreenIndex { get; set; }
    }
}