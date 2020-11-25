using BeatSaberMarkupLanguage;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VMCSetting.Views
{
    public class VMCFlowCoordinator : FlowCoordinator
    {
        Camera mirrorCamera;
        RenderTexture renderTexture;
        Material _previewMaterial;
        GameObject quad;
        VMCSettingViewController vMCSettingViewController;
        TopViewController dammyViewController;

        const int ViewTherdParson = 3;
        const int ViewFirstPerson = 6;
        const int AlwaysVisible = 10;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation) {
                this.showBackButton = true;
                this.SetTitle("VMC SETTING");
                if (vMCSettingViewController == null) {
                    vMCSettingViewController = BeatSaberUI.CreateViewController<VMCSettingViewController>();
                }
                if (dammyViewController == null) {
                    dammyViewController = BeatSaberUI.CreateViewController<TopViewController>();
                }
                var mainCamera = Camera.main;
                var gameObj = Instantiate(mainCamera.gameObject);
                gameObj.SetActive(false);
                gameObj.name = "VMC Setting";
                gameObj.tag = "Untagged";
                while (gameObj.transform.childCount > 0) DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
                
                DestroyImmediate(gameObj.GetComponent("AudioListener"));
                DestroyImmediate(gameObj.GetComponent("MeshCollider"));
                this.mirrorCamera = gameObj.GetComponent<Camera>();
                this.mirrorCamera.clearFlags = CameraClearFlags.SolidColor;
                this.mirrorCamera.stereoTargetEye = StereoTargetEyeMask.None;
                this.mirrorCamera.transform.parent = transform;
                this.mirrorCamera.transform.localPosition = Vector3.zero;
                this.mirrorCamera.transform.localRotation = Quaternion.identity;
                this.mirrorCamera.transform.localScale = Vector3.one;
                this.mirrorCamera.transform.position = new Vector3(0f, 1.2f, 2.99f);
                this.mirrorCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                this.mirrorCamera.cullingMask |= 1 << ViewTherdParson;
                this.mirrorCamera.cullingMask &= ~(1 << ViewFirstPerson);
                this.mirrorCamera.cullingMask |= 1 << AlwaysVisible;
                this.mirrorCamera.fieldOfView = 50f;
                var liv = this.mirrorCamera.GetComponent<LIV.SDK.Unity.LIV>();
                if (liv) {
                    Destroy(liv);
                }
                this.CreateRenderTextuer();
            }
            this.StartCoroutine(this.ActiveQuad());
            
            this.ProvideInitialViewControllers(this.dammyViewController, this.vMCSettingViewController);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            this.mirrorCamera.gameObject.SetActive(false);
            this.quad.SetActive(false);
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
            base.BackButtonWasPressed(topViewController);
        }

        void CreateRenderTextuer()
        {
            HMMainThreadDispatcher.instance.Enqueue(() =>
            {
                var replace = false;
                if (this.renderTexture == null) {
                    this.renderTexture = new RenderTexture(1920, 1080, 24);
                    replace = true;
                }
                if (!replace) {
                    Plugin.Log.Debug("Don't need to replace");
                    return;
                }
                quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.parent = this.transform;
                quad.transform.localScale = new Vector3(3.556f, 2f, 1f);
                quad.transform.localEulerAngles = new Vector3(0, 180, 0);
                quad.transform.localPosition = new Vector3(0f, 1f, 3f);
                DestroyImmediate(quad.GetComponent<Collider>());
                quad.layer = 0;
                this.renderTexture.useDynamicScale = false;
                this.renderTexture.antiAliasing = 2;
                this.renderTexture.Create();
                this._previewMaterial = new Material(Shader.Find("Hidden/BlitCopyWithDepth"));
                this._previewMaterial.SetTexture("_MainTex", this.renderTexture);
                this.mirrorCamera.targetTexture = this.renderTexture;
                this.mirrorCamera.enabled = true;
                this.quad.GetComponent<MeshRenderer>().material = this._previewMaterial;
                this.mirrorCamera.gameObject.SetActive(true);
            });
        }

        IEnumerator ActiveQuad()
        {
            yield return new WaitWhile(() => this.quad == null);
            this.quad.SetActive(true);
            this.mirrorCamera.gameObject.SetActive(true);
        }
    }
}
