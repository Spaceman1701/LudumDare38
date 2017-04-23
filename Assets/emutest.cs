using UnityEngine;
using System.Collections;
using src.emulator;

public class emutest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(Instruction.Type.ADD.IsVaidParam(0, Instruction.ParamType.LBL));


        string program =
            ".text \n" +
            "mov ax, 100 \n" +
            "albl: mov bx, 12 \n";

        Compiler.Compile(program);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
