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
        string name = NameTextBox.Text.Trim();
        string email = EmailTextBox.Text.Trim();
        DateTime? birthDate = BirthDatePicker.SelectedDate;

        if (string.IsNullOrWhiteSpace(name))
        {
            ResultTextBlock.Text = "Ошибка: введите имя.";
            return;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            ResultTextBlock.Text = "Ошибка: введите email.";
            return;
        }

        if (!birthDate.HasValue)
        {
            ResultTextBlock.Text = "Ошибка: выберите дату рождения.";
            return;
        }

        if (!EmailValidator.IsValid(email))
        {
            ResultTextBlock.Text = "Ошибка: email имеет неверный формат.";
            return;
        }

        try
        {
            RegistrationProfile profile = AgeCalculator.BuildProfile(birthDate.Value);

            ResultTextBlock.Text = $"{name}: {profile.Description}";
        }
        catch (ArgumentOutOfRangeException)
        {
            ResultTextBlock.Text = "Ошибка: дата рождения не может быть в будущем.";
        }
        catch (Exception ex)
        {
            ResultTextBlock.Text = $"Произошла ошибка: {ex.Message}";
        }
    }
}