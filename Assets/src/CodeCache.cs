using UnityEngine;
using System.Collections.Generic;

public class CodeCache {

    public const string AUTO_SAVE = "__auto-save__";

    private static CodeCache instance;

    public static CodeCache Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CodeCache();
            }

            return instance;
        }
    }

    private IDictionary<string, string> code;

    private CodeCache()
    {
        code = new Dictionary<string, string>();
        code[AUTO_SAVE] = "auto save test";
    }


    public void SaveCode(string name, string codeText)
    {
        code[name]  = codeText;
    }

    public bool IsSavedCode(string name)
    {
        return code.ContainsKey(name);
    }

    public string GetSavedCode(string name)
    {
        return code[name];
    }
}
