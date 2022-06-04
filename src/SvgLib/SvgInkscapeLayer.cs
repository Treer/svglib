using System.Xml;

namespace SvgLib
{
    public sealed class SvgInkscapeLayer : SvgContainer
    {
        const string NamespaceUrlInkscape = "http://www.inkscape.org/namespaces/inkscape";
        private SvgInkscapeLayer(XmlElement element, string label)
            : base(element)
        {
            Label = label;
            Element.SetAttribute("groupmode", NamespaceUrlInkscape, "layer");
        }

        public string Label
        {
            get => Element.GetAttribute("label", NamespaceUrlInkscape);
            set => Element.SetAttribute("label", NamespaceUrlInkscape, value);
        }

        internal static SvgInkscapeLayer Create(XmlElement parent, string label)
        {
            var element = parent.OwnerDocument.CreateElement("g");
            parent.OwnerDocument.DocumentElement.SetAttribute("xmlns:inkscape", NamespaceUrlInkscape);
            parent.AppendChild(element);

            return new SvgInkscapeLayer(element, label);
        }
    }
}
