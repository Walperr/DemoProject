using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegistrationDemo.Tests;

[TestClass]
public class EmailValidatorTests
{
    [TestMethod]
    public void IsValid_CorrectEmail_ReturnsTrue()
    {
        // Arrange
        string email = "student@university.ru";

        // Act
        bool result = EmailValidator.IsValid(email);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValid_EmptyString_ReturnsFalse()
    {
        // Arrange
        string email = "";

        // Act
        bool result = EmailValidator.IsValid(email);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsValid_NoAtSymbol_ReturnsFalse()
    {
        // TODO: допишите тест по аналогии с примерами выше.
        // Arrange: email без символа "@", например "studentuniversity.ru"
        // Act: вызвать EmailValidator.IsValid
        // Assert: результат должен быть false
        Assert.Inconclusive("Тест ещё не реализован");
    }

    [TestMethod]
    public void IsValid_DomainWithoutDot_ReturnsFalse()
    {
        // TODO: допишите тест по аналогии с примерами выше.
        // Arrange: email вида "student@university" (без точки в домене)
        // Act: вызвать EmailValidator.IsValid
        // Assert: результат должен быть false
        Assert.Inconclusive("Тест ещё не реализован");
    }
}
