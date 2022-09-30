using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentricProjectTeam4.Models
{
    public class LeaderBoardInfo
    {
        public string employee { get; set; }

        public int numRecognitions { get; set; }

        public int numDeliveryRecs { get; set; }

        public int numIntegrityRecs { get; set; }

        public int numStewardshipRecs { get; set; }

        public int numCultureRecs { get; set; }

        public int numGoodRecs { get; set; }

        public int numInnovationRecs { get; set; }

        public int numBalancedRecs { get; set; }
    }
}