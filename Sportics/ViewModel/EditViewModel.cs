using Microsoft.Win32;
using Sportics.Model;
using System;
using System.IO;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class EditViewModel : BaseViewModel
    {
        public Membership Membership { get; set; }

        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public byte[] PhotoData { get; set; }

        public ICommand SelectPhotoCommand { get; }

        public ICommand EditCommand { get; }

        public EditViewModel(Membership membership)
        {
            Membership = membership;
            SelectPhotoCommand = new RelayCommand(obj => ExecuteSelectPhoto());
            EditCommand = new RelayCommand(obj => EditMembership());
        }

        public EditViewModel() 
        {
            
        }

        public event Action RequestClose;

        private void EditMembership()
        {
            DataWorker.EditMembership(Membership, FullName, ShortName, Description, Category, Price, PhotoData);
            RequestClose?.Invoke();

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
    }
}
