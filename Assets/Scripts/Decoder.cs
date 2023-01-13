using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DefaultNamespace
{
    public class Decoder : MonoBehaviour
    {
        public string fileName;
        private Data data;
        private bool isPlaying;
        private float time;

        private void Start()
        {
            var filePath = Application.dataPath + "/Saves/" + fileName;
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);

                data = formatter.Deserialize(stream) as Data;

                stream.Close();
            }
            else
            {
                Debug.LogError("Save file not found in" + filePath);
            }
        }

        private void Update()
        {
            if (!isPlaying && (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.One)))
            {
                isPlaying = true;
            }

            if (isPlaying && time < data.timeLength)
            {
                var pos = new Vector3();
                var rot = new Vector3();
                for (int i = 0; i < data.posX.Length; i++)
                {
                    pos.x = Mathf.Pow(time, i) * (float)data.posX[i];
                }

                for (int i = 0; i < data.posY.Length; i++)
                {
                    pos.y = Mathf.Pow(time, i) * (float)data.posY[i];
                }

                for (int i = 0; i < data.posZ.Length; i++)
                {
                    pos.z = Mathf.Pow(time, i) * (float)data.posZ[i];
                }

                for (int i = 0; i < data.rotX.Length; i++)
                {
                    rot.x = Mathf.Pow(time, i) * (float)data.rotX[i];
                }

                for (int i = 0; i < data.rotY.Length; i++)
                {
                    rot.y = Mathf.Pow(time, i) * (float)data.rotY[i];
                }

                for (int i = 0; i < data.rotZ.Length; i++)
                {
                    rot.z = Mathf.Pow(time, i) * (float)data.rotZ[i];
                }

                transform.position = pos;
                transform.rotation = Quaternion.Euler(rot);
                time += Time.deltaTime;
            }
        }
    }
}