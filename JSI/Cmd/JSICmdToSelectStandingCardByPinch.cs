using X;
using JSI.Scenario;
using UnityEngine;
using JSI.Geom;

namespace JSI.Cmd {
    public class JSICmdToSelectStandingCardByPinch : XLoggableCmd {
        // constants
        private static readonly float PINCH_SELECT_THRESHOLD = 0.025f;

        // fields
        private JSIHand.Handedness mHandness;
        private JSIStandingCard mSelectedStandingCard = null;

        // private constructor
        private JSICmdToSelectStandingCardByPinch(XApp app,
            JSIHand.Handedness handedness) : base(app) {

            JSIApp jsi = (JSIApp)this.mApp;
            this.mHandness = handedness;
        }

        // static method to construct and execute this command
        public static bool execute(XApp app, JSIHand.Handedness handedness) {
            JSICmdToSelectStandingCardByPinch cmd =
                new JSICmdToSelectStandingCardByPinch(app, handedness);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIHandMgr hm = jsi.getHandMgr();
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();

            JSIHand hand;
            if (this.mHandness == JSIHand.Handedness.LEFT) {
                hand = hm.getLeftHand();
            } else {
                hand = hm.getRightHand();
            }

            float nearestDist = float.MaxValue;

            foreach (JSIStandingCard sc in jsi.getStandingCardMgr().getStandingCards()) {
                if (scenario.getManipulaingStandingCardByLeftHand() == sc ||
                    scenario.getManipulaingStandingCardByRightHand() == sc) {
                    continue;
                }
                float dist = this.calcDistFromPinchToSC(hand, sc);
                if (dist < nearestDist) {
                    nearestDist = dist;
                    this.mSelectedStandingCard = sc;
                }
            }

            Debug.LogWarning($"NearestDist = {nearestDist}");

            if (nearestDist > PINCH_SELECT_THRESHOLD) {
                return false;
            }

            if (this.mHandness == JSIHand.Handedness.LEFT) {
                scenario.setManipulatingStandingCardByLeftHand(this.mSelectedStandingCard);
                scenario.setLastLeftPinchPos(hand.calcPinchPos());
            } else {
                scenario.setManipulatingStandingCardByRightHand(this.mSelectedStandingCard);
                scenario.setLastRightPinchPos(hand.calcPinchPos());
            }
            return true;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            data.addMember("cardId", this.mSelectedStandingCard.getId());
            return data;
        }

        // return float.MaxValue if the distance is bigger than the threshold.
        // return float.MaxValue if the projected pt on plane is out of card.
        // Otherwise, return the normal dist.
        private float calcDistFromPinchToSC(JSIHand hand, JSIStandingCard sc) {
            Vector3 pinchPos = hand.calcPinchPos();
            JSIRect3D rect = (JSIRect3D)sc.getCard().getGeom();
            Vector3 cardNormal = sc.getGameObject().transform.rotation * Vector3.forward;
            Vector3 cardWDir = sc.getGameObject().transform.rotation * Vector3.right;
            Vector3 cardHDir = sc.getGameObject().transform.rotation * Vector3.up;
            Vector3 cardCenter = sc.getGameObject().transform.position;

            Plane plane = new Plane(cardNormal, cardCenter);
            float normalDist = Mathf.Abs(plane.GetDistanceToPoint(pinchPos));
            if (normalDist > PINCH_SELECT_THRESHOLD) {
                Debug.LogWarning($"It's too far from plane : {normalDist}");
                return float.MaxValue;
            }

            Vector3 projPt = plane.ClosestPointOnPlane(pinchPos);
            Vector3 projDelta = projPt - cardCenter;
            
            Vector3 dw = Vector3.Project(projDelta, cardWDir);
            if (dw.magnitude > rect.getWidth() * 0.5f + PINCH_SELECT_THRESHOLD) {
                Debug.LogWarning($"It exceeds along width direction : {dw.magnitude}");
                return float.MaxValue;
            }
            Vector3 dh = Vector3.Project(projDelta, cardHDir);
            if (dh.magnitude > rect.getHeight() * 0.5f + PINCH_SELECT_THRESHOLD) {
                Debug.LogWarning($"It exceeds along height direction : {dh.magnitude}");
                return float.MaxValue;
            }

            return normalDist;
        }
    }
}