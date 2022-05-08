using System.Collections.Generic;
using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class MoveWithDoublePinchScene : JSIScene {
            // singleton pattern
            private static MoveWithDoublePinchScene mSingleton = null;
            public static MoveWithDoublePinchScene getSingleton() {
                Debug.Assert(MoveWithDoublePinchScene.mSingleton != null);
                return MoveWithDoublePinchScene.mSingleton;
            }
            public static MoveWithDoublePinchScene createSingleton(
                XScenario scenario) {
                Debug.Assert(MoveWithDoublePinchScene.mSingleton == null);
                MoveWithDoublePinchScene.mSingleton = new MoveWithDoublePinchScene(
                    scenario);
                return MoveWithDoublePinchScene.mSingleton;
            }
            private MoveWithDoublePinchScene(XScenario scenario) :
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
            }

            public override void handleLeftPinchEnd() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHandMgr hm = jsi.getHandMgr();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                scenario.setManipulatingStandingCardByLeftHand(null);
                XCmdToChangeScene.execute(jsi, JSIEditStandingCardScenario.
                    MoveWithSinglePinchScene.getSingleton(), this.mReturnScene);
            }

            public override void handleRightPinchStart() {
            }

            public override void handleRightPinchEnd() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIHandMgr hm = jsi.getHandMgr();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                scenario.setManipulatingStandingCardByRightHand(null);
                XCmdToChangeScene.execute(jsi, JSIEditStandingCardScenario.
                    MoveWithSinglePinchScene.getSingleton(), this.mReturnScene);
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

                // activate and highlight only the selected stands.
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