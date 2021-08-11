using System.Collections.Generic;

namespace RoadmapLogic
{
    public sealed class QuarterColors
    {
        public QuarterColors(
            List<int> quarter1, 
            List<int> quarter2,
            List<int> quarter3,
            List<int> quarter4,
            List<int> quarter5,
            List<int> quarter6
        )
        {
            Quarter1 = quarter1;
            Quarter2 = quarter2;
            Quarter3 = quarter3;
            Quarter4 = quarter4;
            Quarter5 = quarter5;
            Quarter6 = quarter6;
        }

        public List<int> Quarter1 
        { 
            get; 
            set; 
        }

        public List<int> Quarter2 
        { 
            get; 
            set; 
        }

        public List<int> Quarter3 
        { 
            get; 
            set; 
        }

        public List<int> Quarter4 
        { 
            get; 
            set; 
        }

        public List<int> Quarter5 
        { 
            get; 
            set; 
        }

        public List<int> Quarter6 
        { 
            get; 
            set; 
        }
    }
}
