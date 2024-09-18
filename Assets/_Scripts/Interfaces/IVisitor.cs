using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor
{
    public void Visit<T>(T visitable) where T : Component, IVisitable;
}
