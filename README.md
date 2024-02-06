                                        DataAnnotationsValidationExtensions

DataAnnotationsValidationExtensions is a library of extensions for validating objects and their properties using attributes from the System.ComponentModel.DataAnnotations namespace. These extensions allow defining additional conditions for object validation, such as conditional validation, validation of object lists, and more. Examples of using each attribute can be found in the tests provided with the library.

Features: 

    1)RangeIfAttribute: Attribute to specify a range of values for a property based on the value of another property.
    2)RequiredAnyAttribute: Attribute indicating that at least one of the listed properties must have a value.
    3)ValidateObjectAttribute: Attribute providing the ability to validate nested objects.
    4)ValidateListObjectsAttribute: Attribute for validating lists of objects.
    5)RequiredIfAttribute: Attribute making a property required based on the value of another property.
    6)InnIfAttribute: Attribute for validating INN (Individual Taxpayer Identification Number) considering conditions.
