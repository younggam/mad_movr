using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using MathNet.Numerics;
using UnityEngine.Windows.WebCam;

namespace DefaultNamespace
{
    public class Recoder : MonoBehaviour
    {
        public string fileName;
        public int order = 5;
        private List<double> times;
        private List<Vector3> positions;
        private List<Vector3> rotations;
        private bool isRecording;
        private float time;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.One))
            {
                if (isRecording) StopRecording();
                else StartRecording();
            }

            if (isRecording)
            {
                times.Add(time);
                positions.Add(transform.position);
                rotations.Add(transform.eulerAngles);
                time += Time.deltaTime;
            }
        }

        private void StartRecording()
        {
            times = new();
            positions = new();
            rotations = new();
            time = 0;
            isRecording = true;
        }

        private void StopRecording()
        {
            var timesArray = times.ToArray();
            var posX = Fit.Polynomial(timesArray, positions.Select(v => (double)v.x).ToArray(), order);
            var posY = Fit.Polynomial(timesArray, positions.Select(v => (double)v.y).ToArray(), order);
            var posZ = Fit.Polynomial(timesArray, positions.Select(v => (double)v.z).ToArray(), order);

            var rotX = Fit.Polynomial(timesArray, rotations.Select(v => (double)v.x).ToArray(), order);
            var rotY = Fit.Polynomial(timesArray, rotations.Select(v => (double)v.y).ToArray(), order);
            var rotZ = Fit.Polynomial(timesArray, rotations.Select(v => (double)v.z).ToArray(), order);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/Saves/" + fileName, FileMode.Create);

            formatter.Serialize(stream,
                new Data
                {
                    timeLength = time, posX = posX, posY = posY, posZ = posZ, rotX = rotX, rotY = rotY, rotZ = rotZ
                });
            stream.Close();
        }
    }
}