using System;
using System.Text;
using System.Windows;

namespace RightTriangleApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnExecuteClick(object sender, RoutedEventArgs e)
        {
            TextBoxOutput.Clear();

            if (!TryParsePositiveDouble(TextBoxA1.Text, out double a1) ||
                !TryParsePositiveDouble(TextBoxB1.Text, out double b1) ||
                !TryParsePositiveDouble(TextBoxA2.Text, out double a2) ||
                !TryParsePositiveDouble(TextBoxB2.Text, out double b2))
            {
                MessageBox.Show("Введите корректные положительные числа для всех катетов. Например: 30,68", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RightTriangle triangle1 = new RightTriangle(a1, b1);
            RightTriangle triangle2 = new RightTriangle(a2, b2);

            var sb = new StringBuilder();

            sb.AppendLine("Первый треугольник:");
            AppendTriangleInfo(sb, triangle1);

            RightTriangle triangle1Inc = ++triangle1;
            sb.AppendLine($"После ++: {triangle1Inc}");

            RightTriangle triangle1Dec = --triangle1Inc;
            sb.AppendLine($"После --: {triangle1Dec}");

            double area1 = (double)triangle1;
            sb.AppendLine($"Площадь через приведение к double: {area1}");

            bool exists1 = triangle1;
            sb.AppendLine($"Треугольник существует: {exists1}");

            sb.AppendLine();

            sb.AppendLine("Второй треугольник:");
            AppendTriangleInfo(sb, triangle2);

            RightTriangle triangle2Inc = ++triangle2;
            sb.AppendLine($"После ++: {triangle2Inc}");

            RightTriangle triangle2Dec = --triangle2Inc;
            sb.AppendLine($"После --: {triangle2Dec}");

            double area2 = (double)triangle2;
            sb.AppendLine($"Площадь через приведение к double: {area2}");

            bool exists2 = triangle2;
            sb.AppendLine($"Треугольник существует: {exists2}");

            sb.AppendLine();

            sb.AppendLine("Сравнение треугольников по площади:");
            sb.AppendLine($"triangle1 <= triangle2: {triangle1 <= triangle2}");
            sb.AppendLine($"triangle1 >= triangle2: {triangle1 >= triangle2}");

            TextBoxOutput.Text = sb.ToString();
        }

        private bool TryParsePositiveDouble(string text, out double value)
        {
            return double.TryParse(text, out value) && value > 0;
        }

        private void AppendTriangleInfo(StringBuilder sb, RightTriangle triangle)
        {
            sb.AppendLine(triangle.ToString());
            sb.AppendLine($"Площадь: {triangle.CalculateArea()}");
        }
    }
}
