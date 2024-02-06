using DataAnnotationsValidationExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Model;

namespace Tests
{
    public static class Helper
    {
        /// <summary>
        /// TODO: Уйти от рекурсии
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationResults"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<ValidationError> GetErrors<T>(
            ICollection<System.ComponentModel.DataAnnotations.ValidationResult> validationResults,
            T entity) where T : MainDocumentTestModel
        {
            var errors = new List<ValidationError>();

            foreach (var validationResult in validationResults)
            {
                string fieldName = validationResult.MemberNames.FirstOrDefault() ?? "";

                var errorToAdd = new ValidationError()
                {
                    Id = entity.Id,
                    ParentType = entity.GetType(),
                    FieldName = fieldName,
                    Error = validationResult.ErrorMessage
                };

                if (validationResult is ExtensionForDataAnnotations extensionResult && extensionResult.Results.Any())
                {
                    errorToAdd.Errors = GetErrors(extensionResult.Results.ToArray(), entity);
                }

                errors.Add(errorToAdd);
            }

            return errors;
        }

        public static IEnumerable<ValidationError> Validate(MainDocumentTestModel statement)
        {
            var errors = new List<ValidationError>();
            ValidationContext context = new ValidationContext(statement, null, null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool valid = Validator.TryValidateObject(statement, context, validationResults, true);
            if (!valid)
            {
                errors.AddRange(GetErrors<MainDocumentTestModel>(validationResults, statement));
            }

            return errors;
        }
    }
    public class ValidationError
    {
        public long Id { get; set; }
        public Type ParentType { get; set; }
        public string FieldName { get; set; }
        public string Error { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}
