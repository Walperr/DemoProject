using System;

namespace RegistrationDemo;

/// <summary>
/// Возрастная категория участника и скидка, которая ей соответствует.
/// </summary>
/// <param name="Name">Название категории — попадает в текст на форме.</param>
/// <param name="MinAge">Нижняя граница возраста, включительно. Верхняя граница отдельно
/// не хранится: ею считается нижняя граница следующей категории в таблице.</param>
/// <param name="DiscountPercent">Базовая скидка категории, %.</param>
public sealed record ParticipantTier(string Name, int MinAge, int DiscountPercent);

/// <summary>
/// Результат расчёта по дате рождения: точный возраст, категория и итоговая цена участия.
/// </summary>
public sealed record RegistrationProfile(
    int Years,
    int Months,
    int Days,
    int DaysToNextBirthday,
    ParticipantTier Tier,
    int DiscountPercent,
    decimal Price)
{
    /// <summary>Готовая строка для вывода на форму.</summary>
    public string Description =>
        $"{Years} {AgeCalculator.YearsWord(Years)} {Months} мес. {Days} дн. — " +
        $"категория «{Tier.Name}», скидка {DiscountPercent}%, к оплате {Price:N0} ₽";
}

/// <summary>
/// Расчёт возраста участника и стоимости участия со скидками.
/// </summary>
public static class AgeCalculator
{
    /// <summary>Стоимость участия до применения скидок.</summary>
    public const decimal BasePrice = 12000m;

    /// <summary>За сколько дней до дня рождения начинает действовать праздничный бонус.</summary>
    private const int BirthdayBonusWindowDays = 7;

    private const int BirthdayBonusPercent = 10;
    private const int RoundAgeBonusPercent = 5;
    private const int MaxDiscountPercent = 100;

    /// <summary>
    /// Возрастные категории, отсортированные по возрастанию нижней границы.
    /// Диапазон категории — от её MinAge до MinAge следующей категории.
    /// </summary>
    private static readonly ParticipantTier[] Tiers =
    [
        new ParticipantTier("Дошкольник", 0, 100),
        new ParticipantTier("Школьник", 7, 50),
        new ParticipantTier("Студент", 18, 30),
        new ParticipantTier("Взрослый", 24, 0),
        new ParticipantTier("Пенсионер", 60, 40),
    ];

    /// <summary>Полный расчёт на сегодняшний день.</summary>
    public static RegistrationProfile BuildProfile(DateTime birthDate) =>
        BuildProfile(birthDate, DateTime.Today);

    /// <summary>
    /// Полный расчёт на произвольную дату. Отдельный параметр <paramref name="today"/>
    /// нужен, чтобы результат можно было проверять unit-тестами.
    /// </summary>
    public static RegistrationProfile BuildProfile(DateTime birthDate, DateTime today)
    {
        if (birthDate > today)
            throw new ArgumentOutOfRangeException(nameof(birthDate), "Дата рождения не может быть в будущем.");

        (int years, int months, int days) = CalculateExactAge(birthDate, today);
        int daysToNextBirthday = DaysUntilNextBirthday(birthDate, today);

        ParticipantTier tier = ResolveTier(years);
        int discount = CalculateDiscount(tier, years, daysToNextBirthday);
        decimal price = BasePrice * (100 - discount) / 100m;

        return new RegistrationProfile(years, months, days, daysToNextBirthday, tier, discount, price);
    }

    /// <summary>Полных лет на сегодняшний день.</summary>
    public static int CalculateAge(DateTime birthDate) => CalculateAge(birthDate, DateTime.Today);

    /// <summary>Полных лет на указанную дату.</summary>
    public static int CalculateAge(DateTime birthDate, DateTime today) =>
        CalculateExactAge(birthDate, today).Years;

    /// <summary>Точный возраст: полных лет, месяцев и дней.</summary>
    internal static (int Years, int Months, int Days) CalculateExactAge(DateTime birthDate, DateTime today)
    {
        // Сколько календарных месяцев прошло, если считать по номерам месяцев.
        int totalMonths = (today.Year - birthDate.Year) * 12 + (today.Month - birthDate.Month);

        // Уточняем: если «месячная годовщина» в этом месяце ещё не наступила,
        // последний месяц не полный. AddMonths сам обрезает 31-е число до конца месяца.
        if (birthDate.AddMonths(totalMonths) > today)
            totalMonths--;

        DateTime lastAnniversary = birthDate.AddMonths(totalMonths);

        int years = totalMonths / 12;
        int months = totalMonths % 12;
        int days = (today - lastAnniversary).Days;

        return (years, months, days);
    }

    /// <summary>Сколько дней осталось до ближайшего дня рождения (0 — если он сегодня).</summary>
    internal static int DaysUntilNextBirthday(DateTime birthDate, DateTime today)
    {
        int yearsPassed = today.Year - birthDate.Year;

        // AddYears корректно переносит 29 февраля на 28-е в невисокосном году.
        DateTime anniversary = birthDate.AddYears(yearsPassed);
        if (anniversary < today)
            anniversary = birthDate.AddYears(yearsPassed + 1);

        return (anniversary - today).Days;
    }

    /// <summary>Подбирает возрастную категорию по таблице <see cref="Tiers"/>.</summary>
    internal static ParticipantTier ResolveTier(int years)
    {
        for (int i = 0; i < Tiers.Length; i++)
        {
            int lowerBound = Tiers[i].MinAge;
            int upperBound = i + 1 < Tiers.Length ? Tiers[i + 1].MinAge : int.MaxValue;

            // возраст должен попасть в диапазон от нижней границы до верхней
            if (years >= lowerBound && years <= upperBound)
                return Tiers[i];
        }

        // Подстраховка на случай криво заполненной таблицы: отдаём последнюю категорию,
        // чтобы расчёт не падал.
        return Tiers[^1];
    }

    /// <summary>Складывает скидку категории с бонусами и ограничивает результат сверху.</summary>
    internal static int CalculateDiscount(ParticipantTier tier, int years, int daysToNextBirthday)
    {
        int discount = tier.DiscountPercent;

        // бонус «скоро день рождения»
        if (daysToNextBirthday <= BirthdayBonusWindowDays)
            discount += BirthdayBonusPercent;

        // бонус за круглую дату: 20, 30, 40 лет...
        if (years > 0 && years % 10 == 0)
            discount += RoundAgeBonusPercent;

        return Math.Min(discount, MaxDiscountPercent);
    }

    /// <summary>Склонение слова «год»: 21 год, 22 года, 25 лет.</summary>
    internal static string YearsWord(int years)
    {
        if (years % 100 is >= 11 and <= 14)
            return "лет";

        return (years % 10) switch
        {
            1 => "год",
            2 or 3 or 4 => "года",
            _ => "лет",
        };
    }
}
