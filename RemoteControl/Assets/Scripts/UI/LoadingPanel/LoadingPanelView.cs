using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanelView : UIView<LoadingPanelCtr>
{
   private Text _loadingTxt;
   private Slider _progressSlider;
   public override void InitUIElements(UIArgs uiArgs)
   {
      _progressSlider = RootObj.transform.Find("loadingSlider").GetComponent<Slider>();
      
      _progressSlider.value = 0;
   }

   public void UpdateProgress(float progress)
   {
      _progressSlider.value = progress;
   }
}
