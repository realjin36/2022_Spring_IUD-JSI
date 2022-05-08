using JSI.AppObject;
using JSI.Geom;
using System.Collections.Generic;
using UnityEngine;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        // singleton pattern
        private static JSIEditStandingCardScenario mSingleton = null;
        public static JSIEditStandingCardScenario getSingleton() {
            Debug.Assert(JSIEditStandingCardScenario.mSingleton != null);
            return JSIEditStandingCardScenario.mSingleton;
        }
        public static JSIEditStandingCardScenario createSingleton(XApp app) {
            Debug.Assert(JSIEditStandingCardScenario.mSingleton == null);
            JSIEditStandingCardScenario.mSingleton =
                new JSIEditStandingCardScenario(app);
            return JSIEditStandingCardScenario.mSingleton;
        }
        private JSIEditStandingCardScenario(XApp app) : base(app) {
            this.mManipulatingTouchMarks = new List<JSITouchMark>();
        }

        protected override void addScenes() {
            this.addScene(JSIEditStandingCardScenario.RotateWithPenScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.MoveWithPenScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.ScaleWithPenScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.MoveWithTouchScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.ScaleWithTouchScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.MoveNRotateWithTouchScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.MoveWithSinglePinchScene.
                createSingleton(this));
            this.addScene(JSIEditStandingCardScenario.MoveWithDoublePinchScene.
                createSingleton(this));
        }

        // fields
        private JSIStandingCard mSelectedStandingCard = null;
        public JSIStandingCard getSelectedStandingCard() {
            return this.mSelectedStandingCard;
        }
        public void setSelectedStandingCard(JSIStandingCard sc) {
            this.mSelectedStandingCard = sc;
        }
        private List<JSITouchMark> mManipulatingTouchMarks = null;
        public List<JSITouchMark> getManipulatingTouchMarks() {
            return this.mManipulatingTouchMarks;
        }
        private JSIStandingCard mManipulaingStandingCardByLeftHand = null;
        public JSIStandingCard getManipulaingStandingCardByLeftHand() {
            return this.mManipulaingStandingCardByLeftHand;
        }
        public void setManipulatingStandingCardByLeftHand(JSIStandingCard sc) {
            this.mManipulaingStandingCardByLeftHand = sc;
        }
        private JSIStandingCard mManipulaingStandingCardByRightHand = null;
        public JSIStandingCard getManipulaingStandingCardByRightHand() {
            return this.mManipulaingStandingCardByRightHand;
        }
        public void setManipulatingStandingCardByRightHand(JSIStandingCard sc) {
            this.mManipulaingStandingCardByRightHand = sc;
        }
        private Vector3 mLastLeftPinchPos;
        public Vector3 getLastLeftPinchPos() {
            return this.mLastLeftPinchPos;
        }
        public void setLastLeftPinchPos(Vector3 pos) {
            this.mLastLeftPinchPos = pos;
        }
        private Vector3 mLastRightPinchPos;
        public Vector3 getLastRightPinchPos() {
            return this.mLastRightPinchPos;
        }
        public void setLastRightPinchPos(Vector3 pos) {
            this.mLastRightPinchPos = pos;
        }

        // methods
        public JSIStandingCard selectStandingCardByStand(JSICursor2D cursor) {
            JSIApp jsi = (JSIApp)this.mApp;
            List<JSIStandingCard> hitStandingCards =
                new List<JSIStandingCard>();
            foreach (JSIStandingCard sc in
                jsi.getStandingCardMgr().getStandingCards()) {
                if (cursor.hits(sc.getStand())) {
                    hitStandingCards.Add(sc);
                }
            }

            if (hitStandingCards.Count == 0) {
                return null;
            }

            // find and return the smallest standing card among the hit ones.
            float minWidth = Mathf.Infinity;
            JSIStandingCard smallestStandingCard = null;
            foreach (JSIStandingCard sc in hitStandingCards) {
                JSIAppRect3D card = sc.getCard();
                JSIRect3D rect = (JSIRect3D)card.getGeom();
                if (rect.getWidth() < minWidth) {
                    smallestStandingCard = sc;
                    minWidth = rect.getWidth();
                }
            }

            return smallestStandingCard;
        }

        public JSIStandingCard selectStandingCardByScaleHandle(
            JSICursor2D cursor) {

            JSIApp jsi = (JSIApp)this.mApp;
            List<JSIStandingCard> hitStandingCards = new List<JSIStandingCard>();
            foreach (JSIStandingCard sc in jsi.getStandingCardMgr().
                getStandingCards()) {

                if (cursor.hits(sc.getScaleHandle())) {
                    hitStandingCards.Add(sc);
                }
            }

            if (hitStandingCards.Count == 0) {
                return null;
            }

            // find and return the smallest standing card among the hit ones.
            float minWidth = Mathf.Infinity;
            JSIStandingCard smallestStandingCard = null;
            foreach (JSIStandingCard sc in hitStandingCards) {
                JSIAppRect3D card = sc.getCard();
                JSIRect3D rect = (JSIRect3D)card.getGeom();
                if (rect.getWidth() < minWidth) {
                    smallestStandingCard = sc;
                    minWidth = rect.getWidth();
                }
            }

            return smallestStandingCard;
        }
    }
}