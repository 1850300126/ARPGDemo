using System;
using UnityEngine;

namespace Taco.Timeline
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ColorAttribute : Attribute
    {
        public Color Color;
        public ColorAttribute(float r, float g, float b)
        {
            Color = new Color(r, g, b, 255);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ClipViewAttribute: Attribute
    {
        public string ViewType;
        public ClipViewAttribute(string viewType)
        {
            ViewType = viewType;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class OrderedAttribute : Attribute
    {
        public float Index;
        public OrderedAttribute(float index = 0)
        {
            Index = index;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowInInspectorAttribute : OrderedAttribute
    {
        public ShowInInspectorAttribute(float index = 0) : base(index)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ShowTextAttribute : OrderedAttribute
    {
        public ShowTextAttribute(float index = 0) : base(index)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OnValueChangedAttribute : Attribute
    {
        public string[] Methods;
        public OnValueChangedAttribute(params string[] methods)
        {
            Methods = methods;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : OrderedAttribute
    {
        public string Label; 
        public ButtonAttribute(string label = null, int index = 0) : base(index)
        {
            Label = label;
        }
    }
}