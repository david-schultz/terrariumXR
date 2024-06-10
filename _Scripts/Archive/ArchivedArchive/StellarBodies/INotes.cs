using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public interface INotes
    {
        int id { get; }
        string name { get; }
        IStellarBody parent { get; }
        DateTime dateCreated { get; }
        DateTime dateCompleted { get; set; }
    }
}