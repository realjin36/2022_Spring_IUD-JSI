using System.Text;
using X;
using UnityEngine;
using JSI.AppObject;
using System.Collections.Generic;

namespace JSI.Cmd {
    public class JSICmdToTumbleCamera : XLoggableCmd {
        // fields
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;

        // private constructor
        private JSICmdToTumbleCamera(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIPenMark penMark = jsi.getPenMarkMgr().getLastPenMark();
            this.mPrevPt = penMark.getRecentPt(1);
            this.mCurPt = penMark.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToTumbleCamera cmd = new JSICmdToTumbleCamera(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIPerspCameraPerson cp = jsi.getPerspCameraPerson();

            float dx = this.mCurPt.x - this.mPrevPt.x;
            float dy = this.mCurPt.y - this.mPrevPt.y;
            float dAzimuth = 180f * dx / Screen.width;
            float dZenith = 180f * dy / Screen.height;

            Quaternion qa = Quaternion.AngleAxis(dAzimuth, Vector3.up);
            Quaternion qz = Quaternion.AngleAxis(-dZenith, cp.getRight());

            Vector3 pivotToEye = cp.getEye() - cp.getPivot();
            Vector3 nextEye = cp.getPivot() + qa * qz * pivotToEye;
            Vector3 nextView = qa * qz * cp.getView();

            cp.setEye(nextEye);
            cp.setView(nextView);

            JSIAppPolyline3D rod = cp.getRod();
            List<Vector3> pts1 = new List<Vector3>();
            pts1.Add(cp.getPivot());
            pts1.Add(cp.getEye());
            rod.setPts(pts1);

            JSIAppPolyline3D column = cp.getColumn();
            List<Vector3> pts2 = new List<Vector3>();
            pts2.Add(cp.getEye());
            pts2.Add(new Vector3(cp.getEye().x, 0f, cp.getEye().z));
            column.setPts(pts2);

            JSIAppPolyline3D viewRay = cp.getViewRay();
            List<Vector3> pts3 = new List<Vector3>();
            pts3.Add(cp.getEye());
            Ray ray = new Ray(cp.getEye(), cp.getView());
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            float rayDist = float.NaN;
            ground.Raycast(ray, out rayDist);
            if(rayDist > 1e4f || rayDist < 0) {
                rayDist = 1000f;
            }
            Vector3 onPoint = ray.GetPoint(rayDist);
            pts3.Add(onPoint);
            viewRay.setPts(pts3);


            return true;
        }

        protected override string createLog() {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name).Append("\t");
            sb.Append(this.mPrevPt).Append("\t");
            sb.Append(this.mCurPt);
            return sb.ToString();
        }

    }
}