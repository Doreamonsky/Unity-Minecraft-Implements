using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using OPS.AntiCheat;
using OPS.AntiCheat.Field;

public class ProtectedFieldsDemo : MonoBehaviour {

	void Start ()
    {
        Debug.Log("------------------");
        Debug.Log("Protected Fields Demo");
        Debug.Log("------------------");

        //Add the namespace OPS.AntiCheat.Field
        //Replace your field type with a protected one. (Most types are supported, so add a Protected in front.)
        //Now access the protected value of a field, with the property Value.

        //UInt
        ProtectedUInt16 a = new ProtectedUInt16(1234);
        a += 1;
        Debug.Log(a);
        ProtectedUInt32 b = new ProtectedUInt32(5678);
        b += 2;
        Debug.Log(b);
        ProtectedUInt64 c = new ProtectedUInt64(91011);
        c += 3;
        Debug.Log(c);

        //Int
        ProtectedInt16 d = new ProtectedInt16(1234);
        d += 1;
        Debug.Log(d);
        ProtectedInt32 e = new ProtectedInt32(5678);
        e += 2;
        Debug.Log(e);
        ProtectedInt64 f = new ProtectedInt64(91011);
        f += 3;
        Debug.Log(f);

        //Float
        ProtectedFloat g = new ProtectedFloat(1234.123f);
        g += 0.11f;
        Debug.Log(g);

        //String
        ProtectedString i = new ProtectedString("My Protected Text");
        i += "!!";
        Debug.Log(i);
    }

    private ProtectedInt32 demoField;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            this.demoField += 1;
        }
    }
}
