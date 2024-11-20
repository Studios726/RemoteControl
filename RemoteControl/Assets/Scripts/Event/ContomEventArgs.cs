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
    public TaskCommand taskCommand;

    public TaskLogArgs(string str,TaskCommand taskCommand)
    {
        this.des = str;
        this.time = taskCommand.AllData.CodeTime;
        this.taskCommand = taskCommand;
        pos = "下一目标点 ";
        if (taskCommand.AllData.NextPositionList==null)
        {
            taskCommand.AllData.NextPositionList = new List<int>();
            taskCommand.AllData.NextPositionList.Add(0);
            taskCommand.AllData.NextPositionList.Add(0);
            taskCommand.AllData.NextPositionList.Add(0);
        }
       
        if (taskCommand.AllData.NextPositionList!=null)
        {
            for (int i = 0; i < taskCommand.AllData.NextPositionList.Count; i++)//[回转，俯仰，前进dd]
            {
                if (i==0)
                {
                    pos =pos+ $"回转: {taskCommand.AllData.NextPositionList[i]}° ";
                }else if (i==1)
                {
                    pos =pos+ $"俯仰: {taskCommand.AllData.NextPositionList[i]}° ";
                }
                else if (i==2)
                {
                    pos =pos+$"前进: {taskCommand.AllData.NextPositionList[i]}m";
                }
                else
                {
                    //无
                }
            }
        }
       
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
