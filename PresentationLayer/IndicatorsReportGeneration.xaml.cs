using DataPersistenceLayer;
using DataPersistenceLayer.Entities;
using DataPersistenceLayer.UnitsOfWork;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using Utilities;

namespace PresentationLayer
{
    /// <summary>
    /// Lógica de interacción para IndicatorsReportGeneration.xaml
    /// </summary>
    public partial class IndicatorsReportGeneration : Window
    {
        private List<Sector> _sectorsList;
        private List<Assignment> _assignmentsList = new List<Assignment>();
        public IndicatorsReportGeneration()
        {
            InitializeComponent();
        }

        private void BehindButtonClicked(object sender, RoutedEventArgs e)
        {
            GoBackToCoordinatorMenu();
        }

        private void GoBackToCoordinatorMenu()
        {
            CoordinatorMenu coordinatorMenu = new CoordinatorMenu();
            coordinatorMenu.Show();
            Close();
        }

        private async void GenerateReportButtonClickedAsync(object sender, RoutedEventArgs e)
        {
            if(TextBoxSavingPath.Text == "")
            {
                MessageBox.Show("Por favor ingrese una ruta de guardado válida");
            }
            else
            {
                ProfessionalPracticesContext professionalPracticesContext = new ProfessionalPracticesContext();
                UnitOfWork unitOfWork = new UnitOfWork(professionalPracticesContext);
                try
                {
                    LoadDataFromDatabase(unitOfWork);
                    GenerateReportAsync();
                    MessageBox.Show("El reporte fue generado con éxito");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Error en la conexión con la base de datos. Por favor, intente más tarde");
                }
                catch (EntityException)
                {
                    MessageBox.Show("Error en la conexión con la base de datos. Por favor, intente más tarde");
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("No se pudo generar el reporte");
                }
                finally
                {
                    unitOfWork.Dispose();
                    GoBackToCoordinatorMenu();
                }
            }
        }

        private void LoadDataFromDatabase(UnitOfWork unitOfWork)
        {
            _sectorsList = unitOfWork.Sectors.GetAll().ToList();
            switch (ComboBoxFilter.SelectedIndex)
            {
                case 0:
                    _assignmentsList = unitOfWork.Assignments.Find(assignment => assignment.Practicioner.User.UserStatus == UserStatus.ACTIVE)
                        .ToList();
                    break;
                case 1:
                    _assignmentsList = unitOfWork.Assignments.Find(assignment => assignment.Practicioner.User.UserStatus == UserStatus.INACTIVE)
                        .ToList();
                    break;
                case 2:
                    _assignmentsList = unitOfWork.Assignments.GetAll().ToList();
                    break;
            }
        }

        private async void GenerateReportAsync()
        {
            IndicatorsReportGenerator indicatorsReportGeneration = new IndicatorsReportGenerator(_assignmentsList, _sectorsList);
            FileInfo fileInfo = new FileInfo(TextBoxSavingPath.Text);
            await indicatorsReportGeneration.SaveExcelFile(fileInfo);
        }


        private void SearchButtonClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel document|*.xlsx";
            saveFileDialog.ShowDialog();
            string savingPath = saveFileDialog.FileName;
            TextBoxSavingPath.Text = savingPath;
        }
    }
}
