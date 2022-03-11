using UnityEngine;
using JSI.AppObject;

namespace JSI {
    public class JSICameraRig : JSIAppObject3D {
        // constructor
        public JSICameraRig() : base("CameraRig") {            
        }

        protected override void addComponents() {
            this.mGameObject.AddComponent<Camera>();
        }
    }
}