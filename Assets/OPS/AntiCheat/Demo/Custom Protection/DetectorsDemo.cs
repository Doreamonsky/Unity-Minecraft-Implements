using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using OPS.AntiCheat;
using OPS.AntiCheat.Detector;

public class DetectorsDemo : MonoBehaviour {

    public UnityEngine.UI.Text Text;

	void Start () {
        //Add the namespace OPS.AntiCheat.Detector

        //Every protected field contains a hidden field, showing its unprotected value in memory.
        //If a cheater tried to modify this value, it will not affect the game, but you can catch him because you 
        //know the correct value.
        FieldCheatDetector.OnFieldCheatDetected += FieldCheatDetector_OnFieldCheatDetected;
    }
    
    private void FieldCheatDetector_OnFieldCheatDetected()
    {
        Text.text = "Field Hack Detected! Cheater tried to modify memory!";
    }
}
