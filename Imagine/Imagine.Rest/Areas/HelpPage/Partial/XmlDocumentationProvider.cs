using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace Imagine.Rest.Areas.HelpPage {
  public partial class XmlDocumentationProvider : IDocumentationProvider {
    private const string AttributeExpression = "/doc/members/member[@name='P:{0}']";

    public string GetResponseAttributeDocumentation(String attributeName) {
      XPathNavigator methodNode = GetAttributeNode(attributeName);
      return GetTagValue(methodNode, "summary");
    }

    private XPathNavigator GetAttributeNode(String attributeName) {
        string selectExpression = String.Format(CultureInfo.InvariantCulture, AttributeExpression, attributeName);
        return _documentNavigator.SelectSingleNode(selectExpression);
    }

  }
}