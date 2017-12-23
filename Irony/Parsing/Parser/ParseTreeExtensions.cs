#region License
/* **********************************************************************************
 * Copyright (c) Roman Ivantsov
 * This source code is subject to terms and conditions of the MIT License
 * for Irony. A copy of the license can be found in the License.txt file
 * at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * MIT License.
 * You must not remove this notice from this software.
 * **********************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Irony.Parsing
{
#if !SILVERLIGHT
    public static class ParseTreeExtensions
    {

        public static string ToXml(this ParseTree parseTree)
        {
            if (parseTree == null || parseTree.Root == null) return string.Empty;
            var xdoc = ToXmlDocument(parseTree);
            return xdoc.ToString();
        }

        public static XDocument ToXmlDocument(this ParseTree parseTree)
        {
            var xdoc = new XDocument();
            if (parseTree == null || parseTree.Root == null) return xdoc;
            var xTree = new XElement("ParseTree");
            xdoc.Add(xTree);
            var xRoot = parseTree.Root.ToXmlElement(xdoc);
            xTree.Add(xRoot);
            return xdoc;
        }

        public static XElement ToXmlElement(this ParseTreeNode node, XDocument ownerDocument)
        {
            var xElem = new XElement("Node");
            xElem.Add(new XAttribute("Term", node.Term.Name));
            var term = node.Term;
            if (term.HasAstConfig() && term.AstConfig.NodeType != null)
                xElem.Add(new XAttribute("AstNodeType", term.AstConfig.NodeType.Name));

            if (node.Token != null)
            {
                xElem.Add(new XAttribute("Terminal", node.Term.GetType().Name));
                //xElem.SetAttribute("Text", node.Token.Text);
                if (node.Token.Value != null)
                    xElem.Add(new XAttribute("Value", node.Token.Value.ToString()));
            }
            else
                foreach (var child in node.ChildNodes)
                {
                    var xChild = child.ToXmlElement(ownerDocument);
                    xElem.Add(xChild);
                }
            return xElem;
        }//method

    }//class
#endif
}//namespace
