using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Model;

namespace Tests
{
    public class ValidateListObjectsTest
    {
        private MainDocumentTestModel _model;

        private const string OptionalErrorText = "В разделе  \"Опциональный список\" найдены ошибки:";
        private const string RequiredIsEmptyListErrorText = "Список обязательных объектов пуст";
        private const string RequiredIsNotValidListErrorText = "В разделе  \"Обязательный список\" найдены ошибки:";

        [SetUp]
        public void Setup()
        {
            _model = new MainDocumentTestModel();
        }

        private static IEnumerable<TestCaseData> OptionalCases()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new List<OptionalTestModel>());
        }

        [Test]
        [TestCaseSource(nameof(OptionalCases))]
        public void Validate_WhenOptionalObjectsListIsEmpty_ShouldNotReturnError(IEnumerable<OptionalTestModel> array)
        {
            _model.OptionalObjects = array;

            var errors = Helper.Validate(_model);
            Assert.IsFalse(errors.Any(e => e.Error == OptionalErrorText && e.FieldName == "OptionalObjects"));

        }

        private static IEnumerable<TestCaseData> OptionalCases2()
        {
            yield return new TestCaseData(new List<OptionalTestModel>()
            { 
                new OptionalTestModel()
            });

            yield return new TestCaseData(new List<OptionalTestModel>()
            {
                new OptionalTestModel()
                {
                    OptionalField1 = ""
                }
            });

            yield return new TestCaseData(new List<OptionalTestModel>()
            {
                new OptionalTestModel()
                {
                    OptionalField1 = null
                }
            });

            yield return new TestCaseData(new List<OptionalTestModel>()
            {
                new OptionalTestModel()
                {
                    OptionalField1 = " "
                }
            });
        }
        [Test]
        [TestCaseSource(nameof(OptionalCases2))]
        public void Validate_WhenOptionalObjectsListIsNotEmptyAndNotValid_ShouldReturnError(IEnumerable<OptionalTestModel> array)
        {
            _model.OptionalObjects = array;

            var errors = Helper.Validate(_model);
            Assert.IsTrue(errors.Any(e => e.Error == OptionalErrorText && e.FieldName == "OptionalObjects"));

        }

        private static IEnumerable<TestCaseData> RequiredCases()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new List<RequiredTestModel>());
        }

        [Test]
        [TestCaseSource(nameof(RequiredCases))]
        public void Validate_WhenRequiredObjectsListIsEmpty_ShouldReturnError(IEnumerable<RequiredTestModel> array)
        {
            _model.RequiredObject = array;

            var errors = Helper.Validate(_model);
            Assert.IsTrue(errors.Any(e => e.Error == RequiredIsEmptyListErrorText && e.FieldName == "RequiredObject"));

        }

        private static IEnumerable<TestCaseData> RequiredCases2()
        {
            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
            });

            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
               {
                   Object = new MyClass()
               }
            });

            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Nums = 0
                   }
               }
            });

            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Name = " "
                   }
               }
            });

            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Name = "Value",
                       Nums = 6
                   }
               },
               new RequiredTestModel()
            });
        }

        [Test]
        [TestCaseSource(nameof(RequiredCases2))]
        public void Validate_WhenRequiredObjectsListIsNotEmptyAndNotValid_ShouldReturnError(IEnumerable<RequiredTestModel> array)
        {
            _model.RequiredObject = array;

            var errors = Helper.Validate(_model);
            Assert.IsTrue(errors.Any(e => e.Error == RequiredIsNotValidListErrorText && e.FieldName == "RequiredObject"));

        }

        private static IEnumerable<TestCaseData> RequiredCases3()
        {
            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Name = "Name",
                       Nums = 10
                   }
               },
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Name = "Name",
                       Nums = 0
                   }
               }
            });
            yield return new TestCaseData(new List<RequiredTestModel>()
            {
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Name = "Value",
                       Nums = 1
                   }
               },
               new RequiredTestModel()
               {
                   Object = new MyClass()
                   {
                       Name = "Value",
                       Nums = 5
                   }
               }
            });
        }

        [Test]
        [TestCaseSource(nameof(RequiredCases3))]
        public void Validate_WhenRequiredObjectsListIsValid_ShouldNotReturnError(IEnumerable<RequiredTestModel> array)
        {
            _model.RequiredObject = array;

            var errors = Helper.Validate(_model);
            Assert.IsFalse(errors.Any(e => e.Error == RequiredIsEmptyListErrorText && e.FieldName == "RequiredObject"));

        }
    }
}
