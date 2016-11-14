using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class JoystickHelper : MonoBehaviour {

	public Text text;

	private string[] keys = new string[16]{"a", "q", "d", "c", "w", "e", "x", "z", "y", "t", "u", "f", "j", "n", "h", "r"};
	private KeyCode[] joystickCodes = new KeyCode[14] { KeyCode.JoystickButton0, KeyCode.JoystickButton1, KeyCode.JoystickButton2,
		KeyCode.JoystickButton3, KeyCode.JoystickButton4, KeyCode.JoystickButton5, KeyCode.JoystickButton6,
		KeyCode.JoystickButton7, KeyCode.JoystickButton8, KeyCode.JoystickButton9, KeyCode.JoystickButton10, 
		KeyCode.JoystickButton11, KeyCode.JoystickButton12, KeyCode.JoystickButton13 };

	private float f_front = 0;
	private float f_back = 0;
	private float f_right = 0;
	private float f_left = 0;


	void Start() {
		text.text = "Rafa";
	}

	void Update() {


		/*for (int i = 0; i < keys.Length; i++) {
			if(CrossPlatformInputManager.GetButtonDown(keys[i])) {
				text.text = keys[i] + " ";
			}
		}

		for (int n = 0; n < joystickCodes.Length; n++) {
			if(Input.GetKey(joystickCodes[n])) {
				text.text = joystickCodes[n] + " ";
			}
		}

		for (int n = 0; n < joystickCodes.Length; n++) {
			if(Input.GetButtonDown("Fire1")) {
				text.text = "Fire1" + " ";
			}
		}*/

		if (Input.GetKeyDown (KeyCode.JoystickButton4)) {
			f_front = 1;
		} else if (Input.GetKeyDown (KeyCode.JoystickButton2)) {
			f_front = 0;
		}

		if (Input.GetKeyDown (KeyCode.JoystickButton6)) {
			f_back = 1;
		} else if (Input.GetKeyDown (KeyCode.JoystickButton3)) {
			f_back = 0;
		}

		if (Input.GetKeyDown (KeyCode.JoystickButton5)) {
			f_right = 1;
		} else if (Input.GetKeyDown (KeyCode.JoystickButton1)) {
			f_right = 0;
		}

		if (Input.GetKeyDown (KeyCode.JoystickButton7)) {
			f_left = 1;
		} else if (Input.GetKeyDown (KeyCode.JoystickButton11)) {
			f_left = 0;
		}

		Vector3 moveDirection = new Vector3(f_right - f_left, 0, f_front - f_back);
		text.text = moveDirection.ToString ();

	}

	void OnGUI( )
	{
		if (Event.current.ToString () == "Repaint")
			return;
		if (Event.current.ToString () == "Layout")
			return;
		if (Event.current.ToString () == "layout")
			return;

		//text.text = Event.current.ToString ();
	}
}
