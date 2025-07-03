using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using DialogueDataClass = ProjectVS.Dialogue.DialogueData.DialogueData;
using DialogueDataServiceClass = ProjectVS.Dialogue.DialogueManagerAddons.DialogueDataService.DialogueDataService;
using ProjectVS.Dialogue.DialogueManagerR;

namespace ProjectVS.Dialogue.DialogueManagerAddons.DialogueConditionEvaluator
{
    public class DialogueConditionEvaluator
    {
        private DialogueDataServiceClass _dataService;
        private AffinityChecker _affinityChecker;

        public DialogueConditionEvaluator(DialogueDataServiceClass dataService, AffinityChecker affinityChecker)
        {
            _dataService = dataService;
            _affinityChecker = affinityChecker;
        }

        public bool CanReturnDialogue(DialogueType type)
        {
            return ReturnDialogue(type) != null;
        }

        public DialogueDataClass ReturnDialogue(DialogueType type)
        {
            int timing = (int)type;
            var candidates = _dataService.GetByTiming(timing);

            // Repeat와 StageClear는 특수 로직 처리 필요
            return type switch
            {
                DialogueType.Repeat => GetRepeatDialogue(),
                DialogueType.StageClear => GetStageClearDialogue(),
                _ => candidates.FirstOrDefault(d =>
                    _affinityChecker.CanShow(d) &&
                    !d.IsPrinted)
            };
        }

        private DialogueDataClass GetRepeatDialogue()
        {
            var allRepeat = _dataService.GetByTiming(3)
                .Where(d => d.IsRepeatable)
                .ToList();

            if (allRepeat.Count == 0)
                return null;

            var lastEvent = _dataService.GetByTiming(2)
                .Where(d => d.IsPrinted)
                .OrderByDescending(d => d.ID)
                .FirstOrDefault();

            if (lastEvent == null)
            {
                int minAffinity = allRepeat.Min(d => d.NeedAffinity);
                var lowestCandidates = allRepeat
                    .Where(d => d.NeedAffinity == minAffinity)
                    .ToList();

                return lowestCandidates[Random.Range(0, lowestCandidates.Count)];
            }

            int maxAllowedAffinity = lastEvent.NeedAffinity;

            var valid = allRepeat
                .Where(d => d.NeedAffinity <= maxAllowedAffinity)
                .ToList();

            if (valid.Count == 0)
                return null;

            int maxAffinity = valid.Max(d => d.NeedAffinity);
            var finalCandidates = valid
                .Where(d => d.NeedAffinity == maxAffinity)
                .ToList();

            return finalCandidates[Random.Range(0, finalCandidates.Count)];
        }

        private DialogueDataClass GetStageClearDialogue()
        {
            var candidates = _dataService.GetByTiming(4);

            foreach (var data in candidates)
            {
                if (!_affinityChecker.CanShow(data)) continue;
                if (data.IsPrinted) continue;
                if (HasUnshownPreviousEvent(data)) continue;

                return data;
            }

            return null;
        }

        private bool HasUnshownPreviousEvent(DialogueDataClass stageClearData)
        {
            if (stageClearData.ID == 100001) return false;
            if (stageClearData.NeedAffinity < 5) return false; // 5 이하일 때 조건 검사하지 않도록 하드코딩 됨

            return _dataService.GetByTiming(2).Any(data =>
                data.NeedAffinity < stageClearData.NeedAffinity &&
                _affinityChecker.CanShow(data) &&
                !data.IsPrinted);
        }
    }
}
