using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using Team_Project_CS.Commands;
using Team_Project_CS.Models;

namespace Team_Project_CS.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
        public Student? SelectedStudent { get; set; }
        public Student NewStudent { get; set; } = new Student();

        public ICommand AddStudentCommand { get; set; }
        public ICommand SaveStudentsCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }

        public MainViewModel()
        {
            AddStudentCommand = new RelayCommand(p => AddStudent());
            SaveStudentsCommand = new RelayCommand(p => SaveStudents());
            DeleteStudentCommand = new RelayCommand(p => DeleteStudent(p));

            LoadStudentsFromFile();
        }

        private void AddStudent()
        {
            Students.Add(new Student
            {
                Name = NewStudent.Name,
                Age = NewStudent.Age,
                Grade = NewStudent.Grade,
                GradeValue = NewStudent.GradeValue
            });
            NewStudent = new Student();
            OnPropertyChanged(nameof(NewStudent));
        }

        private void DeleteStudent(object? parameter)
        {
            if (SelectedStudent != null)
            {
                Students.Remove(SelectedStudent);
                SelectedStudent = null;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private void SaveStudents()
        {
            try
            {
                var json = JsonSerializer.Serialize(Students);
                File.WriteAllText("students.json", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні студентів: {ex.Message}");
            }
        }

        private void LoadStudentsFromFile()
        {
            try
            {
                if (File.Exists("students.json"))
                {
                    var json = File.ReadAllText("students.json");
                    var loadedStudents = JsonSerializer.Deserialize<ObservableCollection<Student>>(json);

                    if (loadedStudents != null)
                    {
                        Students.Clear();
                        foreach (var student in loadedStudents)
                        {
                            Students.Add(student);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні студентів: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}