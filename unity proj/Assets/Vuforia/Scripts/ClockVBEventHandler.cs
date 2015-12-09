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
        private GameObject cube;
        private bool leftrotate = false;
        private bool rightrotate = false;
        void Start()
        {
            VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
            for(int i = 0; i< vbs.Length; i++)
            {
                vbs[i].RegisterEventHandler(this);
            }
            clock = transform.FindChild("Clock_panel").gameObject;
            cube = transform.FindChild("Cube").gameObject;
            cube.SetActive(false);
        }
        
        void Update()
        {
           
            if(rightrotate || leftrotate)
            {

                var rotationVector = clock.transform.rotation.eulerAngles;
                if(rightrotate)
                {
                    rotationVector.y += 50 * Time.deltaTime;
                }
                else
                {
                    rotationVector.y -= 50 * Time.deltaTime;
                }
                

                clock.transform.rotation = Quaternion.Euler(rotationVector);
                //clock.transform.Rotate(0, 10*Time.deltaTime, 0);
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
                    clock.SetActive(false);
                    cube.SetActive(true);
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
