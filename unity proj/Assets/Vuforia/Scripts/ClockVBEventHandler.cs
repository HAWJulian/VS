using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// This behaviour associates a Virtual Button with a game object. Use the
    /// functionality in ImageTargetBehaviour to create and destroy Virtual Buttons
    /// at run-time.
    /// </summary>
    public class ClockVBEventHandler : MonoBehaviour, IVirtualButtonEventHandler
    {
        private GameObject clock;
        private GameObject clock2;
        private bool leftrotate = false;
        private bool rightrotate = false;
        private GameObject[] objects;
        private int currentObject = 0;
        private int texturecounter;
        void Start()
        {
            VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
            for(int i = 0; i< vbs.Length; i++)
            {
                vbs[i].RegisterEventHandler(this);
            }
            clock = transform.FindChild("Clock_panel").gameObject;
            clock2 = transform.FindChild("Clock").gameObject;
            objects = new GameObject[2] {clock,clock2};
            for(int i = 1; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }
        }

        void Update()
        {
           
            if(rightrotate || leftrotate)
            {

                var rotationVector = objects[currentObject].transform.rotation.eulerAngles;
                if(rightrotate)
                {
                    rotationVector.y += 50 * Time.deltaTime;
                }
                else
                {
                    rotationVector.y -= 50 * Time.deltaTime;
                }
                objects[currentObject].transform.rotation = Quaternion.Euler(rotationVector);
            }
        }
        public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
        {
            switch(vb.VirtualButtonName)
            {
                case "vbrr":
                    if(!leftrotate)
                    {
                        rightrotate = true;
                    }
                    break;
                case "vbrl":
                    if(!rightrotate)
                    {
                        leftrotate = true;
                    }
                    break;
                case "objectswitch":
                    objects[currentObject].SetActive(false);
                    var rotVec = objects[currentObject].transform.rotation.eulerAngles;
                    currentObject++;
                    currentObject %= objects.Length;
                    objects[currentObject].SetActive(true);
                    objects[currentObject].transform.rotation = Quaternion.Euler(rotVec);
                    break;
            }
        }
        public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
        {
            switch (vb.VirtualButtonName)
            {
                case "vbrr":
                    rightrotate = false;
                    break;
                case "vbrl":
                    leftrotate = false;
                    break;
            }
        }
    }
}
