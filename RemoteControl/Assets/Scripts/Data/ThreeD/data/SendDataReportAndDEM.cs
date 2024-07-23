using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShangHaiPro
{
    public class SendDataReportAndDEM
    {
        public CoalHeapDEM SendCoalFeapDEM;
        public Report SendReport;

        public SendDataReportAndDEM()
        {
            SendCoalFeapDEM = new CoalHeapDEM();

            SendReport = new Report();
        }

    }

}