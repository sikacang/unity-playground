using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VadimskyiLab.Events;

public class ViewEvent : EventBase<ViewEvent, ViewEventArgs>
{    
}

public readonly struct ViewEventArgs
{
    public readonly EnumId ViewId;
    
    public ViewEventArgs(EnumId viewId)
    {
        ViewId = viewId;
    }
}
