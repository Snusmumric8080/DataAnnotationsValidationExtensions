using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAnnotationsValidationExtensions.ExtensionForDataAnnotations;

namespace Tests.Model
{
    public class RequiredTestModel
    {
        [ValidateObject(ErrorMessage = "В поле Object найдены ошибки:")]
        public MyClass Object { get; set; }
    }
    public class MyClass()
    {
        [RangeIf(propertyName: "Name", value: "Value", min: 1, max: 5, isEquals: true, ErrorMessage = "Поле \"Nums\" заполнено некорректно")]
        public int Nums { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
