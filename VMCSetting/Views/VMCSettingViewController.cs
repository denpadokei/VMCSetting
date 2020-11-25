using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using uOSC;

namespace VMCSetting.Views
{
    [HotReload]
    internal class VMCSettingViewController : BSMLAutomaticViewController
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
        [UIValue("modes")]
        private List<object> modes { get; } = new List<object>() { "1 Normal", "2 Floor correction" };

        uOscClient client;

        /// <summary>説明 を取得、設定</summary>
        private string mode_;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("mode")]
        public string Mode
        {
            get => this.mode_ ?? "";

            set
            {
                this.mode_ = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>説明 を取得、設定</summary>
        private string notifyText_;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("notify-text")]
        public string NotifyText
        {
            get => this.notifyText_ ?? "";

            set
            {
                this.notifyText_ = value;
                this.NotifyPropertyChanged();
            }
        }
        void Awake()
        {
            this.client = this.gameObject.AddComponent<uOscClient>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        [UIAction("calibrate-ready")]
        async void ClickedCalibrate()
        {
            this.RequestCalibrate();
            for (int i = 0; i < 5; i++) {
                this.NotifyText = $"{5 - i}";
                await Task.Delay(1000);
            }
            await this.ExecuteCalibrate();
        }

        [UIAction("#post-parse")]
        void PostParse()
        {
            this.NotifyText = "Ready";
        }

        void RequestCalibrate()
        {
            this.client.Send("/VMC/Ext/Set/Calib/Ready", null);
        }

        async Task ExecuteCalibrate()
        {
            this.client.Send("/VMC/Ext/Set/Calib/Exec", this.modes.IndexOf(Mode) + 1);
            this.NotifyText = "Calibrating...";
            await Task.Delay(2000);
            this.NotifyText = "Compleate";
        }
    }
}
