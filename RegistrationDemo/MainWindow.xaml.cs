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
        // ЗАДАНИЕ ДЛЯ СТУДЕНТА (задание на паре — правка + Pull Request):
        //
        // 1. Прочитать имя из NameTextBox.Text, email из EmailTextBox.Text
        //    и дату рождения из BirthDatePicker.SelectedDate (это Nullable<DateTime> —
        //    проверьте, что дата вообще выбрана).
        // 2. Проверить email через EmailValidator.IsValid(...).
        // 3. Посчитать профиль участника: AgeCalculator.BuildProfile(birthDate).
        //    Метод вернёт RegistrationProfile — возраст, категорию, скидку и цену.
        // 4. Вывести результат в ResultTextBlock.Text, например:
        //       $"{name}: {profile.Description}"
        //    что даст строку вида
        //       "Иван: 20 лет 3 мес. 12 дн. — категория «Студент», скидка 35%, к оплате 7 800 ₽"
        //    или сообщить, что не так, если поля не заполнены / email некорректен.
        //
        // Реализация ниже — временная заглушка, замените её.

        //ResultTextBlock.Text = "TODO: обработчик ещё не реализован";


    }
}
