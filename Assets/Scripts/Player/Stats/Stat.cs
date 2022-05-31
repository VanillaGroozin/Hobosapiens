using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat : LevelSystem
{
    [SerializeField]
    private int _baseValue = 0;
    public string name = string.Empty;

    public int GetValue()
    {
        return _baseValue;
    }
    public void ModifyValueWithOperator(string operation)
    {
        var operationType = operation.Remove(1, operation.Length - 1);
        var operationValue = operation.Remove(0, 1);

        if (operationType == "+")
            _baseValue += Convert.ToInt32(operationValue);
        else if (operationType == "-")
            _baseValue -= Convert.ToInt32(operationValue);
    }
    public void ModifyValue(string value)
    {        
        _baseValue = Convert.ToInt32(value);
    }
}
