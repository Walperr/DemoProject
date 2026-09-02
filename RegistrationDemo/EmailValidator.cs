namespace RegistrationDemo;

/// <summary>
/// Простая проверка формата email. Ошибок не содержит намеренно —
/// это класс для демонстрации unit-тестов (чистая функция без побочных эффектов).
/// </summary>
public static class EmailValidator
{
    public static bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        int at = email.IndexOf('@');
        if (at <= 0 || at == email.Length - 1)
            return false;

        string domain = email[(at + 1)..];
        return domain.Contains('.') && !domain.StartsWith('.') && !domain.EndsWith('.');
    }
}
