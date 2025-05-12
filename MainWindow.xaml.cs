using System;
using System.Text;
using System.Windows;

namespace RightTriangleApp
{
    public partial class MainWindow : Window
    {
        private RightTriangle triangle1;
        private RightTriangle triangle2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool TryCreateTriangle1()
        {
            if (!TryParsePositiveDouble(TextBoxA1.Text, out double a) ||
                !TryParsePositiveDouble(TextBoxB1.Text, out double b))
            {
                MessageBox.Show("Введите корректные положительные числа для катетов треугольника 1.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            triangle1 = new RightTriangle(a, b);
            return true;
        }

        private bool TryCreateTriangle2()
        {
            if (!TryParsePositiveDouble(TextBoxA2.Text, out double a) ||
                !TryParsePositiveDouble(TextBoxB2.Text, out double b))
            {
                MessageBox.Show("Введите корректные положительные числа для катетов треугольника 2.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            triangle2 = new RightTriangle(a, b);
            return true;
        }

        private bool TryParsePositiveDouble(string text, out double value)
        {
            return double.TryParse(text, out value) && value > 0;
        }

        private void ButtonInc1_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle1()) return;
            triangle1 = ++triangle1;
            UpdateTriangle1Inputs();
            AppendOutput($"Треугольник 1 после ++: {triangle1}");
        }

        private void ButtonDec1_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle1()) return;
            triangle1 = --triangle1;
            UpdateTriangle1Inputs();
            AppendOutput($"Треугольник 1 после --: {triangle1}");
        }

        private void ButtonShow1_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle1()) return;
            ShowTriangleInfo(1, triangle1);
        }

        private void UpdateTriangle1Inputs()
        {
            TextBoxA1.Text = triangle1.A.ToString();
            TextBoxB1.Text = triangle1.B.ToString();
        }

        private void ButtonInc2_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle2()) return;
            triangle2 = ++triangle2;
            UpdateTriangle2Inputs();
            AppendOutput($"Треугольник 2 после ++: {triangle2}");
        }

        private void ButtonDec2_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle2()) return;
            triangle2 = --triangle2;
            UpdateTriangle2Inputs();
            AppendOutput($"Треугольник 2 после --: {triangle2}");
        }

        private void ButtonShow2_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle2()) return;
            ShowTriangleInfo(2, triangle2);
        }

        private void UpdateTriangle2Inputs()
        {
            TextBoxA2.Text = triangle2.A.ToString();
            TextBoxB2.Text = triangle2.B.ToString();
        }

        private void ButtonCompare_Click(object sender, RoutedEventArgs e)
        {
            if (!TryCreateTriangle1() || !TryCreateTriangle2()) return;

            var sb = new StringBuilder();
            sb.AppendLine("Сравнение треугольников по площади:");
            sb.AppendLine($"Площадь треугольника 1: {(double)triangle1}");
            sb.AppendLine($"Площадь треугольника 2: {(double)triangle2}");
            sb.AppendLine($"triangle1 <= triangle2: {triangle1 <= triangle2}");
            sb.AppendLine($"triangle1 >= triangle2: {triangle1 >= triangle2}");

            AppendOutput(sb.ToString());
        }

        private void ShowTriangleInfo(int num, RightTriangle triangle)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Треугольник {num}: {triangle}");
            sb.AppendLine($"Площадь: {triangle.CalculateArea()}");
            sb.AppendLine($"Площадь через double: {(double)triangle}");
            sb.AppendLine($"Существует: {(triangle ? "Да" : "Нет")}");
            AppendOutput(sb.ToString());
        }

        private void AppendOutput(string text)
        {
            TextBoxOutput.AppendText(text + Environment.NewLine + Environment.NewLine);
            TextBoxOutput.ScrollToEnd();
        }
    }
}
