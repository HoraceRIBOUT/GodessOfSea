using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static int whichRemote;
    // Start is called before the first frame update
    void Start()
    {
			Wii.StartSearch();
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 input = Wii.GetWiimoteAcceleration(whichRemote);
        //Debug.Log(input);
          //Wii.ToggleRumble(whichRemote);
          if (input.z > 3.0f || input.z < -3.0f){//Decided arbitrarily through tests
            Debug.Log("Input?");
          }
    }
}
