using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace SvgLib
{
    public abstract class SvgElement
    {
        protected readonly XmlElement Element;

        protected SvgElement(XmlElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public string Id
        {
            get => Element.GetAttribute("id");
            set => Element.SetAttribute("id", value);
        }

        public int? TabIndex
        {
            get => Element.GetAttribute("tabindex", (int?)null);
            set => Element.SetAttribute("tabindex", value);
        }

        // TODO Add https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/Presentation

        public Color Fill
        {
            get => HexToColor(Element.GetAttribute("fill", SvgDefaults.Attributes.FillAndStroke.Fill));
            set => Element.SetAttribute("fill", ColorToHex(value));
        }

        public double FillOpacity
        {
            get => Element.GetAttribute("fill-opacity", SvgDefaults.Attributes.FillAndStroke.FillOpacity);
            set => Element.SetAttribute("fill-opacity", value);
        }

        public Color Stroke
        {
            get => HexToColor(Element.GetAttribute("stroke", SvgDefaults.Attributes.FillAndStroke.Stroke));
            set => Element.SetAttribute("stroke", ColorToHex(value));
        }

        public double StrokeOpacity
        {
            get => Element.GetAttribute("stroke-opacity", SvgDefaults.Attributes.FillAndStroke.StrokeOpacity);
            set => Element.SetAttribute("stroke-opacity", value);
        }

        public double StrokeWidth
        {
            get => Element.GetAttribute("stroke-width", SvgDefaults.Attributes.FillAndStroke.StrokeWidth);
            set => Element.SetAttribute("stroke-width", value);
        }

        public SvgStrokeLineCap StrokeLineCap
        {
            get => Element.GetAttribute<SvgStrokeLineCap>("stroke-linecap", SvgDefaults.Attributes.FillAndStroke.StrokeLineCap);
            set => Element.SetAttribute("stroke-linecap", value);
        }

        public string Transform
        {
            get => Element.GetAttribute("transform");
            set => Element.SetAttribute("transform", value);
        }

        public bool Visible
        {
            get => GetStyle("display") != "none";
            set => SetStyle("display", value ? string.Empty : "none");
        }

        public IEnumerable<string> GetClasses() => ParseClassAttribute();

        public bool HasClass(string name) => GetClasses().Contains(name);

        public void AddClass(string name)
        {
            var classes = ParseClassAttribute();
            classes.Add(name);
            SetClassAttribute(classes);
        }

        public void RemoveClass(string name)
        {
            var classes = ParseClassAttribute();
            classes.Remove(name);
            SetClassAttribute(classes);
        }

        public void ToggleClass(string name)
        {
            if (HasClass(name)) RemoveClass(name);
            else AddClass(name);
        }

        protected string GetStyle(string name)
        {
            var styles = ParseStyleAttribute();
            return styles[name];
        }

        protected void SetStyle(string name, string value)
        {
            var styles = ParseStyleAttribute();
            styles[name] = value;
            SetStyleAttribute(styles);
        }

        private HashSet<string> ParseClassAttribute()
            => new HashSet<string>(Element.GetAttribute("class").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

        private void SetClassAttribute(IEnumerable<string> classes)
        {
            if (classes == null || !classes.Any())
            {
                Element.RemoveAttribute("class");
                return;
            }

            var value = string.Join(" ", classes);
            Element.SetAttribute("class", value);
        }

        private Dictionary<string, string> ParseStyleAttribute()
            => Element.GetAttribute("style")
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { ':' }))
                .Where(x => x.Length == 2)
                .ToDictionary(x => x[0].Trim(), x => x[1].Trim(), StringComparer.OrdinalIgnoreCase);

        private void SetStyleAttribute(IReadOnlyDictionary<string, string> styles)
        {
            if (styles == null || !styles.Any())
            {
                Element.RemoveAttribute("style");
                return;
            }

            var value = string.Join(";", styles.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
            Element.SetAttribute("style", value);
        }

        private static string ColorToHex(Color c)
        {
            return c == Color.Empty ? "none" : "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        } 
        private static Color HexToColor(string hex)
        {
            hex = hex?.Trim().ToLower() ?? string.Empty;
            if (string.IsNullOrEmpty(hex) || hex == "none") { return Color.Empty; }

            int argb = int.Parse(hex.Replace("#", ""), NumberStyles.HexNumber);
            return Color.FromArgb(argb);
        }        
    }
}
