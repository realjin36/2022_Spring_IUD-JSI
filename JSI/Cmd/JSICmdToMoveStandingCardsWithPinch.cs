using X;
using UnityEngine;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToMoveStandingCardsWithPinch : XLoggableCmd {
        // fields

        // private constructor
        private JSICmdToMoveStandingCardsWithPinch(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToMoveStandingCardsWithPinch cmd =
                new JSICmdToMoveStandingCardsWithPinch(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                (JSIEditStandingCardScenario)JSIEditStandingCardScenario.
                getSingleton();
            JSIHandMgr hm = jsi.getHandMgr();
            
            JSIStandingCard scLeft =
                scenario.getManipulaingStandingCardByLeftHand();
            JSIStandingCard scRight =
                scenario.getManipulaingStandingCardByRightHand();

            if (scLeft == null && scRight == null) {
                return false;
            }

            if (scLeft != null) {
                Vector3 lastPinchPos = scenario.getLastLeftPinchPos();
                Vector3 curPinchPos = hm.getLeftHand().calcPinchPos();
                Vector3 diff = curPinchPos - lastPinchPos;

                Vector3 scPrevPos = scLeft.getGameObject().transform.position;

                scLeft.getGameObject().transform.position =
                    new Vector3(scPrevPos.x + diff.x, scPrevPos.y,
                    scPrevPos.z + diff.z);
                
                scenario.setLastLeftPinchPos(curPinchPos);
            }

            if (scRight != null) {
                Vector3 lastPinchPos = scenario.getLastRightPinchPos();
                Vector3 curPinchPos = hm.getRightHand().calcPinchPos();
                Vector3 diff = curPinchPos - lastPinchPos;

                Vector3 scPrevPos = scRight.getGameObject().transform.position;

                scRight.getGameObject().transform.position =
                    new Vector3(scPrevPos.x + diff.x, scPrevPos.y,
                    scPrevPos.z + diff.z);

                scenario.setLastRightPinchPos(curPinchPos);
            }

            return true;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            return data;
        }
    }
}