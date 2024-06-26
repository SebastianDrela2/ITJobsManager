﻿using AppliedJobsManager.DataManagement;
using AppliedJobsManager.Models;
using AppliedJobsManager.ViewModels;
using AppliedJobsManager.Views;
using System.Windows.Input;

namespace AppliedJobsManager.Commands.DataWindowCommands
{
    internal class DataOnOkClickedCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public AppliedJobsViewModel _appliedJobsViewModel;

        public DataOnOkClickedCommand(AppliedJobsViewModel appliedJobsViewModel)
        {
            _appliedJobsViewModel = appliedJobsViewModel;
        }

        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter)
        {
            var view = (DataWindow)parameter!;
            var newRow = new Row
            {
                Link = view._linkTextbox.Text,
                Job = view._jobTextBox.Text,
                Description = view._descriptionTextbox.Text,
                Pay = view._payTextBox.Text,
                Date = view._dateTextbox.Text
            };
            var invalidRowsNotifier = new InvalidRowsNotifier();

            if (!invalidRowsNotifier.TryNotifyRow(newRow))
            {
                _appliedJobsViewModel.Rows.Add(newRow);
                _appliedJobsViewModel.RowsAreOutdated = true;

                view.Close();
            }
        }
    }
}
