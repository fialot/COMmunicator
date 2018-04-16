using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace COMunicator
{
    /// <summary>
    /// Holds information about a property in a Dictionary-based PropertyGrid
    /// </summary>
    public class GridProperty
    {
        public GridProperty(string name)
        { Name = name; }
        public GridProperty(string name, string category)
        { Name = name; Category = category; }
        public GridProperty(string name, string category, string description)
        { Name = name; Category = category; Description = description; }

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Description { get; set; }
        public bool IsReadOnly { get; set; }
        public object DefaultValue { get; set; } // shown if value is null

        public static implicit operator GridProperty(string name) { return new GridProperty(name); }
    }

    /// <summary>An object that wraps a dictionary so that it can be used as the
    /// SelectedObject property of a standard PropertyGrid control.</summary>
    /// <example>
    /// propertyGrid.SelectedObject = new DictionaryPropertyGridAdapter(dict, "");
    /// </example>
    public class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        internal IDictionary<GridProperty, object> _dictionary;
        internal string _defaultCategory;

        public DictionaryPropertyGridAdapter(Dictionary<GridProperty, object> dict, string defaultCategory)
        {
            _dictionary = dict;
            _defaultCategory = defaultCategory;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var props = new PropertyDescriptor[_dictionary.Count];
            int i = 0;
            foreach (var prop in _dictionary)
                props[i++] = new GridPropertyDescriptor(prop.Key, this);
            return new PropertyDescriptorCollection(props);
        }

        #region Boilerplate

        #region Never called
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
        PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            return GetProperties(null);
        }
        #endregion

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _dictionary;
        }
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        #endregion

        class GridPropertyDescriptor : PropertyDescriptor
        {
            GridProperty _prop;
            DictionaryPropertyGridAdapter _parent;

            internal GridPropertyDescriptor(GridProperty prop, DictionaryPropertyGridAdapter parent)
                : base(prop.Name, null)
            {
                _prop = prop;
                _parent = parent;
            }
            public override string Description
            {
                get { return _prop.Description; }
            }
            public override string Category
            {
                get { return _prop.Category ?? _parent._defaultCategory; }
            }
            public override Type PropertyType
            {
                get { return (_parent._dictionary[_prop] ?? _prop.DefaultValue ?? "").GetType(); }
            }
            public override void SetValue(object component, object value)
            {
                _parent._dictionary[_prop] = value;
            }
            public override object GetValue(object component)
            {
                return _parent._dictionary[_prop];
            }
            public override bool IsReadOnly
            {
                get { return _prop.IsReadOnly; }
            }
            public override Type ComponentType
            {
                get { return null; }
            }
            public override bool CanResetValue(object component)
            {
                return _prop.DefaultValue != null;
            }
            public override void ResetValue(object component)
            {
                SetValue(component, _prop.DefaultValue);
            }
            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }
    }
}
