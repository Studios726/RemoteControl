using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShangHaiPro
{
    public class SendDataReportAndDEM
    {
        public CoalHeapDEM SendCoalHeapDEM;
        public Report SendReport;
        public int code;

        public SendDataReportAndDEM()
        {
            SendCoalHeapDEM = new CoalHeapDEM();

            SendReport = new Report();
        }

    }

}