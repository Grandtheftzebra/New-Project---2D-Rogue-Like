using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitable
{
    public void Accept(IVisitor visitor);
}
