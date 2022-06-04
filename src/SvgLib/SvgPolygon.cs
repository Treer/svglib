using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using Godot;

namespace SvgLib
{
    public sealed class SvgPolygon : SvgElement
    {
        private SvgPolygon(XmlElement element)
            : base(element)
        {
        }

        internal static SvgPolygon Create(XmlElement parent)
        {
            var element = parent.OwnerDocument.CreateElement("polygon");
            parent.AppendChild(element);
            return new SvgPolygon(element);
        }

        public IEnumerable<Vector2> Points
        {
            get
            {
                var stringArray = Element.GetAttribute("points");
                var coordEnumerator = stringArray
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .AsEnumerable()
                    .GetEnumerator();

                while (coordEnumerator.MoveNext())
                {
                    float x = float.Parse(coordEnumerator.Current);
                    if (!coordEnumerator.MoveNext()) { break; }
                    yield return new Vector2(x, float.Parse(coordEnumerator.Current));
                }
            }
            set
            {
                var points = string.Join(", ", value.Select(v => $"{v.x.ToString("G", CultureInfo.InvariantCulture)},{v.y.ToString("G", CultureInfo.InvariantCulture)}"));
                Element.SetAttribute("points", points);
            }
        }

        public double[] Points_double
        {
            get
            {
                var stringArray = Element.GetAttribute("points");
                return stringArray
                    .Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(value => double.Parse(value, CultureInfo.InvariantCulture))
                    .ToArray();
            }
            set
            {
                var points = string.Join(", ", value.Select(x => x.ToString("G", CultureInfo.InvariantCulture)));
                Element.SetAttribute("points", points);
            }
        }

        public SvgFillRule FillRule
        {
            get => Element.GetAttribute<SvgFillRule>("fill-rule", SvgDefaults.Attributes.FillAndStroke.FillRule);
            set => Element.SetAttribute("fill-rule", value);
        }
    }
}
