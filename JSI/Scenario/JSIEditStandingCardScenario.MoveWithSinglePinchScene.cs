using System.Collections.Generic;
using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class MoveWithSinglePinchScene : JSIScene {
            // singleton pattern
            private static MoveWithSinglePinchScene mSingleton = null;
            public static MoveWithSinglePinchScene getSingleton() {
                Debug.Assert(MoveWithSinglePinchScene.mSingleton != null);
                return MoveWithSinglePinchScene.mSingleton;
            }
            public static MoveWithSinglePinchScene createSingleton(
                XScenario scenario) {
                Debug.Assert(MoveWithSinglePinchScene.mSingleton == null);
                MoveWithSinglePinchScene.mSingleton = new MoveWithSinglePinchScene(
                    scenario);
                return MoveWithSinglePinchScene.mSingleton;
            }
            private MoveWithSinglePinchScene(XScenario scenario) :
                base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
            }

            public override void handleKeyUp(Key k) {
            }

            public override void handlePenDown(Vector2 pt) {
            }

            public override void handlePenDrag(Vector2 pt) {
            }

            public override void handlePenUp(Vector2 pt) {
            }

            public override void handleEraserDown(Vector2 pt) {
            }

            public override void handleEraserDrag(Vector2 pt) {
            }

            public override void handleEraserUp(Vector2 pt) {
            }

            public override void handleTouchDown() {
            }

            public override void handleTouchDrag() {
            }

            public override void handleTouchUp() {
            }

            public override void handleLeftPinchStart() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHand leftHand = jsi.getHandMgr().getLeftHand();
                Vector3 leftPinchPos = leftHand.calcPinchPos();

                if (JSICmdToSelectStandingCardByPinch.execute(jsi,
                    JSIHand.Handedness.LEFT)) {
                    XCmdToChangeScene.execute(jsi,JSIEditStandingCardScenario.
                        MoveWithDoublePinchScene.getSingleton(), this);
                } else {
                    JSIUtil.createDebugSphere(leftPinchPos);
                }
            }

            public override void handleLeftPinchEnd() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHandMgr hm = jsi.getHandMgr();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                if (scenario.getManipulaingStandingCardByLeftHand() != null) {
                    scenario.setManipulatingStandingCardByLeftHand(null);
                    XCmdToChangeScene.execute(jsi, this.mReturnScene, null);
                }
            }

            public override void handleRightPinchStart() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHand rightHand = jsi.getHandMgr().getRightHand();
                Vector3 rightPinchPos = rightHand.calcPinchPos();

                if (JSICmdToSelectStandingCardByPinch.execute(jsi,
                    JSIHand.Handedness.RIGHT)) {
                    XCmdToChangeScene.execute(jsi,JSIEditStandingCardScenario.
                        MoveWithDoublePinchScene.getSingleton(), this);
                } else {
                    JSIUtil.createDebugSphere(rightPinchPos);
                }
            }

            public override void handleRightPinchEnd() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHandMgr hm = jsi.getHandMgr();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                if (scenario.getManipulaingStandingCardByRightHand() != null) {
                    scenario.setManipulatingStandingCardByRightHand(null);
                    XCmdToChangeScene.execute(jsi, this.mReturnScene, null);
                }
            }

            public override void handleHandsMove() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToMoveStandingCardsWithPinch.execute(jsi);
            }

            public override void getReady() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHandMgr hm = jsi.getHandMgr();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                // deactivate all stands.
                // deactivate all scale handles.
                foreach (JSIStandingCard sc in
                    jsi.getStandingCardMgr().getStandingCards()) {
                    sc.getStand().getGameObject().SetActive(false);
                    sc.getScaleHandle().getGameObject().SetActive(false);
                }

                // activate and highlight only the selected stand.
                JSIStandingCard leftSC =
                    scenario.getManipulaingStandingCardByLeftHand();
                if (leftSC != null) {
                    leftSC.getStand().getGameObject().SetActive(true);
                    leftSC.highlightStand(true);
                }                    

                JSIStandingCard rightSC =
                    scenario.getManipulaingStandingCardByRightHand();
                if (rightSC != null) {
                    rightSC.getStand().getGameObject().SetActive(true);
                    rightSC.highlightStand(true);
                }
            }

            public override void wrapUp() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToTakeSnapshot.execute(jsi);
            }
        }
    }
}