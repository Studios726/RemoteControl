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

public class BeltRunArgs : EventArgs
{
    public bool isPlayPileBeltAnim;
    public bool isPlayTakeBeltAnim;

    public BeltRunArgs(bool isPlayPileBeltAnim, bool isPlayTakeBeltAnim)
    {
        this.isPlayPileBeltAnim = isPlayPileBeltAnim;
        this.isPlayTakeBeltAnim = isPlayTakeBeltAnim;
    }
}

public class TaskLogArgs : EventArgs
{
    public string des;
    public DateTime time;
    public string pos;

    public TaskLogArgs(string str,DateTime dateTime,string  pos)
    {
        this.des = str;
        this.time = dateTime;
        this.pos = pos;
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
