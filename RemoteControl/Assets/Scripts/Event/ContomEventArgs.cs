using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContomEventArgs
{
    
}

public class MessageEventArgs:EventArgs
{
    public string message;
    public SocketType socketTpe;

    public MessageEventArgs(string message,SocketType type)
    {
        this.message = message;
        this.socketTpe = type;
    }
}
