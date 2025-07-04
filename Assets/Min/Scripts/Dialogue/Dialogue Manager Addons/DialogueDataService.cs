using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using ChoiceDataClass = ProjectVS.Dialogue.ChoiceData.ChoiceData;
using ChoiceDataParserClass = ProjectVS.Dialogue.ChoiceDataParser.ChoiceDataParser;
using DialogueDataParserClass = ProjectVS.Dialogue.DialogueDataParser.DialogueDataParser;
using DialogueDataClass = ProjectVS.Dialogue.DialogueData.DialogueData;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;


namespace ProjectVS.Dialogue.DialogueManagerAddons.DialogueDataService
{
    public class DialogueDataService
    {
        public int DialogueCount => _dialogues.Count;

        private List<DialogueDataClass> _dialogues;
        private List<ChoiceDataClass> _choices;

        public void Load(string dialoguePath, string choicePath)
        {
            var dialogueTable = new CsvTable(dialoguePath, '\t');
            CsvReader.Read(dialogueTable);
            _dialogues = DialogueDataParserClass.Parse(dialogueTable);

            var choiceTable = new CsvTable(choicePath, '\t');
            CsvReader.Read(choiceTable);
            _choices = ChoiceDataParserClass.Parse(choiceTable);
        }

        public List<DialogueDataClass> GetAllDialogues() => _dialogues;

        public List<ChoiceDataClass> GetAllChoices() => _choices;

        public DialogueDataClass GetDialogueByID(int id)
        {
            return _dialogues.FirstOrDefault(d => d.ID == id);
        }

        public IEnumerable<DialogueDataClass> GetByTiming(int timing)
        {
            return _dialogues.Where(d => d.OccurTiming == timing);
        }

        public ChoiceDataClass GetChoiceByNextID(int nextID)
        {
            return _choices.FirstOrDefault(c => c.NextDialogueID == nextID);
        }
    }
}
