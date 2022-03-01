using System.Collections.Generic;

namespace JSI {
    public class JSIStandingCardMgr {
        // fields
        private List<JSIStandingCard> mStandingCards = null;
        public List<JSIStandingCard> getStandingCards() {
            return this.mStandingCards;
        }

        // constructor
        public JSIStandingCardMgr() {
            this.mStandingCards = new List<JSIStandingCard>();
        }
    }
}