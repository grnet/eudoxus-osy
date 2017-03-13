using System;

namespace EudoxusOsy.BusinessModel
{
    public class PropertyChange
    {
        public string PropertyName { get; set; }

        [NonSerialized]
        private object oldV = default(object);
        public object OldValue
        {
            get { return oldV; }
            set
            {
                if (value == null || value is DBNull) oldV = "NULL";
                else oldV = value;
            }
        }


        [NonSerialized]
        private object newV = default(object);
        public object NewValue
        {
            get { return newV; }
            set
            {
                if (value == null || value is DBNull) newV = "NULL";
                else newV = value;
            }
        }

        public PropertyChange()
        {
            PropertyName = string.Empty;
            OldValue = default(object);
            NewValue = default(object);
        }

        public PropertyChange(object oldVal, object newVal)
        {
            OldValue = oldVal;
            NewValue = newVal;
        }

        public PropertyChange(object oldVal, object newVal, string property)
        {
            PropertyName = property;
            OldValue = oldVal;
            NewValue = newVal;
        }

    }
}
