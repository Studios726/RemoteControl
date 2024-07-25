using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class BucketWheelTaskBase : MonoBehaviour
{
   public Button scramStopBtn;
   
   public Button resetBtn;
   public Button warningBtn;
   public Text[] warningTexts;
   public InputField startTakeMaterText;
   public InputField stopTakeMaterText;
   public Toggle leftToggle;
   public Toggle rightToggle;
   public InputField leftTakeMaterText;
   public InputField rightTakeMaterText;
   public InputField takeMaterStep;
   public InputField timeHourText;
   public InputField timeMinuteText;
   public Toggle timeOpenToggle;
   public Toggle timeCloseToggle;
   public InputField takeMaterNum;
   public Toggle quantityOpenToggle;
   public Toggle quantityCloseToggle;
   public Button takeMaterStartBtn;
   public Button takeMaterStopBtn;
   public Button takeMaterReversingBtn;
   public Button takeMaterEndBtn;
}
