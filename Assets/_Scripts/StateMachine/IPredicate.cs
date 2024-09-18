using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public interface IPredicate
    {
        public bool Evaluate();
    }
}