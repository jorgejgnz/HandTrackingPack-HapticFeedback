using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

namespace JorgeJGnz
{
    public enum LogicGate
    {
        EQ,
        NOTEQ,
        AND,
        OR,
        NAND,
        NOR,
        XOR
    }
    public class Condition : BooleanAction
    {
        [Header("Condition")]
        public BooleanAction A;
        public LogicGate logicGate;
        public BooleanAction B;

        [Header("Debugging")]
        public bool currentValue;

        void Update()
        {
            switch (logicGate)
            {
                case LogicGate.EQ:
                    Receive(A.Value == B.Value);
                    break;
                case LogicGate.NOTEQ:
                    // Same as XOR
                    Receive(A.Value != B.Value);
                    break;
                case LogicGate.AND:
                    Receive(A.Value && B.Value);
                    break;
                case LogicGate.OR:
                    Receive(A.Value || B.Value);
                    break;
                case LogicGate.NAND:
                    Receive(!A.Value || !B.Value);
                    break;
                case LogicGate.NOR:
                    Receive(!A.Value && !B.Value);
                    break;
                default:
                    break;
            }

            currentValue = Value;
        }
    }
}
