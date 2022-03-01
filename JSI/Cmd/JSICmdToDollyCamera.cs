using System.Text;
using X;
using UnityEngine;
using JSI.AppObject;
using System.Collections.Generic;

namespace JSI.Cmd {
    public class JSICmdToDollyCamera : XLoggableCmd {
        // fields
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;
        private static JSIAppRect3D mPivotPlane = new JSIAppRect3D("pivotPlane", 5f, 5f, new Color(0.855f, 0.988f, 1f, 0.157f));

        // private constructor
        private JSICmdToDollyCamera(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIPenMark penMark = jsi.getPenMarkMgr().getLastPenMark();
            this.mPrevPt = penMark.getRecentPt(1);
            this.mCurPt = penMark.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToDollyCamera cmd = new JSICmdToDollyCamera(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIPerspCameraPerson cp = jsi.getPerspCameraPerson();

            // create a plane on the pivot, directly facing the camera.
            Plane pivotPlane = new Plane(-cp.getView(), cp.getPivot());

            // project the previous screen point to the plane. 
            Ray prevPtRay = cp.getCamera().ScreenPointToRay(this.mPrevPt);
            float prevPtDist = float.NaN;
            pivotPlane.Raycast(prevPtRay, out prevPtDist);
            Vector3 prevPtOnPlane = prevPtRay.GetPoint(prevPtDist);

            // project the current screen point to the plane. 
            Ray curPtRay = cp.getCamera().ScreenPointToRay(this.mCurPt);
            float curPtDist = float.NaN;
            pivotPlane.Raycast(curPtRay, out curPtDist);
            Vector3 curPtOnPlane = curPtRay.GetPoint(curPtDist);

            // calculate the position difference between the two points.
            Vector3 offset = curPtOnPlane - prevPtOnPlane;

            // update the postion of the camera.
            cp.setEye(cp.getEye() - offset);

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
            // Plane ground = new Plane(Vector3.up, Vector3.zero);
            float rayDist = float.NaN;
            pivotPlane.Raycast(ray, out rayDist);
            if(rayDist > 1e4f || rayDist < 0) {
                rayDist = 1000f;
            }
            Vector3 onPoint = ray.GetPoint(rayDist);
            pts3.Add(onPoint);
            viewRay.setPts(pts3);

            // if (mPivotPlane != null) {
            //     mPivotPlane.destroyGameObject();
            // }
            // Color col = new Color(0.855f, 0.988f, 1f, 0.157f);
            // mPivotPlane = new JSIAppRect3D("pivotPlane", 5f, 5f, col);
            mPivotPlane.getGameObject().transform.rotation = Quaternion.FromToRotation(Vector3.back, pivotPlane.normal);

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