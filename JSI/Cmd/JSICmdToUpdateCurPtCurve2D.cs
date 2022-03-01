using System.Text;
using X;
using UnityEngine;
using System.Collections.Generic;
using JSI.AppObject;

namespace JSI.Cmd {
    public class JSICmdToUpdateCurPtCurve2D : XLoggableCmd {
        // fields
        private Vector2 mPt = JSIUtil.VECTOR2_NAN;

        // private constructor
        private JSICmdToUpdateCurPtCurve2D(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            this.mPt = jsi.getPenMarkMgr().getLastPenMark().getLastPt();
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToUpdateCurPtCurve2D cmd = 
                new JSICmdToUpdateCurPtCurve2D(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIAppPolyline2D curPtCurve2D = 
                jsi.getPtCurve2DMgr().getCurPtCurve2D();
            curPtCurve2D.setPts(jsi.getPenMarkMgr().getLastPenMark().getPts());                
            return true;
        }

        protected override string createLog() {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name).Append("\t");
            sb.Append(this.mPt);
            return sb.ToString();
        }

    }
}