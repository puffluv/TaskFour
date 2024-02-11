using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskFour
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private DatabaseManager databaseManager;
        private ObservableCollection<Student> students;
        public MainWindow()
        {
            InitializeComponent();

            databaseManager = new DatabaseManager();
            students = new ObservableCollection<Student>();
            studentsDataGrid.ItemsSource = students;

            LoadStudents();
        }

        private void LoadStudents()
        {
            students.Clear();
            var studentsFromDb = databaseManager.GetStudents();
            foreach (var student in studentsFromDb)
            {
                students.Add(student);
            }
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Добавление студента в базу данных
            var newStudent = new Student { Name = "New Student" };
            databaseManager.InsertStudent(newStudent);

            // Перезагрузка данных и обновление DataGrid
            LoadStudents();
        }

        private void UpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранной строки
            if (studentsDataGrid.SelectedItem != null)
            {
                // Получение выбранного студента из DataGrid
                var selectedStudent = (Student)studentsDataGrid.SelectedItem;

                // Обновление имени студента
                databaseManager.UpdateStudent(selectedStudent.Id, selectedStudent.Name, selectedStudent.PhysicsGrade, selectedStudent.MathGrade);

                // Перезагрузка данных и обновление DataGrid
                LoadStudents();
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранной строки
            if (studentsDataGrid.SelectedItem != null)
            {
                // Получение выбранного студента из DataGrid
                var selectedStudent = (Student)studentsDataGrid.SelectedItem;

                // Удаление студента из базы данных
                databaseManager.DeleteStudent(selectedStudent.Id);

                // Перезагрузка данных и обновление DataGrid
                LoadStudents();
            } 
        }
    }
}
