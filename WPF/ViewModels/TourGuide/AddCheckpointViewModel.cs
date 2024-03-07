using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Managers;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AddCheckpointViewModel : ViewModel
    {
        #region FieldProperties
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        #endregion

        private ObservableCollection<CheckpointCardViewModel> _checkpointCards;

        public ObservableCollection<CheckpointCardViewModel> CheckpointCards
        {
            get => _checkpointCards;
            set
            {
                if (_checkpointCards != value)
                {
                    _checkpointCards = value;
                    OnPropertyChanged("CheckpointCards");
                }
            }
        }

        private CheckpointCardViewModel? _selectedCard;

        public CheckpointCardViewModel? SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (_selectedCard != value)
                {
                    _selectedCard = value;
                    OnPropertyChanged("SelectedCard");
                }
            }
        }

        private ObservableCollection<string> _checkpointTypes;

        public ObservableCollection<string> CheckpointTypes
        {
            get => _checkpointTypes;
            set
            {
                if (_checkpointTypes != value)
                {
                    _checkpointTypes = value;
                    OnPropertyChanged("CheckpointTypes");
                }
            }
        }

        private string _buttonContent;

        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                if (_buttonContent != value)
                {
                    _buttonContent = value;
                    OnPropertyChanged("ButtonContent");
                }
            }
        }


        public CheckpointManager CheckpointManager { get; set; }


        public RelayCommand AddCheckpointCommand { get; set; }
        public RelayCommand DeleteCheckpointCommand { get; set; }
        public RelayCommand EditCheckpointCommand { get; set; }
        public  RelayCommand ClearCheckpointsCommand { get; set; }

        public AddCheckpointViewModel(ObservableCollection<CheckpointCardViewModel> checkpointsCards)
        {
            _checkpointCards = checkpointsCards;
            _checkpointTypes = InitializeCheckpointTypes();
            CheckpointManager = new CheckpointManager(CheckpointCards, CheckpointTypes);
            _buttonContent = "Add";

            AddCheckpointCommand = new RelayCommand(AddCheckpoint, CanExecuteMethod);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpoint, CanExecuteMethod);
            EditCheckpointCommand = new RelayCommand(EditCheckpoint, CanExecuteMethod);
            ClearCheckpointsCommand = new RelayCommand(ClearCheckpoints, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        public ObservableCollection<string> InitializeCheckpointTypes()
        {
            var checkpointsTypes = new ObservableCollection<string>
            {
                "START",
                "EXTRA",
                "END"
            };

            var containsStart = CheckpointCards.Any(c => c.Type == CheckpointType.START);
            var containsEnd = CheckpointCards.Any(c => c.Type == CheckpointType.END);

            if (containsStart)
            {
                checkpointsTypes.Remove("START");
            }

            if (containsEnd)
            {
                checkpointsTypes.Remove("END");
            }

            

            return checkpointsTypes;
        }


        private void AddCheckpoint(object sender)
        {
            if (ButtonContent == "Confirm")
            {
                var index = CheckpointCards.IndexOf(SelectedCard);
                CheckpointCards[index].Name = Name;
                if (Type == "START")
                {
                    if (CheckpointCards[index].Type == CheckpointType.EXTRA)
                    {
                        CheckpointCards.Move(index, 0);
                        index = 0;
                    }
                    else if (CheckpointCards[index].Type == CheckpointType.END)
                    {
                        CheckpointCards.Move(index, 0);
                        index = 0;
                    }
                    CheckpointCards[index].Type = CheckpointType.START;
                    CheckpointManager.CheckpointTypes.RemoveAt(CheckpointTypes.IndexOf("START"));
                }
                else if (Type == "EXTRA")
                {
                    CheckpointCards[index].Type = CheckpointType.EXTRA;
                }
                else if(Type == "END")
                {
                    if (CheckpointCards[index].Type == CheckpointType.EXTRA)
                    {
                        CheckpointCards.Move(index, CheckpointCards.Count-1);
                        index = CheckpointCards.Count - 1;
                    }
                    else if(CheckpointCards[index].Type == CheckpointType.START)
                    {
                        CheckpointCards.Move(index, CheckpointCards.Count - 1);
                        index = CheckpointCards.Count - 1;
                    }
                    CheckpointCards[index].Type = CheckpointType.END;
                    CheckpointManager.CheckpointTypes.RemoveAt(CheckpointTypes.IndexOf("END"));
                }

                CheckpointCards[index].Background = new SolidColorBrush(Colors.AliceBlue);
                CheckpointCards[index].CanEdit = true;
                CheckpointCards[index].CanDelete = true;
                CheckpointCards[index].CanEditImagePath = "/Resources/Icons/pencil.png";
                CheckpointCards[index].CanDeleteImagePath = "/Resources/Icons/bin.png";
                
                ButtonContent = "Add";
                SelectedCard = null;
            }
            else
            {
                CheckpointManager.AddCheckpoint(Name, Type);
            }
            Name = string.Empty;
            Type = string.Empty;
        }

        private void ClearCheckpoints(object sender)
        {
            CheckpointManager.ClearCheckpoints();

            Name = string.Empty;
            Type = string.Empty;
            ButtonContent = "Add";
        }

        private void EditCheckpoint(object sender)
        {
            var selectedCheckpoint = sender as CheckpointCardViewModel;

            if (SelectedCard != null)
            {
                var index = CheckpointCards.IndexOf(SelectedCard);
                CheckpointCards[index].Background = new SolidColorBrush(Colors.AliceBlue);
                CheckpointCards[index].CanEdit = true;
                CheckpointCards[index].CanDelete = true;
                CheckpointCards[index].CanEditImagePath = "/Resources/Icons/pencil.png";
                CheckpointCards[index].CanDeleteImagePath = "/Resources/Icons/bin.png";
            }

            SelectedCard = selectedCheckpoint;

            if (selectedCheckpoint.Type == CheckpointType.START)
            {
                CheckpointTypes.Insert(0,"START");
            }
            else if (selectedCheckpoint.Type == CheckpointType.END)
            {
                CheckpointTypes.Insert(CheckpointTypes.Count, "END");
            }

            Name = selectedCheckpoint.Name;
            Type = selectedCheckpoint.Type.ToString();
            selectedCheckpoint.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8FFB7"));
            selectedCheckpoint.CanDelete = false;
            selectedCheckpoint.CanEdit = false;
            selectedCheckpoint.CanEditImagePath = "/Resources/Icons/pencil_light.png";
            selectedCheckpoint.CanDeleteImagePath = "/Resources/Icons/bin_light.png";
            ButtonContent = "Confirm";
        }

        private void DeleteCheckpoint(object sender)
        {
            var selectedCheckpoint = sender as CheckpointCardViewModel;
            CheckpointCards.Remove(selectedCheckpoint);

            if (selectedCheckpoint.Type == CheckpointType.START)
            {
                CheckpointTypes.Insert(0, "START");
            }
            if (selectedCheckpoint.Type == CheckpointType.END)
            {
                CheckpointTypes.Insert(CheckpointTypes.Count, "END");
            }
        }
    }
}
