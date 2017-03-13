namespace EudoxusOsy.BusinessModel
{
    public static class CriteriaField
    {
        public static class Operators
        {
            public new const string Equals = "=";
            public const string NotEquals = "!=";
            public const string NotNull = "IS NOT NULL";
            public const string Null = "IS NULL";
            public const string Or = "OR";
            public const string And = "AND";
        }
    }

    public class CriteriaField<T>
    {
        bool _isEmpty = true;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { _isEmpty = value; }
        }

        T _FieldValue;
        public T FieldValue
        {
            get { return _FieldValue; }
            set
            {
                _FieldValue = value;
                IsEmpty = value == null;
            }
        }

        string _Operator = CriteriaField.Operators.Equals;
        public string Operator
        {
            get { return _Operator; }
            set
            {
                if (value == CriteriaField.Operators.Null || value == CriteriaField.Operators.NotNull)
                    IsEmpty = false;
                _Operator = value;
            }
        }
    }
}
