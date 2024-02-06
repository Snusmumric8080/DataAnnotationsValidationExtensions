using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAnnotationsValidationExtensions.ExtensionForDataAnnotations;

namespace Tests.Model
{
    public class OptionalTestModel
    {
        [Display(Name = @"OptionalFields")]
        [RequiredAny(new string[] { "OptionalField1", "OptionalField2", "OptionalField3" }, IsEverything = false,
            ErrorMessage = "- Необходимо заполнить хотя бы одно поле: \"OptionalField1\", \"OptionalField2\", \"OptionalField3\"")]
        public string OptionalField1 { get; set; }
        public string OptionalField2 { get; set; }
        public string OptionalField3 { get; set; }
    }
}
