using System.Collections.Generic;
using UnityEngine;

namespace X {
    public class XLogMgr {
        // fields
        private List<string> mLogs = null;
        public List<string> getLogs() {
            return this.mLogs;
        }
        private bool mPrintOn = false;
        public bool isPrintOn() {
            return this.mPrintOn;
        }
        public void setPrintOn(bool isPrintOn) {
            this.mPrintOn = isPrintOn;
        }

        // constructor 
        public XLogMgr() {
            this.mLogs = new List<string>();
        }

        public void addLog(string log) {
            this.mLogs.Add(log);
            if (this.mPrintOn) {
                Debug.Log(log);
            }
        }
    }
}
