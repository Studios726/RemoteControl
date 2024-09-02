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

public class UpdateModelDirectionEventArgs:EventArgs
{
    public ModelDirection[] Direction;
    public Machine Machine;
    public UpdateModelDirectionEventArgs(ModelDirection[] direction,Machine machine)
    {
        Direction = direction;
        Machine = machine;
    }
}
