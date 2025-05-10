using Microsoft.Win32;
using Sportics.Migrations;
using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AddViewModel: BaseViewModel
    {
        public List<Membership> Memberships { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public byte[] PhotoData { get; set; }

        public ICommand SelectPhotoCommand { get; }

        public ICommand AddCommand { get; }

        public AddViewModel()
        {
            SelectPhotoCommand = new RelayCommand(obj => ExecuteSelectPhoto());
            AddCommand = new RelayCommand(obj => ExecuteAdd());
        }

        private void ExecuteSelectPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                PhotoData = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        public event Action RequestClose;

        private void ExecuteAdd()
        {
            if (int.TryParse(Price, out int parsedPrice))
            {
                DataWorker.AddMembership(FullName, ShortName, Category, Description, parsedPrice, PhotoData);
                
                RequestClose?.Invoke();
            }
            else
            {
                MessageBox.Show("Цена введена некорректно!");
            }
        }
    }
}
