using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectEventArgs : EventArgs
{
    public SocketType type;

    public ConnectEventArgs(SocketType type)
    {
        this.type = type;
    }
}
