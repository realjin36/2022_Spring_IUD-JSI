using System.Text;
using X;
using UnityEngine;

namespace JSI.Cmd {
    public class JSICmdToDoSomething : XLoggableCmd {
        // fields
        // ...

        // private constructor
        private JSICmdToDoSomething(XApp app) : base(app) {
            // JSIApp jsi = (JSIApp)this.mApp;
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToDoSomething cmd = new JSICmdToDoSomething(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            throw new System.NotImplementedException();
        }

        protected override string createLog() {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name).Append("\t");
            // ...
            return sb.ToString();
        }

    }
}