using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SOSTeam.TravelAgency.WPF.Managers
{
    /*
     Summary
     This class sort checkpoints
     */

    public class CheckpointManager : ViewModel
    {

        public ObservableCollection<CheckpointCardViewModel> CheckpointCards { get; set; }
        public ObservableCollection<string> CheckpointTypes { get; set; }
        public int MinNumOfCheckpoints { get; set; }
        public CheckpointManager(ObservableCollection<CheckpointCardViewModel> checkpointCards, ObservableCollection<string> checkpointTypes)
        {
            CheckpointCards = checkpointCards;
            CheckpointTypes = checkpointTypes;
        }

        public void AddCheckpoint(string name, string type)
        {
            var checkpointCard = new CheckpointCardViewModel
            {
                Name = name,
                Type = type switch
                {
                    "START" => CheckpointType.START,
                    "EXTRA" => CheckpointType.EXTRA,
                    _ => CheckpointType.END
                }
            };

            if (checkpointCard.Type == CheckpointType.START)
            {
                CheckpointTypes.Remove("START");
                if (CheckpointCards.Count > 0)
                {
                    CheckpointCards.Insert(0, checkpointCard);
                }
                else
                {
                    CheckpointCards.Add(checkpointCard);
                }
            }
            else if (checkpointCard.Type == CheckpointType.END)
            {
                CheckpointTypes.Remove("END");
                if (CheckpointCards.Count > 0)
                {
                    CheckpointCards.Insert(CheckpointCards.Count, checkpointCard);
                }
                else
                {
                    CheckpointCards.Add(checkpointCard);
                }
            }
            else if (checkpointCard.Type == CheckpointType.EXTRA)
            {
                if (CheckpointCards.Count == 0)
                {
                    CheckpointCards.Add(checkpointCard);
                }
                else if (CheckpointCards.Count == 1)
                {
                    if (CheckpointCards[0].Type == CheckpointType.START || CheckpointCards[0].Type == CheckpointType.EXTRA)
                    {
                        CheckpointCards.Add(checkpointCard);
                    }
                    else if (CheckpointCards[0].Type == CheckpointType.END)
                    {
                        CheckpointCards.Insert(CheckpointCards.Count - 1, checkpointCard);
                    }
                }
                else if (CheckpointCards.Count > 1)
                {
                    if (CheckpointCards[CheckpointCards.Count - 1].Type == CheckpointType.END)
                    {
                        CheckpointCards.Insert(CheckpointCards.Count - 1, checkpointCard);
                    }
                    else
                    {
                        CheckpointCards.Insert(CheckpointCards.Count, checkpointCard);
                    }
                }
            }
        }

        public void ClearCheckpoints()
        {
            CheckpointCards.Clear();
            InitializeCheckpointTypes();
        }

        public void InitializeCheckpointTypes()
        {
            CheckpointTypes.Clear();
            CheckpointTypes.Add("START");
            CheckpointTypes.Add("EXTRA");
            CheckpointTypes.Add("END");
        }

    }
}
