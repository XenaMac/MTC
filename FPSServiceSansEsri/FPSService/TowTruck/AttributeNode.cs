using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FPSService.TowTruck
{
    public class AttributeNode
    {
            public String Attribute { set; get; }
            public String Value { set; get; }
            public AttributeNode ChildAttributes { set; get; }
            public AttributeNode NextAttribute { set; get; }

            public AttributeNode()
            {
                Attribute = "";
                Value = "";
                ChildAttributes = null;
                NextAttribute = null;
            }
    }
}