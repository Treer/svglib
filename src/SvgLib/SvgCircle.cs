using System.Xml;
using Godot;

namespace SvgLib
{
    public sealed class SvgCircle : SvgBasicShape
    {
        private SvgCircle(XmlElement element)
            : base(element)
        {
        }

        internal static SvgCircle Create(XmlElement parent)
        {
            var element = parent.OwnerDocument.CreateElement("circle");
            parent.AppendChild(element);
            return new SvgCircle(element);
        }

        public void SetCenter(double x, double y)
        {
            CX = x;
            CY = y;
        }

        public void SetCenter(Vector2 c)
        {
            CX = c.x;
            CY = c.y;
        }

        public double CX
        {
            get => Element.GetAttribute("cx", SvgDefaults.Attributes.Position.CX);
            set => Element.SetAttribute("cx", value);
        }

        public double CY
        {
            get => Element.GetAttribute("cy", SvgDefaults.Attributes.Position.CY);
            set => Element.SetAttribute("cy", value);
        }

        public double R
        {
            get => Element.GetAttribute("r", SvgDefaults.Attributes.Radius.R);
            set => Element.SetAttribute("r", value);
        }
    }
}
