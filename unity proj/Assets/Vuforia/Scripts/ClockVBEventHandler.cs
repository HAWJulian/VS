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
		/* All GameObjects */
		/*
        private GameObject clock2;
		private GameObject clock3;
		private GameObject clock4;
		private GameObject clock5;
		private GameObject clock6;
		private GameObject clock7;
		private GameObject clock8;
		private GameObject clock9;
		private GameObject clock10;
		*/

		/* booleans to control rotation */
        private bool leftrotate = false;
        private bool rightrotate = false;
		/* collections to hold current displayed objects*/
        private GameObject[] objects;
		private GameObject[] topsalt;
		private GameObject[][] objects2;
		private GameObject[][] tops;
		private GameObject[][] bots;
		/* counter variable to determine current index */
        private int currentObject = 0;
        private int texturecounter = 0;
		private int SIZE = 3;
		public enum State {OBJECT, TEXTURE};
		private State state;

        void Start()
        {
            //register vbb
            VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
            for(int i = 0; i< vbs.Length; i++)
            {
                vbs[i].RegisterEventHandler(this);
            }

            //add objects
			GameObject clock_2 = GameObject.Find ("Clock_2").gameObject;
			GameObject clock_3 = GameObject.Find ("Clock_3").gameObject;
			GameObject clock_4 = GameObject.Find ("Clock_4").gameObject;
			GameObject clock_5 = GameObject.Find ("Clock_5").gameObject;
			GameObject clock_6 = GameObject.Find ("Clock_6").gameObject;
			GameObject clock_7 = GameObject.Find ("Clock_7").gameObject;
			GameObject clock_8 = GameObject.Find ("Clock_8").gameObject;
			GameObject clock_9 = GameObject.Find ("Clock_9").gameObject;
			GameObject clock_10 = GameObject.Find ("Clock_10").gameObject;

			GameObject clock_2_top = GameObject.Find ("Clock_2_top").gameObject;
			GameObject clock_3_top = GameObject.Find ("Clock_3_top").gameObject;
			GameObject clock_4_top = GameObject.Find ("Clock_4_top").gameObject;
			GameObject clock_5_top = GameObject.Find ("Clock_5_top").gameObject;
			GameObject clock_6_top = GameObject.Find ("Clock_6_top").gameObject;
			GameObject clock_7_top = GameObject.Find ("Clock_7_top").gameObject;
			GameObject clock_8_top = GameObject.Find ("Clock_8_top").gameObject;
			GameObject clock_9_top = GameObject.Find ("Clock_9_top").gameObject;
			GameObject clock_10_top = GameObject.Find ("Clock_10_top").gameObject;

			GameObject clock_2_bot = GameObject.Find ("Clock_2_bot").gameObject;
			GameObject clock_3_bot = GameObject.Find ("Clock_3_bot").gameObject;
			GameObject clock_4_bot = GameObject.Find ("Clock_4_bot").gameObject;
			GameObject clock_5_bot = GameObject.Find ("Clock_5_bot").gameObject;
			GameObject clock_6_bot = GameObject.Find ("Clock_6_bot").gameObject;
			GameObject clock_7_bot = GameObject.Find ("Clock_7_bot").gameObject;
			GameObject clock_8_bot = GameObject.Find ("Clock_8_bot").gameObject;
			GameObject clock_9_bot = GameObject.Find ("Clock_9_bot").gameObject;
			GameObject clock_10_bot = GameObject.Find ("Clock_10_bot").gameObject;
			//TODO: Insert real models
			objects2 = new GameObject[][]{
				new GameObject[3]{ clock_2, clock_3, clock_4 },
				new GameObject[3]{ clock_5, clock_6, clock_7 },
				new GameObject[3]{ clock_8, clock_9, clock_10 }
			};

			tops = new GameObject[][]{
				new GameObject[3]{ clock_2_top, clock_3_top, clock_4_top },
				new GameObject[3]{ clock_5_top, clock_6_top, clock_7_top },
				new GameObject[3]{ clock_8_top, clock_9_top, clock_10_top}
			};

			bots = new GameObject[][]{
				new GameObject[3]{ clock_2_bot, clock_3_bot, clock_4_bot},
				new GameObject[3]{ clock_5_bot, clock_6_bot, clock_7_bot},
				new GameObject[3]{ clock_8_bot, clock_9_bot, clock_10_bot}
			};


			/* hide all objects not displayed at the beginning */	
			for(int i = 0; i < objects2.GetLength(0); i++)
            {
				for (int j = 0; j < objects2.GetLength(1); j++) 
				{
					if (!(j == 0 && i == 0)) //Skip launched model 
					{
						objects2 [i] [j].SetActive (false);
						tops[i] [j].SetActive (false);
						bots[i] [j].SetActive (false);
					}
				}
            }
        }

        void Update()
        {
            //if right or leftrotate is active
            if(rightrotate || leftrotate)
            {
                //get current rotation
				var rotationVector = objects2[currentObject][texturecounter].transform.rotation.eulerAngles;
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
				objects2[currentObject][texturecounter].transform.rotation = Quaternion.Euler(rotationVector);
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
					state = State.OBJECT;
					HandleSwitchPressed (state);
                    break;

                case "textureswitch":
					state = State.TEXTURE;
					HandleSwitchPressed (state);
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

		public void HandleSwitchPressed(State state){
			objects2 [currentObject] [texturecounter].SetActive (false);
			tops [currentObject] [texturecounter].SetActive (false);
			bots [currentObject] [texturecounter].SetActive (false);

			/* safe rotation vector for next object */
			var rotVec = objects2 [currentObject] [texturecounter].transform.rotation.eulerAngles;
			var y = rotVec.y;

			if(state == State.OBJECT){
				currentObject++;
				currentObject %= SIZE; // next object in modulo circle
				texturecounter = 0; // reset texture counter when object changed
			} else{
				texturecounter++; // next texture
				texturecounter %= SIZE; // modulo circle
			}

			objects2 [currentObject] [texturecounter].SetActive(true);
			tops [currentObject] [texturecounter].SetActive(true);
			bots [currentObject] [texturecounter].SetActive(true);
			//[*]
			objects2 [currentObject] [texturecounter].transform.eulerAngles =
				new Vector3(objects2 [currentObject] [texturecounter].transform.eulerAngles.x,
					y, /* apply absolute rotation */ 
					objects2 [currentObject] [texturecounter].transform.eulerAngles.z);
		}
    }
}
//[*]
//objects[currentObject].transform.rotation = Quaternion.Euler(rotVec);
//objects[currentObject].transform.rotation = Quaternion.Euler(new Vector3(objects[currentObject].transform.rotation.x, y, objects[currentObject].transform.rotation.z));
//objects[currentObject].transform.Rotate(0, y, 0, Space.Self);
//funktioniert
//objects[currentObject].transform.eulerAngles = new Vector3(objects[currentObject].transform.rotation.x, y, objects[currentObject].transform.rotation.z);
