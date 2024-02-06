using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataAnnotationsValidationExtensions
{
    public class ExtensionForDataAnnotations : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public ExtensionForDataAnnotations(string errorMessage) : base(errorMessage) { }
        public ExtensionForDataAnnotations(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected ExtensionForDataAnnotations(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }

        public class RangeIfAttribute : ValidationAttribute
        {
            RangeAttribute _innerAttribute;
            public string PropertyName { get; set; }
            public object Value { get; set; }
            public bool IsEquals { get; set; }

            public RangeIfAttribute(string propertyName, object value, int min, int max, bool isEquals = true)
            {
                PropertyName = propertyName;
                Value = value;
                IsEquals = isEquals;
                _innerAttribute = new RangeAttribute(min, max);
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext is null");
                }

                PropertyInfo property = validationContext.ObjectType.GetProperty(this.PropertyName);
                if (validationContext == null)
                {
                    throw new ArgumentNullException("property is null");
                }

                object propertyValue = property.GetValue(validationContext.ObjectInstance);

                if (propertyValue != null && (IsEquals && propertyValue.Equals(Value) || (!IsEquals && !propertyValue.Equals(Value))))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        string specificErrorMessage = ErrorMessage;
                        if (specificErrorMessage.Length < 1)
                            specificErrorMessage = $"{name} is required.";

                        return new ValidationResult(specificErrorMessage, new[] { validationContext.MemberName });
                    }

                }
                return ValidationResult.Success;
            }

        }

        public class RequiredAny : ValidationAttribute
        {
            RequiredAttribute _innerAttribute = new RequiredAttribute();
            public bool IsEverything { get; set; }
            public string[] PropertyNames { get; set; }

            public RequiredAny(string[] propertyNames)
            {
                PropertyNames = propertyNames;
            }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext is null");
                }

                var propertyValues = new List<object>();
                foreach (var propertyName in PropertyNames)
                {
                    PropertyInfo property = validationContext.ObjectType.GetProperty(propertyName);
                    if (validationContext == null)
                    {
                        throw new ArgumentNullException("property is null");
                    }

                    propertyValues.Add(property.GetValue(validationContext.ObjectInstance));
                }

                bool hasEmpty = propertyValues.Any(r => r != null && !String.IsNullOrWhiteSpace(r.ToString()) && r.ToString() != "0");
                if ((IsEverything && hasEmpty) || (!IsEverything && !hasEmpty))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        string specificErrorMessage = ErrorMessage;
                        if (specificErrorMessage.Length < 1)
                            specificErrorMessage = $"{name} is required.";

                        return new ValidationResult(specificErrorMessage, PropertyNames);
                    }

                }
                return ValidationResult.Success;
            }
        }
       
        public class ValidateObjectAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if(value is null)
                {
                    return new ValidationResult(ErrorMessage, new string[] { validationContext.MemberName });
                }
                var validationResult = new ExtensionForDataAnnotations(String.Format(ErrorMessage));
                var result = new List<ValidationResult>();

                ValidateOption(value, validationContext, result);

                if (result.Count != 0)
                {
                    result.ForEach(validationResult.AddResult);
                    return validationResult;
                }

                return ValidationResult.Success;
            }
        }

        public class ValidateListObjectsAttribute : ValidationAttribute
        {
            public string IterationText { get; set; }
            public string IsEmptyError { get; set; }
            public bool IsCanEmpty { get; set; } = true;
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var validationResult = new ExtensionForDataAnnotations(String.Format(ErrorMessage), new string[] { validationContext.MemberName });
                var values = value as IList;
                if (values == null || values.Count == 0)
                {
                    if(IsCanEmpty)
                    {
                        return ValidationResult.Success;
                    }
                    return new ValidationResult(IsEmptyError, new string[] { validationContext.MemberName });
                }

                for (int i = 0; i < values.Count; i++)
                {
                    var result = new List<ValidationResult>();
                    ValidateOption(values[i], validationContext, result);

                    if (result.Count != 0)
                    {
                        if (!String.IsNullOrWhiteSpace(IterationText))
                        {
                            validationResult.AddResult(new ValidationResult(IterationText + (i + 1), new string[] { validationContext.MemberName }));
                        }
                        result.ForEach(validationResult.AddResult);
                    }
                }

                if (validationResult.Results.Any())
                    return validationResult;

                return ValidationResult.Success;
            }
        }

        public class RequiredIfAttribute : ValidationAttribute
        {
            RequiredAttribute _innerAttribute = new RequiredAttribute();
            public string PropertyName { get; set; }
            public object Value { get; set; }
            public bool IsEquals { get; set; }

            public RequiredIfAttribute(string propertyName, object value, bool isEquals = true)
            {
                PropertyName = propertyName;
                Value = value;
                IsEquals = isEquals;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext is null");
                }

                PropertyInfo property = validationContext.ObjectType.GetProperty(this.PropertyName);
                if (validationContext == null)
                {
                    throw new ArgumentNullException("property is null");
                }

                object propertyValue = property.GetValue(validationContext.ObjectInstance);

                if (propertyValue != null && (IsEquals && propertyValue.Equals(Value) || (!IsEquals && !propertyValue.Equals(Value))))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        string specificErrorMessage = ErrorMessage;
                        if (specificErrorMessage.Length < 1)
                            specificErrorMessage = $"{name} is required.";

                        return new ValidationResult(specificErrorMessage, new[] { validationContext.MemberName });
                    }

                }
                return ValidationResult.Success;
            }
        }

        public class InnIfAttribute : ValidationAttribute
        {
            InnAttribute _innerAttribute = new InnAttribute();
            public string PropertyName { get; set; }
            public object Value { get; set; }
            public bool IsEquals { get; set; }

            public InnIfAttribute(string propertyName, object value, bool isEquals = true)
            {
                PropertyName = propertyName;
                Value = value;
                IsEquals = isEquals;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext is null");
                }

                PropertyInfo property = validationContext.ObjectType.GetProperty(this.PropertyName);

                if (validationContext == null)
                {
                    throw new ArgumentNullException("property is null");
                }

                object propertyValue = property.GetValue(validationContext.ObjectInstance);

                if (propertyValue != null && (IsEquals && propertyValue.Equals(Value) || (!IsEquals && !propertyValue.Equals(Value))))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        string specificErrorMessage = ErrorMessage;
                        if (specificErrorMessage.Length < 1)
                            specificErrorMessage = $"{name} is required.";

                        return new ValidationResult(specificErrorMessage, new[] { validationContext.MemberName });
                    }
                }
                return ValidationResult.Success;
            }

            public class InnAttribute : ValidationAttribute
            {
                public override bool IsValid(object value)
                {
                    return value != null && DataAnnotationsValidationExtensions.Helper.Validator.IsValidINN(value.ToString());
                }
            }
        }
        public static void ValidateOption(object option, ValidationContext validationContext,
         ICollection<ValidationResult> results)
        {
            Validator.TryValidateObject(option,
              new ValidationContext(option, validationContext, validationContext.Items),
              results, true);
        }
    }
}
