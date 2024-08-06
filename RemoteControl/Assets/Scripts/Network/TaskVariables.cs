using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVariables
{
    public string TaskID { get; set; }
    public string OperatorSystem { get; set; }
    public List<int> ProcessingProgress { get; set; }
    public List<int> MachineError { get; set; }
}
