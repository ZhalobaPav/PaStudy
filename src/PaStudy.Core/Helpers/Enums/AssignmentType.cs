namespace PaStudy.Core.Helpers.Enums;

public enum AssignmentType: byte
{
    Quiz = 1,      // Тестування
    Task = 2,      // Звичайне завдання (завантаження файлу/текст)
    Reading = 3,   // Матеріал для ознайомлення (без оцінки)
    Laboratory = 4 // Лабораторна робота
}
