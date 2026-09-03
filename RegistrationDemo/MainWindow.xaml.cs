using System;
using System.Windows;

namespace RegistrationDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        // 1. Читаем данные из полей
        string name = NameTextBox.Text?.Trim() ?? string.Empty;
        string email = EmailTextBox.Text?.Trim() ?? string.Empty;
        DateTime? birthDate = BirthDatePicker.SelectedDate;

        // Проверяем имя
        if (string.IsNullOrWhiteSpace(name))
        {
            ResultTextBlock.Text = "Ошибка: имя не заполнено.";
            return;
        }

        // Проверяем email
        if (string.IsNullOrWhiteSpace(email) || !EmailValidator.IsValid(email))
        {
            ResultTextBlock.Text = "Ошибка: введите корректный email.";
            return;
        }

        // Проверяем дату рождения
        if (!birthDate.HasValue)
        {
            ResultTextBlock.Text = "Ошибка: дата рождения не выбрана.";
            return;
        }

        // Проверяем, что дата рождения не в будущем
        if (birthDate.Value > DateTime.Today)
        {
            ResultTextBlock.Text = "Ошибка: дата рождения не может быть в будущем.";
            return;
        }

        try
        {
            // 2. Строим профиль участника
            RegistrationProfile profile = AgeCalculator.BuildProfile(birthDate.Value);

            // 3. Выводим результат
            ResultTextBlock.Text = $"{name}: {profile.Description}";
        }
        catch (ArgumentOutOfRangeException ex)
        {
            ResultTextBlock.Text = $"Ошибка: {ex.Message}";
        }
        catch (Exception ex)
        {
            ResultTextBlock.Text = $"Произошла ошибка: {ex.Message}";
        }

    }
}
