using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeOfLife.OTT
{
    class OTT
    {
        //--------------------------------------------------------------------------------------
        // Returns a hyperlink to the taxonomy browser for a given OTT taxon
        public static string getTaxonLink( uint ottID, string displayName = null) 
        {
            // ASSUMES we will always have the ottid, else check for unique name
            if (ottID == 0) 
                return displayName;
    
            // empty or missing name? show the raw ID
            if (displayName == null) 
                displayName = "OTT: {OTT_ID}".Replace("OTT_ID",ottID.ToString());
            
            string link = "<a href=\"{TAXO_BROWSER_URL}\" title=\"OTT Taxonomy\" target=\"taxobrowser\">{DISPLAY_NAME}</a>";
            return link.Replace("{TAXO_BROWSER_URL}", getTaxobrowserURL(ottID)).Replace("{DISPLAY_NAME}", displayName);
        }

        //--------------------------------------------------------------------------------------
        // Returns a bare URL to the taxonomy browser for a given OTT taxon
        public static string getTaxobrowserURL(uint ottID) 
        {
            if (ottID == 0) 
                return null;
    
            string url = "/taxonomy/browse?id={OTT_ID}";
            return url.Replace("{OTT_ID}", ottID.ToString());
        }
    }
}
