using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsValidationExtensions;
using static DataAnnotationsValidationExtensions.ExtensionForDataAnnotations;

namespace Tests.Model
{
    public class MainDocumentTestModel
    {
        public int Id { get; set; }

        public CountryEnum Country { get; set; }

        [RequiredIf("Country", CountryEnum.Russia, true, ErrorMessage = "Поле \"Адрес\" обязательно к заполнению для страны Россия")]
        [Display(Name = @"Адрес")]
        public string Address { get; set; }

        [InnIf("Country", CountryEnum.Russia, true, ErrorMessage = "- Некорректно заполнен ИНН/VATIN")]
        [Display(Name = @"ИНН/ВАТин")]
        public string Inn { get; set; }

        [ValidateListObjects(ErrorMessage = "В разделе  \"Опциональный список\" найдены ошибки:",
             IterationText = "В объекте № ", IsCanEmpty = true, IsEmptyError = "")]
        public IEnumerable<OptionalTestModel> OptionalObjects { get; set; }

        [ValidateListObjects(ErrorMessage = "В разделе  \"Обязательный список\" найдены ошибки:",
        IterationText = "В объекте № ", IsCanEmpty = false, IsEmptyError = "Список обязательных объектов пуст")]
        public IEnumerable<RequiredTestModel> RequiredObject { get; set; }


    }
}
