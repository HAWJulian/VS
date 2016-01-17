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
		private GameObject clock3;
		private GameObject clock4;
        private bool leftrotate = false;
        private bool rightrotate = false;
        private GameObject[] objects;
		private GameObject[] tops;
        private int currentObject = 0;
        private int texturecounter = 0;
        void Start()
        {
            //register vbb
            VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
            for(int i = 0; i< vbs.Length; i++)
            {
                vbs[i].RegisterEventHandler(this);
            }
            //add objects
            clock = GameObject.Find("Clock_1").gameObject;
            clock2 = GameObject.Find("Clock_2").gameObject;
			clock3 = GameObject.Find("Clock_2_top").gameObject;
			clock4 = clock3;
            //insert into array "objects"
            objects = new GameObject[2] {clock,clock2};
			tops = new GameObject[2] {clock3, clock4};
            for(int i = 1; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
				tops[i].SetActive(false);
            }
        }

        void Update()
        {
            //if right or leftrotate is active
            if(rightrotate || leftrotate)
            {
                //get current rotation
                var rotationVector = objects[currentObject].transform.rotation.eulerAngles;
                //add or substract 50*Time.deltaTime depending on rotation direction
                if(rightrotate)
                {
                    rotationVector.y += 50 * Time.deltaTime;
                }
                else
                {
                    rotationVector.y -= 50 * Time.deltaTime;
                }
                //set as new rotation
                objects[currentObject].transform.rotation = Quaternion.Euler(rotationVector);
            }
        }
        //event which is called when a vb is activated 
        public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
        {
            //switch over vb name (!= object name)
            switch(vb.VirtualButtonName)
            {
                    //set right-/leftrotate if rotation buttons are activated
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
                //on objectswitch, set current object as inactive, aktivate next obeject in list
                case "objectswitch":
                    objects[currentObject].SetActive(false);
					tops[currentObject].SetActive(false);
                    var rotVec = objects[currentObject].transform.rotation.eulerAngles;
					var y = rotVec.y;
                    currentObject++;
                    currentObject %= objects.Length;
                    objects[currentObject].SetActive(true);
					tops[currentObject].SetActive(true);
                    //objects[currentObject].transform.rotation = Quaternion.Euler(rotVec);
					//objects[currentObject].transform.rotation = Quaternion.Euler(new Vector3(objects[currentObject].transform.rotation.x, y, objects[currentObject].transform.rotation.z));
					//objects[currentObject].transform.Rotate(0, y, 0, Space.Self);
					//funktioniert
					//objects[currentObject].transform.eulerAngles = new Vector3(objects[currentObject].transform.rotation.x, y, objects[currentObject].transform.rotation.z);
					objects[currentObject].transform.eulerAngles = new Vector3(objects[currentObject].transform.eulerAngles.x, y, objects[currentObject].transform.eulerAngles.z);
                    break;
                case "textureswitch":
					break;
			}
		}
		public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
		{
			//deactivate rotations on vb release
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
