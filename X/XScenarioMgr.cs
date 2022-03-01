using System.Collections.Generic;

namespace X {
    public abstract class XScenarioMgr {
        // fields
        protected XApp mApp = null;
        protected List<XScenario> mScenarios = null;
        protected XScene mCurScene = null;
        public XScene getCurScene() {
            return this.mCurScene;
        }
        public void setCurScene(XScene scene) {
            if (this.mCurScene != null) {
                this.mCurScene.wrapUp();
            }
            scene.getReady();
            this.mCurScene = scene;
        }

        // contructor
        protected XScenarioMgr(XApp app) {
            this.mApp = app;
            this.mScenarios = new List<XScenario>();
            this.addScenarios();
            this.setInitCurScene();
        }

        // abstract methods
        protected abstract void addScenarios();
        protected abstract void setInitCurScene();

        // concrete methods
        protected void addScenario(XScenario scenario) {
            this.mScenarios.Add(scenario);
        }
    }
}
