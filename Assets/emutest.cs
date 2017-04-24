using UnityEngine;
using System.Collections.Generic;
using src.emulator;

public class emutest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        string program =
            ".data \n" +
            "CONST 4 \n" +
            "ARRAY [4] \n" +
            ".text \n" +
            "mov %ax, $3 \n" +
            "jmp b \n" +
            "albl: mov [$ARRAY + %ax], $12 \n" +
            "b: hlt \n" +
            "mov %ax, $17";

        Program p = Compiler.Compile(program, 0);

        CPU cpu = new CPU(256, 4, new Dictionary<Instruction.Type, CPU.ExternalInst>());

        cpu.LoadProgram(p);

        cpu.ExecuteUntilHalt();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
