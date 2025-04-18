using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_laba3_2
{
    // Класс NumberModel реализует интерфейс INotifyPropertyChanged для уведомления об изменении свойств.
    public class NumberModel : INotifyPropertyChanged
    {
        // Приватные поля для хранения значений A, B и C.
        private int _a;
        private int _b;
        private int _c;

        // Событие PropertyChanged используется для уведомления о том, что одно или несколько свойств изменились.
        public event PropertyChangedEventHandler PropertyChanged;

        // Конструктор класса. При создании объекта загружаются сохраненные значения.
        public NumberModel()
        {
            LoadValues();
        }

        // Свойство A:
        // - Значение должно быть в диапазоне от 0 до 100 (ограничено методом Clamp).
        // - Если значение A увеличивается, то B и C также увеличиваются, чтобы оставаться >= A.
        public int A
        {
            get => _a; // Возвращает текущее значение A.
            set
            {
                // Сохраняем старые значения для проверки изменений.
                int oldA = _a, oldB = _b, oldC = _c;

                // Применяем ограничение на значение A (от 0 до 100).
                _a = Clamp(value);

                // Если B или C меньше нового значения A, они увеличиваются до значения A.
                if (_b < _a) _b = _a;
                if (_c < _a) _c = _a;

                // Уведомляем об изменениях, если хотя бы одно значение изменилось.
                NotifyIfChanged(oldA, oldB, oldC);
            }
        }

        // Свойство B:
        // - Значение должно находиться между A и C (включительно).
        public int B
        {
            get => _b; // Возвращает текущее значение B.
            set
            {
                // Ограничиваем новое значение B так, чтобы оно оставалось в диапазоне [A, C].
                int clamped = Math.Max(_a, Math.Min(value, _c));

                // Если новое значение B отличается от текущего, обновляем его.
                if (_b != clamped)
                {
                    // Сохраняем старые значения для проверки изменений.
                    int oldA = _a, oldB = _b, oldC = _c;
                    _b = clamped;

                    // Уведомляем об изменениях, если хотя бы одно значение изменилось.
                    NotifyIfChanged(oldA, oldB, oldC);
                }
            }
        }

        // Свойство C:
        // - Значение должно быть в диапазоне от 0 до 100 (ограничено методом Clamp).
        // - Если значение C уменьшается, то A и B также уменьшаются, чтобы оставаться <= C.
        public int C
        {
            get => _c; // Возвращает текущее значение C.
            set
            {
                // Сохраняем старые значения для проверки изменений.
                int oldA = _a, oldB = _b, oldC = _c;

                // Применяем ограничение на значение C (от 0 до 100).
                _c = Clamp(value);

                // Если A или B больше нового значения C, они уменьшаются до значения C.
                if (_b > _c) _b = _c;
                if (_a > _c) _a = _c;

                // Уведомляем об изменениях, если хотя бы одно значение изменилось.
                NotifyIfChanged(oldA, oldB, oldC);
            }
        }

        // Метод Clamp ограничивает значение в диапазоне [0, 100].
        private int Clamp(int value) => Math.Max(0, Math.Min(value, 100));

        // Метод NotifyIfChanged проверяет, изменились ли значения A, B или C,
        // и вызывает событие PropertyChanged, если изменения произошли.
        private void NotifyIfChanged(int oldA, int oldB, int oldC)
        {
            if (_a != oldA || _b != oldB || _c != oldC)
            {
                // Уведомляем подписчиков о том, что одно или несколько свойств изменились.
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        // Метод LoadValues загружает сохраненные значения A, B и C из настроек приложения.
        public void LoadValues()
        {
            _a = Properties.Settings.Default.A;
            _b = Properties.Settings.Default.B;
            _c = Properties.Settings.Default.C;

            // Уведомляем об изменениях после загрузки значений.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        // Метод SaveValues сохраняет текущие значения A, B и C в настройки приложения.
        public void SaveValues()
        {
            Properties.Settings.Default.A = _a;
            Properties.Settings.Default.B = _b;
            Properties.Settings.Default.C = _c;

            // Сохраняем изменения в настройках.
            Properties.Settings.Default.Save();
        }
    }
}
