using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DataScriptableObject", menuName = "ScriptableObjects/DataScriptableObject")]
public class DataScriptableObject : ScriptableObject
{
    public List<Symbol> symbolsData = new List<Symbol>();
    public List<PayLines> payLinesData = new List<PayLines>();
}

[Serializable]
public class PayLines
{
    public List<int> lineAddress = new List<int>();
}