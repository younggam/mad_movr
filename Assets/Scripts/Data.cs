using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [Serializable]
    public class Data
    {
        public float timeLength;
        public double[] posX;
        public double[] posY;
        public double[] posZ;
        public double[] rotX;
        public double[] rotY;
        public double[] rotZ;
    }
}