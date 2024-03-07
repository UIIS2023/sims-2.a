using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ArrowCommandsViewModel
    {
        public ObservableCollection<TourViewModel> AllTours { get; set; }
        public ObservableCollection<TourViewModel> SerbianTours { get; set;  }
        public ObservableCollection<TourViewModel> SummerTours { get; set; }

        public bool IsButtonsEnabled { get; set; }
        public bool IsSecondButtonsEnabled { get; set; }
        public bool IsThirdButtonsEnabled { get; set; }

        private RelayCommand _leftArrowCommand;

        public RelayCommand LeftArrowCommand
        {
            get { return _leftArrowCommand; }
            set
            {
                _leftArrowCommand = value;
            }
        }

        private RelayCommand _rightArrowCommand;

        public RelayCommand RightArrowCommand
        {
            get { return _rightArrowCommand; }
            set
            {
                _rightArrowCommand = value;
            }
        }

        private RelayCommand _secondLeftArrowCommand;

        public RelayCommand SecondLeftArrowCommand
        {
            get { return _secondLeftArrowCommand; }
            set
            {
                _secondLeftArrowCommand = value;
            }
        }

        private RelayCommand _secondRightArrowCommand;

        public RelayCommand SecondRightArrowCommand
        {
            get { return _secondRightArrowCommand; }
            set
            {
                _secondRightArrowCommand = value;
            }
        }

        private RelayCommand _thirdLeftArrowCommand;

        public RelayCommand ThirdLeftArrowCommand
        {
            get { return _thirdLeftArrowCommand; }
            set
            {
                _thirdLeftArrowCommand = value;
            }
        }

        private RelayCommand _thirdRightArrowCommand;

        public RelayCommand ThirdRightArrowCommand
        {
            get { return _thirdRightArrowCommand; }
            set
            {
                _thirdRightArrowCommand = value;
            }
        }
        public ArrowCommandsViewModel(ObservableCollection<TourViewModel> allTours, ObservableCollection<TourViewModel> serbianTours, ObservableCollection<TourViewModel> summerTours) 
        {
            AllTours = allTours;
            SerbianTours= serbianTours;
            SummerTours= summerTours;
            CheckIfButtonsAreEnabled();
            LeftArrowCommand = new RelayCommand(Execute_LeftArrowCommand, CanExecuteMethod);
            RightArrowCommand = new RelayCommand(Execute_RightArrowCommand, CanExecuteMethod);
            SecondLeftArrowCommand = new RelayCommand(Execute_SecondLeftArrowCommand, CanExecuteMethod);
            SecondRightArrowCommand = new RelayCommand(Execute_SecondRightArrowCommand, CanExecuteMethod);
            ThirdLeftArrowCommand = new RelayCommand(Execute_ThirdLeftArrowCommand, CanExecuteMethod);
            ThirdRightArrowCommand = new RelayCommand(Execute_ThirdRightArrowCommand, CanExecuteMethod);
        }

        private void CheckIfButtonsAreEnabled()
        {
            IsButtonsEnabled = true;
            IsSecondButtonsEnabled= true;
            IsThirdButtonsEnabled= true;
            if(AllTours.Count >= 0 && AllTours.Count <=2)
            {
                IsButtonsEnabled = false;
            }
            if (SerbianTours.Count >= 0 && SerbianTours.Count <= 2)
            {
                IsSecondButtonsEnabled = false;
            }
            if (SummerTours.Count >= 0 && SummerTours.Count <= 2)
            {
                IsThirdButtonsEnabled = false;
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_RightArrowCommand(object obj)
        {
            int count = AllTours.Count;
            if (count > 0)
            {
                TourViewModel reserv = AllTours[0];
                for (int i = count - 1; i >= 1; i--)
                {
                    if (i == count - 1)
                    {
                        AllTours[0] = AllTours[i];
                        AllTours[i] = AllTours[i - 1];
                    }
                    else if (i == 1)
                    {
                        AllTours[i] = reserv;
                    }
                    else
                    {
                        AllTours[i] = AllTours[i - 1];
                    }
                }
            }
        }

        private void Execute_LeftArrowCommand(object obj)
        {
            int count = AllTours.Count;
            if (count > 0)
            {
                TourViewModel reserv = AllTours[count - 1];
                for (int i = 0; i < count - 1; i++)
                {
                    if (i == 0)
                    {
                        AllTours[count - 1] = AllTours[i];
                    }
                    else if (i == count - 2)
                    {
                        AllTours[i - 1] = AllTours[i];
                        AllTours[i] = reserv;
                    }
                    else
                    {
                        AllTours[i - 1] = AllTours[i];
                    }
                }
            }
        }
        private void Execute_SecondRightArrowCommand(object obj)
        {
            int count = SerbianTours.Count;
            if (count > 0)
            {
                TourViewModel reserv = SerbianTours[0];
                for (int i = count - 1; i >= 1; i--)
                {
                    if (i == count - 1)
                    {
                        SerbianTours[0] = SerbianTours[i];
                        SerbianTours[i] = SerbianTours[i - 1];
                    }
                    else if (i == 1)
                    {
                        SerbianTours[i] = reserv;
                    }
                    else
                    {
                        SerbianTours[i] = SerbianTours[i - 1];
                    }
                }
            }
        }

        private void Execute_SecondLeftArrowCommand(object obj)
        {
            int count = SerbianTours.Count;
            if (count > 0)
            {
                TourViewModel reserv = SerbianTours[count - 1];
                for (int i = 0; i < count - 1; i++)
                {
                    if (i == 0)
                    {
                        SerbianTours[count - 1] = SerbianTours[i];
                    }
                    else if (i == count - 2)
                    {
                        SerbianTours[i - 1] = SerbianTours[i];
                        SerbianTours[i] = reserv;
                    }
                    else
                    {
                        SerbianTours[i - 1] = SerbianTours[i];
                    }
                }
            }
        }
        private void Execute_ThirdRightArrowCommand(object obj)
        {
            int count = SummerTours.Count;
            if (count > 0)
            {
                TourViewModel reserv = SummerTours[0];
                for (int i = count - 1; i >= 1; i--)
                {
                    if (i == count - 1)
                    {
                        SummerTours[0] = SummerTours[i];
                        SummerTours[i] = SummerTours[i - 1];
                    }
                    else if (i == 1)
                    {
                        SummerTours[i] = reserv;
                    }
                    else
                    {
                        SummerTours[i] = SummerTours[i - 1];
                    }
                }
            }
        }

        private void Execute_ThirdLeftArrowCommand(object obj)
        {
            int count = SummerTours.Count;
            if (count > 0)
            {
                TourViewModel reserv = SummerTours[count - 1];
                for (int i = 0; i < count - 1; i++)
                {
                    if (i == 0)
                    {
                        SummerTours[count - 1] = SummerTours[i];
                    }
                    else if (i == count - 2)
                    {
                        SummerTours[i - 1] = SummerTours[i];
                        SummerTours[i] = reserv;
                    }
                    else
                    {
                        SummerTours[i - 1] = SummerTours[i];
                    }
                }
            }
        }
    }
}
