using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using OPS.AntiCheat;
using OPS.AntiCheat.Prefs;

public class ProtectedPlayerPrefsDemo : MonoBehaviour {

	void Start ()
    {
        Debug.Log("------------------");
        Debug.Log("Protected Player Prefs Demo");
        Debug.Log("------------------");

        //Add the namespace OPS.AntiCheat.Prefs
        //Replace PlayerPrefs with ProtectedPlayerPrefs. 
        //Now access protected ints, floats and strings
        
        //Int
        ProtectedPlayerPrefs.SetInt("My Int Key", 1234); //Adds a _Protected to a protected key name
        int intValue = PlayerPrefs.GetInt("My Int Key" + "_Protected"); //Because of the added _Protected, you have to read the key plus _Protected
        Debug.Log("Value saved by Unity: " + intValue);
        int protectedIntValue = ProtectedPlayerPrefs.GetInt("My Int Key");
        Debug.Log("Real Value: " + protectedIntValue);

        //Float
        ProtectedPlayerPrefs.SetFloat("My Float Key", 1234.56f);
        float floatValue = PlayerPrefs.GetInt("My Float Key" + "_Protected"); //Stored inside unity as uint
        Debug.Log("Value saved by Unity: " + floatValue);
        float protectedFloatValue = ProtectedPlayerPrefs.GetFloat("My Float Key");
        Debug.Log("Real Value: " + protectedFloatValue);

        //String
        ProtectedPlayerPrefs.SetString("My String Key", "Hello World!");
        string stringValue = PlayerPrefs.GetString("My String Key" + "_Protected");
        Debug.Log("Value saved by Unity: " + stringValue);
        string protectedStringValue = ProtectedPlayerPrefs.GetString("My String Key");
        Debug.Log("Real Value: " + protectedStringValue);
    }
}
