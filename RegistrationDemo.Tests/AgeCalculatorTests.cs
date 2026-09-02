using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegistrationDemo.Tests;

[TestClass]
public class AgeCalculatorTests
{
    /// <summary>
    /// Все расчёты выполняются на фиксированную дату, чтобы результат тестов
    /// не зависел от дня, в который их запускают.
    /// </summary>
    private static readonly DateTime Today = new DateTime(2026, 9, 3);

    [TestMethod]
    public void CalculateAge_BirthdayAlreadyPassed_ReturnsFullYears()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 5, 10);

        // Act
        int age = AgeCalculator.CalculateAge(birthDate, Today);

        // Assert
        Assert.AreEqual(26, age);
    }

    [TestMethod]
    public void CalculateAge_BirthdayNotYetHappened_ReturnsFullYears()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 12, 20);

        // Act
        int age = AgeCalculator.CalculateAge(birthDate, Today);

        // Assert
        Assert.AreEqual(25, age);
    }

    [TestMethod]
    public void BuildProfile_SeventeenYearsOld_IsSchoolboyWithHalfPrice()
    {
        // Arrange — на 03.09.2026 человеку 17 лет
        DateTime birthDate = new DateTime(2008, 10, 20);

        // Act
        RegistrationProfile profile = AgeCalculator.BuildProfile(birthDate, Today);

        // Assert
        Assert.AreEqual(17, profile.Years);
        Assert.AreEqual("Школьник", profile.Tier.Name);
        Assert.AreEqual(50, profile.DiscountPercent);
        Assert.AreEqual(6000m, profile.Price);
    }

    [TestMethod]
    public void BuildProfile_TwentyYearsOld_GetsRoundAgeBonus()
    {
        // Arrange — 20 лет, круглая дата: 30% категории + 5% бонуса
        DateTime birthDate = new DateTime(2006, 5, 10);

        // Act
        RegistrationProfile profile = AgeCalculator.BuildProfile(birthDate, Today);

        // Assert
        Assert.AreEqual(20, profile.Years);
        Assert.AreEqual("Студент", profile.Tier.Name);
        Assert.AreEqual(35, profile.DiscountPercent);
    }

    [TestMethod]
    public void BuildProfile_EighteenYearsOld_IsStudent()
    {
        // Arrange — на 03.09.2026 человеку ровно 18 лет
        DateTime birthDate = new DateTime(2008, 1, 15);

        // Act
        RegistrationProfile profile = AgeCalculator.BuildProfile(birthDate, Today);

        // Assert
        Assert.AreEqual(18, profile.Years, "возраст посчитан неверно");
        Assert.AreEqual("Студент", profile.Tier.Name, "18 лет — это категория «Студент»");
        Assert.AreEqual(30, profile.DiscountPercent);
    }

    [TestMethod]
    public void CalculateAge_BornOnLeapDay_ReturnsFullYears()
    {
        // Arrange
        DateTime birthDate = new DateTime(2004, 2, 29);

        // Act
        int age = AgeCalculator.CalculateAge(birthDate, Today);

        // Assert
        Assert.AreEqual(22, age);
    }
}
