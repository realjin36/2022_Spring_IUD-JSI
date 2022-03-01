using UnityEngine;
using JSI.AppObject;
using System.Collections.Generic;

namespace JSI {
    public class JSIPerspCameraPerson : JSICameraPerson {
        // constants 
        public static readonly Color BG_COLOR = new Color(0.9f, 0.9f, 0.9f);
        public static readonly float FOV = 50f; // in degree
        public static readonly float NEAR = 0.01f; // in meter (1 cm)
        public static readonly float FAR = 100f; // in meter (100 m)
        public static readonly Vector3 HOME_EYE = new Vector3(0f, 1f, -5f);
        public static readonly Vector3 HOME_VIEW = new Vector3(0f, 0f, 1f);
        public static readonly Vector3 HOME_PIVOT = new Vector3(0f, 0f, 0f);

        // fields
        private Vector3 mPivot = Vector3.zero;
        public Vector3 getPivot() {
            return this.mPivot;
        }
        public void setPivot(Vector3 pt) {
            this.mPivot = pt;
        }
        private JSIAppPolyline3D mRod;
        public JSIAppPolyline3D getRod() {
            return this.mRod;
        }
        private JSIAppPolyline3D mColumn;
        public JSIAppPolyline3D getColumn() {
            return this.mColumn;
        }
        private JSIAppPolyline3D mViewRay;
        public JSIAppPolyline3D getViewRay() {
            return this.mViewRay;
        }

        // constructor 
        public JSIPerspCameraPerson() : base("PerspCameraPerson") {      
            List<Vector3> pts = new List<Vector3>();
        }

        protected override void defineInternalCameraParameters() {
            this.mCamera.clearFlags = CameraClearFlags.Color;
            this.mCamera.backgroundColor = JSIPerspCameraPerson.BG_COLOR;
            this.mCamera.cullingMask = 1; // default layer only

            this.mCamera.fieldOfView = JSIPerspCameraPerson.FOV;
            this.mCamera.nearClipPlane = JSIPerspCameraPerson.NEAR;
            this.mCamera.farClipPlane = JSIPerspCameraPerson.FAR;
        }

        protected override void defineExternalCameraParameters() {
            this.setEye(JSIPerspCameraPerson.HOME_EYE);
            this.setView(JSIPerspCameraPerson.HOME_VIEW);
            this.setPivot(JSIPerspCameraPerson.HOME_PIVOT);
            List<Vector3> pts1 = new List<Vector3>();
            pts1.Add(mPivot);
            pts1.Add(getEye());
            this.mRod = new JSIAppPolyline3D("Rod", pts1, 0.05f, Color.black); 

            List<Vector3> pts2 = new List<Vector3>();
            pts2.Add(getEye());
            pts2.Add(new Vector3(getEye().x, 0f, getEye().z));
            this.mColumn = new JSIAppPolyline3D("Column", pts2, 0.05f, Color.blue);

            List<Vector3> pts3 = new List<Vector3>();
            pts3.Add(getEye());
            Ray ray = new Ray(getEye(), getView());
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            float rayDist = float.NaN;
            ground.Raycast(ray, out rayDist);
            if(rayDist > 1e4f || rayDist < 0) {
                rayDist = 1000f;
            }
            Vector3 onPoint = ray.GetPoint(rayDist);
            pts3.Add(onPoint);
            this.mViewRay = new JSIAppPolyline3D("ViewRay", pts3, 0.05f, Color.red);
        }
    }
}