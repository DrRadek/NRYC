using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ValueChecks
{
    public enum ValueChangeResult
    {
        OK = 0,
        FULL = 1,
        EMPTY = 2
    }
    class ValueChange
    {
        public static ValueChangeResult Result = ValueChangeResult.OK; 
    }
}