using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static int whichRemote;
	public static Vector3 wiimoteInput;
    // Start is called before the first frame update
    void Start()
    {
			Wii.StartSearch();
    }

    // Update is called once per frame
    void Update()
    {
      wiimoteInput = Wii.GetWiimoteAcceleration(whichRemote);
			Debug.Log(Wii.GetBattery(whichRemote)+"% Battery.");
    }
}
