using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Reminders> reminders = new ObservableCollection<Reminders>();
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(50);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            int h = DateTime.Now.Hour;   
            int m = DateTime.Now.Minute; 
            foreach (Reminders r in reminders)
            {
                if (h == r.Hour && m == r.Minute && r.IsShown == false)
                {
                    MessageBox.Show($"{r.Hour}:{r.Minute} - {r.Text}",
                        "Напоминание", MessageBoxButton.OK, MessageBoxImage.Information);
                    r.IsShown = true;
                }
            }
            lbNames.ItemsSource = null;
            lbNames.ItemsSource = reminders;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtHour.Text, out int hour) && int.TryParse(txtMinute.Text, out int minute))
            {
                reminders.Add(new Reminders
                {
                    Hour = hour,
                    Minute = minute,
                    Text = txtText.Text,
                    IsShown = false
                });
            }
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (Reminders r in reminders)
                    {
                        writer.WriteLine($"{r.Hour}%%{r.Minute}%%{r.Text}%%{r.IsShown}");
                    }
                }

                MessageBox.Show("Файл сохранён!");
            }
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;

                string[] t = File.ReadAllLines(path);

                foreach (string line in t)
                {
                    string[] temp = line.Split("%%");

                    reminders.Add(new Reminders
                    {
                        Hour = Convert.ToInt32(temp[0]),
                        Minute = Convert.ToInt32(temp[1]),
                        Text = temp[2],
                        IsShown = Convert.ToBoolean(temp[3])
                    });
                }

                lbNames.ItemsSource = reminders;
                timer.Start();
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (lbNames.SelectedItem is Reminders selectedReminder)
            {
                reminders.Remove(selectedReminder); 
            }
        }
    }
}