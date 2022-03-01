using System.Text;
using X;
using UnityEngine;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToSelectSmallestStandingCardByStand : 
        XLoggableCmd {
        // fields
        JSIStandingCard mSelectedStandingCard = null;

        // private constructor
        private JSICmdToSelectSmallestStandingCardByStand(XApp app) : 
            base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToSelectSmallestStandingCardByStand cmd = 
                new JSICmdToSelectSmallestStandingCardByStand(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();
            this.mSelectedStandingCard = scenario.selectStandingCardByStand();
            scenario.setSelectedStandingCard(this.mSelectedStandingCard);
            return true;
        }

        protected override string createLog() {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name).Append("\t");
            sb.Append(this.mSelectedStandingCard);
            return sb.ToString();
        }

    }
}