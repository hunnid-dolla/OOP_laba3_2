using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_laba3_2
{
    public partial class Form1 : Form
    {
        // Модель данных, которая хранит значения A, B и C.
        private NumberModel _model;

        // Флаг для предотвращения циклических обновлений при изменении значений в интерфейсе.
        private bool _updating;

        // Конструктор формы. Инициализирует компоненты, подключает события и загружает данные из модели.
        public Form1()
        {
            InitializeComponent();

            // Создаем экземпляр модели и подписываемся на событие PropertyChanged,
            // чтобы обновлять представление при изменении данных в модели.
            _model = new NumberModel();
            _model.PropertyChanged += (s, e) => UpdateView();

            // Подключаем обработчики событий для элементов управления.
            HookEvents();

            // Обновляем представление, чтобы отобразить начальные значения.
            UpdateView();
        }

        // Метод для подключения обработчиков событий к элементам управления.
        private void HookEvents()
        {
            // A: TextBox, NumericUpDown и TrackBar для свойства A.
            textBoxA.Leave += (s, e) => TrySetModelValue(textBoxA, val => _model.A = val, _model.A);
            numericUpDownA.ValueChanged += (s, e) => { if (!_updating) _model.A = (int)numericUpDownA.Value; };
            trackBarA.Scroll += (s, e) => { if (!_updating) _model.A = trackBarA.Value; };

            // Обработка нажатия Enter для TextBoxA.
            textBoxA.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (int.TryParse(textBoxA.Text, out int value))
                    {
                        _model.A = value; // Модель сама корректирует значения A, B и C.
                    }
                    else
                    {
                        // Если введено некорректное значение, восстанавливаем предыдущее.
                        textBoxA.Text = _model.A.ToString();
                    }

                    // Обновляем представление.
                    UpdateView();
                }
            };

            // B: TextBox, NumericUpDown и TrackBar для свойства B.
            textBoxB.Leave += (s, e) => TrySetModelValue(textBoxB, val => _model.B = val, _model.B);
            numericUpDownB.ValueChanged += (s, e) => { if (!_updating) _model.B = (int)numericUpDownB.Value; };
            trackBarB.Scroll += (s, e) => { if (!_updating) _model.B = trackBarB.Value; };

            // Обработка нажатия Enter для TextBoxB.
            textBoxB.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (int.TryParse(textBoxB.Text, out int value))
                    {
                        // Ограничиваем значение B в диапазоне [A, C].
                        int clampedValue = Math.Max(_model.A, Math.Min(value, _model.C));
                        _model.B = clampedValue; // Обновляем значение в модели.
                    }
                    else
                    {
                        // Если введено некорректное значение, восстанавливаем предыдущее.
                        textBoxB.Text = _model.B.ToString();
                    }

                    // Обновляем представление, чтобы синхронизировать все элементы управления.
                    UpdateView();
                }
            };

            // C: TextBox, NumericUpDown и TrackBar для свойства C.
            textBoxC.Leave += (s, e) => TrySetModelValue(textBoxC, val => _model.C = val, _model.C);
            numericUpDownC.ValueChanged += (s, e) => { if (!_updating) _model.C = (int)numericUpDownC.Value; };
            trackBarC.Scroll += (s, e) => { if (!_updating) _model.C = trackBarC.Value; };

            // Обработка нажатия Enter для TextBoxC.
            textBoxC.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (int.TryParse(textBoxC.Text, out int value))
                    {
                        _model.C = value; // Модель сама корректирует значения A, B и C.
                    }
                    else
                    {
                        // Если введено некорректное значение, восстанавливаем предыдущее.
                        textBoxC.Text = _model.C.ToString();
                    }

                    // Обновляем представление.
                    UpdateView();
                }
            };

            // Сохраняем значения в настройках при закрытии формы.
            this.FormClosing += (s, e) => _model.SaveValues();
        }

        // Метод для безопасного обновления значения в модели через TextBox.
        private void TrySetModelValue(TextBox box, Action<int> setter, int fallback)
        {
            // Пытаемся преобразовать текст в число.
            if (int.TryParse(box.Text, out int value))
            {
                setter(value); // Устанавливаем значение в модели.
            }
            else
            {
                // Если преобразование не удалось, восстанавливаем предыдущее значение.
                box.Text = fallback.ToString();
            }
        }

        // Метод для обновления всех элементов управления на форме.
        private void UpdateView()
        {
            // Устанавливаем флаг, чтобы предотвратить циклические обновления.
            _updating = true;

            // Обновляем элементы управления для A, B и C.
            UpdateComponent(textBoxA, numericUpDownA, trackBarA, _model.A);
            UpdateComponent(textBoxB, numericUpDownB, trackBarB, _model.B);
            UpdateComponent(textBoxC, numericUpDownC, trackBarC, _model.C);

            // Ограничиваем диапазон значений для B (всегда A <= B <= C).
            numericUpDownB.Minimum = _model.A;
            numericUpDownB.Maximum = _model.C;
            trackBarB.Minimum = _model.A;
            trackBarB.Maximum = _model.C;

            // Снимаем флаг после завершения обновления.
            _updating = false;
        }

        // Метод для обновления одного набора элементов управления (TextBox, NumericUpDown, TrackBar).
        private void UpdateComponent(TextBox tb, NumericUpDown nud, TrackBar bar, int value)
        {
            // Устанавливаем текст в TextBox.
            tb.Text = value.ToString();

            // Устанавливаем значения в NumericUpDown и TrackBar, учитывая их границы.
            nud.Value = Math.Max(nud.Minimum, Math.Min(nud.Maximum, value));
            bar.Value = Math.Max(bar.Minimum, Math.Min(bar.Maximum, value));
        }
    }
}