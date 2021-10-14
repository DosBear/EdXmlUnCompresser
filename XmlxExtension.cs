using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WindowsFormsApplication1
{
    static class XmlxExtension
    {
        public static string get(this XmlDocument x, string node)
        {
            XmlNode xnode = x.SelectSingleNode("//*[local-name()='" + node + "']");
            return xnode == null ? string.Empty : xnode.InnerText;
        }
    }
}
