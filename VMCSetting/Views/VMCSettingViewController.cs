using System;
using System.Collections.Generic;
using System.Linq;

using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;


namespace VMCSetting.Views
{
    internal class VMCSettingViewController : PersistentSingleton<VMCSettingViewController>
    {
        

        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);

        [UIAction("calibrate")]
        void ClickedCalibrate()
        {

        }

        void RequestCalibrate()
        {

        }

        void ExecuteCalibrate()
        {

        }
    }
}
