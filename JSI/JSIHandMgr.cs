using UnityEngine;

namespace JSI {
    public class JSIHandMgr {
        // fields
        private JSIHand mLeftHand = null;
        public JSIHand getLeftHand() {
            return this.mLeftHand;
        }
        private JSIHand mRightHand = null;
        public JSIHand getRightHand() {
            return this.mRightHand;
        }

        private JSIHand.Handedness mPinchingHands = 0;
        public JSIHand.Handedness getPinchingHands() {
            return this.mPinchingHands;
        }

        // constructor
        public JSIHandMgr(GameObject leftHandPrefab, GameObject rightHandPrefab) {
            this.mLeftHand = new JSIHand(leftHandPrefab);
            this.mRightHand = new JSIHand(rightHandPrefab);
        }

        // methods
        public bool pinchStart(JSIHand.Handedness handedness) {
            // if (this.mPinchingHands.HasFlag(handedness)) {
            //     return false;
            // }
            
            this.mPinchingHands |= handedness;
            return true;
        }

        public bool pinchEnd(JSIHand.Handedness handedness) {
            // if (!this.mPinchingHands.HasFlag(handedness)) {
            //     return false;
            // }
            
            this.mPinchingHands &= ~handedness;
            return true;
        }

        public bool isLeftHandPinching() {
            return this.mPinchingHands.HasFlag(JSIHand.Handedness.LEFT);
        }
        public bool isRightHandPinching() {
            return this.mPinchingHands.HasFlag(JSIHand.Handedness.RIGHT);
        }
    }
}