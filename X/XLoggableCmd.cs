namespace X { 
    public abstract class XLoggableCmd : XExecutable {
        // fields
        protected XApp mApp = null;

        // contructor 
        protected XLoggableCmd(XApp app) {
            this.mApp = app;
        }

        public bool execute() {
            if (this.defineCmd()) {
                this.mApp.getLogMgr().addLog(this.createLog());
                return true;
            } else {
                return false;
            }
        }

        // abstract methods
        protected abstract bool defineCmd();
        protected abstract string createLog();
    }
}
